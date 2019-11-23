using System;
using System.Collections.Generic;

namespace DataLoggerApp.Models
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class DatasList
    {
        public int TotalCount { get; set; }
        public int ProcessedCount { get; set; }
        public List<Data> Data { get; set; }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Data
    {
        public Method Method { get; set; }
        public Process Process { get; set; }
        public string Layer { get; set; }
        public Creation Creation { get; set; }
        public string Type { get; set; }

        public bool IsProcessed { get; set; }

    }

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Method
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Assembly { get; set; }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Process
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Start Start { get; set; }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Start
    {
        public string Epoch { get; set; }
        public DateTime Date { get; set; }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Creation
    {
        public string Epoch { get; set; }
        public DateTime Date { get; set; }
    }
}