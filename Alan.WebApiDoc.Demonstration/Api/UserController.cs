using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Alan.WebApiDoc.Demonstration.Models;

namespace Alan.WebApiDoc.Demonstration.Api
{
    /// <summary>
    /// Person Api
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Get 
        /// </summary>
        /// <returns>get all user</returns>
        /// <example>Get /Api/User</example>
        /// <version>0.0.1.1</version>
        public IEnumerable<Person> Get()
        {
            return Person.PersonList;
        }
        /// <summary>
        /// Get special user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>speical user</returns>
        /// <example>Get /Api/User/3</example>
        public Person Get(int id)
        {
            return Person.PersonList.FirstOrDefault(p => p.Id == id);
        }
        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="entity">user</param>
        /// <returns>Added person</returns>
        /// <example>Post /Api/User | [Body] {Id:2, FirstName: "Tom", LastName: "Jack"} </example>
        public Person Post(Person entity)
        {
            Person.AddPerson(entity);
            return entity;
        }
        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="id">Person id</param>
        /// <returns>deleted or not</returns>
        /// <example>Delete /Api/User/3</example>
        public bool Delete(int id)
        {
            return Person.DeletePersons(id);
        }
    }
}