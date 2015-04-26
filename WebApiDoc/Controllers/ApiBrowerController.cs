using System.Web.Mvc;
using System.Web.Http;
using Alan.WebApiDoc.Models;

namespace Alan.WebApiDoc.Controllers
{
    public class ApiBrowerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //采用 http://blogs.msdn.com/b/yaohuang1/archive/2012/05/21/asp-net-web-api-generating-a-web-api-help-page-using-apiexplorer.aspx 的接口文档实现
        public ActionResult ApiDoc()
        {
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            return View(apiExplorer);
        }

        //返回所有的文档信息
        public JsonResult OriginalDoc()
        {
            var doc = DocumentModel.GetDocument(Server.MapPath("~/App_Data/WebApiDoc.xml"));
            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        //返回所有注册的接口信息
        public JsonResult AllApis()
        {
            var doc = DocumentModel.GetDocument(Server.MapPath("~/App_Data/WebApiDoc.xml"));    //获取文档信息
            var apis = ApiDescriptionModel.GetAllApis();                                        //获取所有API接口
            apis.ForEach(api => { api.BindDocModel(doc.membersNode.Members); });                //将文档信息绑定但API接口上
            return Json(apis, JsonRequestBehavior.AllowGet);
        }

        //接口文档页面示例
        public ActionResult ApiExplorer()
        {
            var doc = DocumentModel.GetDocument(Server.MapPath("~/App_Data/WebApiDoc.xml"));
            var apis = ApiDescriptionModel.GetAllApis();
            apis.ForEach(api => { api.BindDocModel(doc.membersNode.Members); });
            return View(apis);
        }
    }

}