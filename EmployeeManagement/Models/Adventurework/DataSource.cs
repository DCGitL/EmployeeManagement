using EmployeeManagement.Models.Adventurework.DemoScrollingPaging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

/// <summary>
/// Summary description for DataSource
/// </summary>
public  class DataSource
{
    private  string connectionStr;

    public  DataSource(string connectionStr)
    {
        this.connectionStr = connectionStr;
        //
        // TODO: Add constructor logic here
        //

    }
    public  IList<Employee> GetPagingEmployees(int pageNumber, int pageSize)
    {
        IList<Employee> employees = new List<Employee>();
        Employee employee = null;
        

        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetScrollingEmployee";
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PageNumber",
                    Value = pageNumber
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PageSize",
                    Value = pageSize
                });

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                using (SqlDataReader reader =  cmd.ExecuteReader())// ExecuteReader()) ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        employee = new Employee();
                        employee.EmployeeKey = Convert.ToInt32(reader["EmployeeKey"]);
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.Title = reader["Title"].ToString();
                        employee.HireDate = Convert.ToDateTime(reader["HireDate"]).ToShortDateString();

                        employees.Add(employee);
                    }
                }

            }
        }
        return employees;

        //JavaScriptSerializer js = new JavaScriptSerializer();
        //Context.Response.Write(js.Serialize(employees));
    }


    public async Task< IList<Employee>> GetAsyncPagingEmployees(int pageNumber, int pageSize)
    {
        IList<Employee> employees = new List<Employee>();
        Employee employee = null;


        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetScrollingEmployee";
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PageNumber",
                    Value = pageNumber
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PageSize",
                    Value = pageSize
                });

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())// ExecuteReader()) ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        employee = new Employee();
                        employee.EmployeeKey = Convert.ToInt32(reader["EmployeeKey"]);
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.Title = reader["Title"].ToString();
                        employee.HireDate = Convert.ToDateTime(reader["HireDate"]).ToShortDateString();

                        employees.Add(employee);
                    }
                }

            }
        }
        return employees;

        //JavaScriptSerializer js = new JavaScriptSerializer();
        //Context.Response.Write(js.Serialize(employees));
    }
}