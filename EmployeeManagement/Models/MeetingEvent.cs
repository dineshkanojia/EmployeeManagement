using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MeetingEvent
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }
    }

    public class MeetingData
    {
        public int id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public string text { get; set; }

        public string useremail { get; set; }
    }
}
