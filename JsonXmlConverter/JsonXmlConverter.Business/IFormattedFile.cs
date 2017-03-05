using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonXmlConverter.Business
{
    //define the file conversions that each FormattedContent implementation must
    //  support.
    public interface IFormattedContent
    {
        FileType FileType { get; }

        string Content { get; }

        IFormattedContent ToJsonContent();

        IFormattedContent ToXmlContent();
    }
}
