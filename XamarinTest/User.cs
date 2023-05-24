using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTest
{
    class User : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string [] Tasks { get; set; }
        public int NumberOfTasks { get; private set; }
        
        public void AddTask(string task)
        {
            if (NumberOfTasks < Tasks.Length)
            {
                Tasks[NumberOfTasks] = task;
                NumberOfTasks++;
            }
            else
            {
                throw new Exception("There are no place for another task");
            }
        }
        public User(string firstName, string lastName, string username, string password) : base(firstName, lastName)
        {
            this.Username = username;
            this.Password = password;
            this.Tasks = new string[100];
        }
        public User() { }
    }
}
