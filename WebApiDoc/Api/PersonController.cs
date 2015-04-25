using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApiDoc.Models;

namespace WebApiDoc.Api
{
    /// <summary>
    /// Person Api
    /// </summary>
    public class PersonController : ApiController
    {
        /// <summary>
        /// Get 
        /// </summary>
        /// <returns>get all persons</returns>
        /// <example>Get /Api/Person</example>
        /// <remarks>Must get request</remarks>
        public IEnumerable<Person> Get()
        {
            return Person.PersonList;
        }


        /// <summary>
        /// Get special page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count" example="10">Count per page</param>
        /// <returns>Person list</returns>
        /// <example><![CDATA[Get /Api/Person?page=1&count=10]]></example>
        /// <version>1.0.2</version>
        public IEnumerable<Person> GetByPage(int page, int count = 10)
        {
            var skip = (page - 1) * count;
            return Person.PersonList.Skip(skip).Take(count);
        }

        /// <summary>
        /// Get special person
        /// </summary>
        /// <param name="id" example="1">Person id</param>
        /// <returns></returns>
        /// <example>Get /Api/Person/3</example>
        /// <remarks>Must get request</remarks>
        public Person Get(int id)
        {
            return Person.PersonList.FirstOrDefault(p => p.Id == id);
        }
        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="entity">Person</param>
        /// <returns>Added person</returns>
        /// <example>Post /Api/Person | [Body] {Id:2, FirstName: "Tom", LastName: "Jack"} </example>
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
        /// <example>Delete /Api/Person/3</example>
        public bool Delete(int id)
        {
            return Person.DeletePersons(id);
        }
    }
}