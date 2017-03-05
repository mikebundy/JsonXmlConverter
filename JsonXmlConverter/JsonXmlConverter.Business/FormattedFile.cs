using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JsonXmlConverter.Business
{
    //The FormattedContent class and its decendents abstract away the messy work of converting
    //  file formats between each other. Support for additional formats would be added here in
    //  one location. For convenience, all decendent classes are listed below this abstract class.
    public abstract class FormattedContent : IFormattedContent
    {
        //Only this class and its decendents can set the file type and content
        //the format of the content
        public FileType FileType { get; protected set; }

        //the formatted content
        public string Content { get; protected set; }

        public FormattedContent(string content, FileType fileType)
        {
            FileType = fileType;
            Content = content;
        }

        public abstract IFormattedContent ToJsonContent();

        public abstract IFormattedContent ToXmlContent();

        //This static method drives the creation of each FormattedContent derived class. The format
        //  of the content argument is automatically detected and the FileType is set accordingly.
        public static IFormattedContent CreateFormattedContent(string content)
        {
            if(isXml(content))
            {
                return new XmlFormattedContent(content);
            }
            else if(isJson(content))
            {
                return new JsonFormattedContent(content);
            }
            else
            {
                return new UnknownFormattedContent(content);
            }
        }

        //is the string s valid XML?
        private static bool isXml(string s)
        {
            XDocument xml;
            try
            {
                xml = XDocument.Parse(s);
            }
            catch
            {
                return false;
            }

            return true;
        }

        //is the string s valid JSON?
        private static bool isJson(string s)
        {
            XDocument xml = new XDocument();
            try
            {
                xml = JsonConvert.DeserializeXNode(s);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

    //Represents content that is formatted as JSON. 
    public class JsonFormattedContent : FormattedContent
    {
        public JsonFormattedContent(string content) 
            : base(content, FileType.Json) { }

        //redundant method, as this is already in json format
        public override IFormattedContent ToJsonContent()
        {
            return CreateFormattedContent(this.Content);
        }

        //convert internal JSON content to XML and return a new XmlFormattedContent object
        public override IFormattedContent ToXmlContent()
        {
            return new XmlFormattedContent(JsonConvert.DeserializeXNode(this.Content).ToString());
        }
    }

    //Represents content that is formatted as XML
    public class XmlFormattedContent : FormattedContent
    {
        public XmlFormattedContent(string content)
            : base(content, FileType.Xml) { }

        //convert internal XML to JSON and return a new JsonFormattedContent object
        public override IFormattedContent ToJsonContent()
        {
            XDocument xml = XDocument.Parse(this.Content);
            string jsonContent = JsonConvert.SerializeXNode(xml, Formatting.Indented);
            return new JsonFormattedContent(jsonContent);
        }

        public override IFormattedContent ToXmlContent()
        {
            return new XmlFormattedContent(this.Content);
        }
    }

    //Represents content that is an undetected format. Contents are still saved, but
    //  FileType is unknown.
    public class UnknownFormattedContent : FormattedContent
    {
        public UnknownFormattedContent(string content)
            : base(content, FileType.Unknown) { }

        public override IFormattedContent ToJsonContent()
        {
            throw new Exception("Cannot convert to json. Unknown file format.");
        }

        public override IFormattedContent ToXmlContent()
        {
            throw new Exception("Cannot convert to json. Unknown file format.");
        }
    }
}
