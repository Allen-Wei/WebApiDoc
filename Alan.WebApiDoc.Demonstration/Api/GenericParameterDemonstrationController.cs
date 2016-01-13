using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Alan.WebApiDoc.Demonstration.Models;

namespace Alan.WebApiDoc.Demonstration.Api
{
    /// <summary>
    /// Generic parameter demonstration
    /// </summary>
    public class GenericParameterDemonstrationController : ApiController
    {

        /// <summary>
        /// receive a generic parameter
        /// </summary>
        /// <param name="para">generic parameter</param>
        /// <returns>object</returns>
        //public object Post(GenericParameter<Person> para)
        //{
        //    return para;
        //}

        /// <summary>
        /// receive a generic parameter
        /// </summary>
        /// <param name="parae" docName="Alan.WebApiDoc.Demonstration.Api.GenericParameterDemonstrationController.Put(System.Tuple{Alan.WebApiDoc.Demonstration.Models.Person,Alan.WebApiDoc.Demonstration.Api.GenericParameterDemonstrationController.GenericParameter{Alan.WebApiDoc.ApiDescriptionModel,Alan.WebApiDoc.Demonstration.Models.Person},System.String})">two generic parameter</param>
        /// <returns>object</returns>
        public object Put(GenericParameter<Person> parae)
        {
            return parae;
        }



        public class GenericParameter<T>
        {
            public string Json { get; set; }

            public T Model { get; set; }
        }

        public class GenericParameter<TModel, TOut>
        {
            public string Json { get; set; }

            public TModel Model { get; set; }
            public TOut Out { get; set; }
        }

    }
}
