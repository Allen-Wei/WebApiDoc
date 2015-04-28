using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;

namespace Alan.WebApiDoc
{

    //文档信息模型
    [XmlRoot("doc")]
    public class DocumentModel
    {
        [XmlElement("assembly")]
        public AssemblyNode assemblyNode { get; set; }

        [XmlElement("members")]
        public MembersNode membersNode { get; set; }

        public DocumentModel()
        {
            this.membersNode = new MembersNode();
        }

        public static DocumentModel GetDocument(string fileFullPath)
        {
            XmlSerializer serialize = new XmlSerializer(typeof(DocumentModel));
            var models = new DocumentModel();
            using (TextReader reader = new StreamReader(fileFullPath))
            {
                models = (DocumentModel)serialize.Deserialize(reader);
            }
            return models;
        }

        public class AssemblyNode
        {
            [XmlElement("name")]
            public string Name { get; set; }
        }
        public class MembersNode
        {
            [XmlElement("member")]
            public List<MemberNode> Members { get; set; }

            public MembersNode()
            {
                this.Members = new List<MemberNode>();
            }
        }
        public class MemberNode
        {


            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlElement("summary")]
            public string Summary { get; set; }
            [XmlElement("returns")]
            public string Returns { get; set; }

            [XmlElement("example")]
            public string Example { get; set; }
            [XmlElement("remarks")]
            public string Remarks { get; set; }
            [XmlElement("note")]
            public string Note { get; set; }
            [XmlElement("permission")]
            public string Permission { get; set; }
            [XmlElement("style")]
            public string Style { get; set; }


            [XmlElement("param")]
            public List<MemberParam> Parameters { get; set; }


            [XmlIgnore]
            public bool IsType
            {
                get
                {
                    return this.TypeId == "T";
                }
            }
            [XmlIgnore]
            public bool IsMember
            {
                get
                {
                    return this.TypeId == "M";
                }
            }

            [XmlIgnore]
            public string TypeId
            {
                get
                {
                    return (this.Name ?? " ").Substring(0, 1);
                }
            }

            [XmlIgnore]
            public string AddressInAssembly
            {
                get
                {
                    return (this.Name ?? "   ").Substring(2, this.Name.Length - 2);
                }
            }

            public MemberNode()
            {
                this.Parameters = new List<MemberParam>();
            }
        }

        public class MemberParam
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlText]
            public string Comment { get; set; }

            [XmlAttribute]
            public string IsRequire { get; set; }

            [XmlAttribute]
            public string DefaultValue { get; set; }
            [XmlAttribute]
            public string Example { get; set; }
            [XmlAttribute]
            public string Note { get; set; }
        }

    }



}