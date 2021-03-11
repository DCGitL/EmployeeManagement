using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using Xunit;

namespace Employeemanagement.Test
{
    public class LoginTest
    {
        private readonly Mock<IHttpContextAccessor> _accessor;
        public LoginTest()
        {
            _accessor = new Mock<IHttpContextAccessor>();

        }
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }
    }
}
