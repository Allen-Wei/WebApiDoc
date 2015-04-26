using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alan.WebApiDoc.Demonstration.Models
{
    public class Person
    {
        /// <summary>
        /// Person Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Person firstname
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Gender { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }


        public static List<Person> PersonList
        {
            get
            {
                var persons = HttpContext.Current.Application["Persons"] as List<Person>;
                if (persons == null)
                {
                    persons = new List<Person>(){
                        new Person(){
                            Id = 1,
                            FirstName  = "Alan",
                            LastName = "Way",
                            Age = 25,
                            Salary = 2300
},
new Person(){
    Id = 2,
    FirstName = "Allen",
    LastName = "Wei",
    Age = 24,
    Salary = 2000
    
},
new Person(){
    Id = 3,
    FirstName = "Annr",
    LastName = "Yu",
    Age = 28,
    Salary = 3000
}
                    };
                }
                PersonList = persons;
                return persons;
            }
            set
            {
                HttpContext.Current.Application["Persons"] = value;
            }
        }

        /// <summary>
        /// Add person
        /// </summary>
        /// <param name="p">person</param>
        public static void AddPerson(Person p)
        {
            var persons = PersonList;
            persons.Add(p);
            PersonList = persons;
        }
        public static bool DeletePersons(int id) {
            var persons = PersonList;
            var query = persons.FirstOrDefault(p => p.Id == id);
            if (query == null) { return false; }
            persons.Remove(query);
            PersonList = persons;
            return true;
        }

    }
}