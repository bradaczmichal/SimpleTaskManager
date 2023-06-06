using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class Tasks
    {
        public string Description { get; set; }
        public DateTime BeginDateTask { get; set; }
        public DateTime FinishDateTask { get; set; }
        public  int UserID {get; set;}

        public Tasks(string description, DateTime start, DateTime end, int userid)
        {
            this.Description = description;
            this.BeginDateTask = start;
            this.FinishDateTask = end;
            this.UserID = userid;
        }
    }
}
