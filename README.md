# JsonXmlConverter

## General Architecture
<i>The JsonXmlConverter consists of four parts (each its own project in the solution):</i>

<b>JsonXmlConverter.Business:</b> The business logic encapsulates all file reading and translation logic independent of any particular interface.

<b>JsonXmlConverter.WebApi:</b> This is a HTTP service that wraps the business logic and exposes it in a REST fashion.

<b>JsonXmlConverter.Html:</b> This is a single HTML page that serves as an interface to the Web API. A user can select a file and upload it to the service. The web service response is then displayed to the user.

<b>JsonXmlConverter.Console:</b> Similar to the HTML page, this is also an interface to the web service. The user is allowed to type the name of a file, and, similar to the HTML project, the file is uploaded to the Web API. The service response is then displayed to the user.

## Design Decisions

<b>Architecture:</b> As discussed above, each interface (console and HTML) make use of the Web API to perform the work. My view was that anyone should be able to create a client that uses the service and the HTML and console clients were examples of this. However, the console would be able to use the Business logic library directly and get the same results. I preferred leaving both these options open, so the business logic is completely seperate from any web request/response logic.

<b>File Conversion</b> The file conversion code could have been more procedural in nature and simply been done inline inside the DefaultJsonXmlConverter.Convert class method. However, I decided to leave the design more flexible to support other future file formats which would have made the DefaultJsonXmlConverter.Convert method grow with each additional supported format. It would have grown messier over time. I decided to support file conversion by creating separate classes for each file format and forcing them (via an interface) to implement their own translations. Adding a new file format would then simply be a matter of adding a new class with translations, and, if all existing formats then needed to be able to convert to this new format, we can easily enforce this by adding the new conversion method to the interface. We would then know exactly what needs to be implemented in each class, as the compiler would tell us what was missing.

Another positive side effect of implementing each file type and translations as its own class is that it can be used in other programs in any combination, and not just for translating XML to JSON and back again. You could potentially translate a CSV (a new format) to JSON, XML to CSV, etc.

This design is more complicated than a simpler procedural implementation of the simple requirement of JSON to XML and vice versa, and though it does allow flexibility for the code in the future, an argument of YAGNI (Ya Ain't Gonna Need It) would indeed be valid.
