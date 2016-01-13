using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alan.WebApiDoc
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public class Interceptor
    {
        public static string[] GetPrameterNames(Type parameter)
        {
            if (!parameter.GenericTypeArguments.Any())
            {
                return new string[] { parameter.FullName };
            }

            var parametersTypes = parameter.GenericTypeArguments.Select(para =>
            {
                var genericArgs = para.GenericTypeArguments;
                if (genericArgs.Any())
                {
                    var gnrArgs = para.GenericTypeArguments.Select(gnrArg =>
                    {
                        if (gnrArg.GenericTypeArguments.Any())
                        {
                            return String.Join(",", GetPrameterNames(gnrArg));
                        }
                        return gnrArg.FullName;
                    }).ToList();
                    var fullName = String.Format("{0}{{{1}}}",
                        para.FullName.Split('`')[0],
                        String.Join(",", gnrArgs));
                    return fullName;
                }
                else
                {
                    return para.FullName;
                }
                return "";
            }).ToList();

            return parametersTypes.ToArray();
        }
    }
}
