using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace JsonXmlConverter.Business
{
    //The FormattedContent classes handle the work of converting string content between formats and
    //  can be used independently in combinations depending on the situation. This class implements the requirement that given
    //  XML content it must return a JSON translation and vice versa. Because this logic has nothing to do with
    //  HTTP, we can abstract this away so that an application *could* use this class directly without a service.
    public class DefaultJsonXmlConverter : IJsonXmlConverter
    {
        public IFormattedContent Convert(string s)
        {
            IFormattedContent content = FormattedContent.CreateFormattedContent(s);
            IFormattedContent convertedContent;

            if (content.FileType == FileType.Json)
            {
                convertedContent = content.ToXmlContent();
            }
            else if(content.FileType == FileType.Xml)
            {
                convertedContent = content.ToJsonContent();
            }
            else
            {
                convertedContent = content; //content is already unknown, so nothing to convert, just use the same object
            }

            return convertedContent;
        }
    }
}
