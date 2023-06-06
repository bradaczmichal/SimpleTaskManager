using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class Tasks
    {
        public static int TaskIdCounter { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime BeginDateTask { get; set; }
        public DateTime FinishDateTask { get; set; }
        public  int UserID {get; set;}

        public Tasks(string description, DateTime start, DateTime end, int userid)
        {
            TaskIdCounter++;
            this.Id = TaskIdCounter.ToString() + ". ";
            this.Description = description;
            this.BeginDateTask = start;
            this.FinishDateTask = end;
            this.UserID = userid;
        }
    }
}
