using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Responses
{
    public class ResponseModel<T>
    {

        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T Data { get; set; }
    }
}
