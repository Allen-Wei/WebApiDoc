using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Alan.WebApiDoc
{

    /// <summary>
    /// API描述
    /// </summary>
    public class ApiDescriptionModel
    {
        //接口的方法
        public string HttpMethod { get; set; }
        //接口的URL 
        public string RelativePath { get; set; }
        //接口所属的控制器名称
        public string ControllerName { get; set; }
        //接口所属的控制器类型
        public string ControllerType { get; set; }
        //接口的Action名称(方法名)
        public string ActionName { get; set; }
        //接口在程序集中的路径地址
        public string AddressInAssembly
        {
            get
            {
                var parameters = String.Empty;
                if (this.Parameters.Any())
                {
                    parameters = String.Format("({0})", String.Join(",", this.Parameters.Select(p => p.TypeFullName)));
                }
                return String.Format("{0}.{1}{2}", this.ControllerType, this.ActionName, parameters);
            }
        }
        //接口需要的参数
        public List<ApiDescriptionModel.ParameterDescription> Parameters { get; set; }
        //接口携带的文档信息
        public DocumentModel.MemberNode DocDescription { get; set; }

        //接口参数的描述信息
        public class ParameterDescription
        {
            //参数的名称
            public string Name { get; set; }
            //参数的来源(来自Url或者Body)
            public string Source { get; set; }
            //参数的类型
            public string TypeName { get; set; }
            //参数的类型全称
            public string TypeFullName { get; set; }
            public string Format { get; set; }
            public Type ParaType { get; set; }



            //参数携带的文档信息
            public DocumentModel.MemberParam DocMemPara { get; set; }
        }

        //获取所有的接口
        public static List<ApiDescriptionModel> GetAllApis()
        {
            var query = from api in System.Web.Http.GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions
                        select new ApiDescriptionModel()
                        {
                            HttpMethod = api.HttpMethod.ToString(),
                            RelativePath = api.RelativePath,
                            ControllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName,
                            ControllerType = api.ActionDescriptor.ControllerDescriptor.ControllerType.FullName,
                            ActionName = api.ActionDescriptor.ActionName,
                            Parameters = (from para in api.ParameterDescriptions
                                          select new ApiDescriptionModel.ParameterDescription()
                                          {
                                              Name = para.Name,
                                              Source = para.Source.ToString(),
                                              ParaType = para.ParameterDescriptor.ParameterType,
                                              TypeName = para.ParameterDescriptor.ParameterType.Name,
                                              TypeFullName = para.ParameterDescriptor.ParameterType.FullName
                                          }).ToList()
                        };
            return query.ToList();
        }

        public ApiDescriptionModel()
        {
            this.Parameters = new List<ParameterDescription>();
        }

        //将文档信息绑定到接口信息上
        public void BindDocModel(DocumentModel.MemberNode member)
        {
            this.DocDescription = member;
            if (member.Parameters == null) { member.Parameters = new List<DocumentModel.MemberParam>(); }
            foreach (var apiPara in this.Parameters)
            {
                foreach (var docPara in member.Parameters)
                {
                    if (docPara.Name == apiPara.Name)
                    {
                        apiPara.DocMemPara = docPara;
                        break;
                    }
                }
            }
        }
        public void BindDocModel(List<DocumentModel.MemberNode> members)
        {
            var member = members.FirstOrDefault(mem => mem.AddressInAssembly == this.AddressInAssembly);
            if (member == null) return;
            BindDocModel(member);
        }
    }


}