using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class User : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Tasks> TasksList { get; set; }
        
        public void AddTask(string description, DateTime start, DateTime end, int userid)
        {
            Tasks task = new Tasks(description, start, end, userid);
            TasksList.Add(task);
        }
        
        public User(string firstName, string lastName, string username, string password) : base(firstName, lastName)
        {
            this.Username = username;
            this.Password = password;
            this.TasksList = new List<Tasks>();
        }
        public User() { }
    }
}
