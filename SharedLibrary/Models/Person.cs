using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Person
    {
        public Person()
        {

        }

        public Person(string firstName, string lastName, string age, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            City = city;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string FullInfo => $"{FirstName} {LastName} {Age} {City}";
        
    }
}
