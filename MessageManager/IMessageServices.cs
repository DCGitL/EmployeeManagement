using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager
{
    public interface IMessageServices
    {
        void SendEmail(object Content);
    }
}
