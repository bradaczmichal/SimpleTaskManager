using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class Tasks
    {
        private static int TaskIdCounter { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }

        public Tasks(string description)
        {
            TaskIdCounter++;
            this.Id = TaskIdCounter.ToString() + ". ";
            this.Description = description;
        }
    }
}
