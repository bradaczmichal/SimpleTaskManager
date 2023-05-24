using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class User : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Tasks> Tasks { get; set; }
        
        public void AddTask(string description)
        {
            Tasks task = new Tasks(description);
            Tasks.Add(task);
        }
        public User(string firstName, string lastName, string username, string password) : base(firstName, lastName)
        {
            this.Username = username;
            this.Password = password;
            this.Tasks = new List<Tasks>();
        }
        public User() { }
    }
}
