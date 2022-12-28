using System;

namespace CompFirm.Dto.Requests
{
    public class RequestShortInfoDto
    {
        public string Number { get; set; }

        public string UserName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime PeriodExecution { get; set; }

        public string Status { get; set; }

        public double Sum { get; set; }
    }
}
