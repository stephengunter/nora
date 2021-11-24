using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Entities;

namespace ApplicationCore.Exceptions
{
    public class EmailSendFailed : Exception
    {
        public EmailSendFailed(string msg) : base(msg)
        {

        }
    }
}
