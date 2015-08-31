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
        /// <summary>
        /// 调用接口使用的的 HTTP Method
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 接口的URL 
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 接口所属的控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 接口所属的控制器类型
        /// </summary>
        public string ControllerType { get; set; }
        /// <summary>
        /// 接口的Action名称(方法名)
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 获取接口在程序集中的路径地址
        /// </summary>
        public string AddressInAssembly
        {
            get
            {
                var parameters = String.Empty;
                if (this.Parameters.Any())
                {
                    var paraArray = this.Parameters.Select(para =>
                    {
                        var paraTypeFullName = para.TypeFullName;
                        var genericArgs = para.ParaType.GenericTypeArguments;
                        if (genericArgs.Any())
                        {
                            //拥有泛型参数
                            var methodName = para.ParaType.FullName.Split('+')[0] + "." + para.ParaType.Name.Split('`')[0];
                            var genericArgsString = String.Join(",", genericArgs.Select(arg => arg.FullName));
                            paraTypeFullName = String.Format("{0}{{{1}}}", methodName, genericArgsString);
                        }
                        return paraTypeFullName;
                    }).ToArray();
                    parameters = String.Format("({0})", String.Join(",", paraArray));
                }
                return String.Format("{0}.{1}{2}", this.ControllerType, this.ActionName, parameters);
            }
        }
        /// <summary>
        /// 接口需要的参数
        /// </summary>
        public List<ApiDescriptionModel.ParameterDescription> Parameters { get; set; }
        /// <summary>
        /// 接口携带的文档信息
        /// </summary>
        public DocumentModel.MemberNode DocDescription { get; set; }

        /// <summary>
        /// 接口参数的描述信息
        /// </summary>
        public class ParameterDescription
        {
            /// <summary>
            /// 参数的名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 参数的来源(来自Url或者Body)
            /// </summary>
            public string Source { get; set; }
            /// <summary>
            /// 参数的类型名称
            /// </summary>
            public string TypeName { get; set; }
            /// <summary>
            /// 参数的类型全称
            /// </summary>
            public string TypeFullName { get; set; }
            /// <summary>
            /// 参数的JSON格式
            /// </summary>
            public string Format { get; set; }
            /// <summary>
            /// 参数类型
            /// </summary>
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
            if (member == null)
            {
                //fix bug
                member = members.FirstOrDefault(mem => mem.AddressInAssembly == this.AddressInAssembly.Replace('+', '.'));
            }
            if (member == null)
            {
                member = new DocumentModel.MemberNode();
            }
            BindDocModel(member);
        }
    }


}