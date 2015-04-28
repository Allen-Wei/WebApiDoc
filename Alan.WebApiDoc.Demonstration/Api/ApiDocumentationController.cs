using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Alan.WebApiDoc.Demonstration.Api
{
    public class ApiDocumentationController : ApiController
    {

        private List<DocumentModel.MemberNode> GetDocs
        {
            get
            {
                var docModels = HttpContext.Current.Cache.Get("_ApiDocumentModels") as List<DocumentModel.MemberNode>;
                if (docModels == null)
                {
                    var doc = DocumentModel.GetDocument(HttpContext.Current.Server.MapPath("~/App_Data/WebApiDoc.XML")).membersNode.Members;
                    HttpContext.Current.Cache.Add("_ApiDocumentModels", doc, null, DateTime.MaxValue, TimeSpan.FromDays(1),
                        System.Web.Caching.CacheItemPriority.Default, null);
                    docModels = doc;
                }
                return docModels;
            }
        }
        private List<ApiDescriptionModel> GetApis
        {
            get
            {
                var apiModels = HttpContext.Current.Cache.Get("_ApiDescriptionModel") as List<ApiDescriptionModel>;
                if (apiModels == null)
                {
                    var doc = GetDocs;
                    var apis = ApiDescriptionModel.GetAllApis();
                    apis.ForEach(api =>
                    {
                        api.BindDocModel(doc);
                        api.Parameters.ForEach(para =>
                        {
                            var t = para.ParaType;
                            if (t != null)
                            {
                                try
                                {
                                    var instance = Activator.CreateInstance(t);
                                    para.Format = Newtonsoft.Json.JsonConvert.SerializeObject(instance);
                                }
                                catch(Exception ex)
                                {
                                    para.Format = ex.Message;
                                }
                            }
                        });
                    });
                    HttpContext.Current.Cache.Add("_ApiDescriptionModel", apis, null, DateTime.MaxValue, TimeSpan.FromDays(1),
                        System.Web.Caching.CacheItemPriority.Default, null);
                    apiModels = apis;
                }
                return apiModels;
            }
        }

        public IEnumerable<ApiDescriptionModel> Get()
        {
            var apis = GetApis;
         
            return apis;
        }

        public object Get(string id)
        {

            var apis = GetApis.Where(api => api.ControllerName == id).ToList();
           
            var controller =
                GetDocs.FirstOrDefault(c => c.IsType && c.Name.EndsWith(String.Format(".{0}Controller", id)));
            return new { apis, controller };
        }

        public IEnumerable<object> GetList(string action)
        {

            if (action == "controllers") return GetApis.Select(api => api.ControllerName).Distinct();
            return new List<object>();
        }
    }
}
