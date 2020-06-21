using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class ResponseMessage
    {
        public int ResponseCode { get; set; }
        public string Detail { get; set; }
        public bool IsSuccess { get; set; }
    }
}
