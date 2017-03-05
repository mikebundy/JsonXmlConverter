using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonXmlConverter.Business
{
    public interface IJsonXmlConverter
    {
        IFormattedContent Convert(string s);
    }
}
