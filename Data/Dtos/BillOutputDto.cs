using System;
using System.Collections.Generic;

namespace Marigold
{
    public class BillOutputDto
    {
        public List<BillableServiceOutputDto> Services { get; set; }

        public string CustomerName { get; set; }

        public DateTime CheckinDate { get; set; }

        public int Total { get; set; }
    }
}