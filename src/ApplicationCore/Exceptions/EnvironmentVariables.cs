using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    public class EnvironmentVariableNotFound : Exception
    {
        public EnvironmentVariableNotFound(string key) : base($"key : {key}")
        {

        }
    }
}
