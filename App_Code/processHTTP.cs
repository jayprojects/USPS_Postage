using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;

    public class processHTTP
    {
        private string resText;
        public processHTTP(String url)
        {
      //      string output = "";
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(url);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            // print out page source
            //Console.WriteLine(sb.ToString());

            resText= sb.ToString();

        }
        


        public static XmlDocument PostXMLTransaction(string v_strURL, XmlDocument v_objXMLDoc)
    {
        //Declare XMLResponse document
        XmlDocument XMLResponse = null;

        //Declare an HTTP-specific implementation of the WebRequest class.
        HttpWebRequest objHttpWebRequest;

        //Declare an HTTP-specific implementation of the WebResponse class
        HttpWebResponse objHttpWebResponse = null;

        //Declare a generic view of a sequence of bytes
        Stream objRequestStream = null;
        Stream objResponseStream = null;

        //Declare XMLReader
        XmlTextReader objXMLReader;

        //Creates an HttpWebRequest for the specified URL.
        objHttpWebRequest = (HttpWebRequest)WebRequest.Create(v_strURL);

        try
        {
            //---------- Start HttpRequest 
            string postData = "API=RateV4&XML=";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //Set HttpWebRequest properties
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(v_objXMLDoc.InnerXml);
            objHttpWebRequest.Method = "POST";
            objHttpWebRequest.ContentLength = bytes.Length+ byteArray.Length;
            objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";
            
            


            //Get Stream object 
            objRequestStream = objHttpWebRequest.GetRequestStream();
            objRequestStream.Write(byteArray, 0, byteArray.Length);
            //Writes a sequence of bytes to the current stream 
            objRequestStream.Write(bytes, 0, bytes.Length);

            //Close stream
            objRequestStream.Close();

            //---------- End HttpRequest

            //Sends the HttpWebRequest, and waits for a response.
            objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

            //---------- Start HttpResponse
            if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                //Get response stream 
                objResponseStream = objHttpWebResponse.GetResponseStream();

                //Load response stream into XMLReader
                objXMLReader = new XmlTextReader(objResponseStream);

                //Declare XMLDocument
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(objXMLReader);

                //Set XMLResponse object returned from XMLReader
                XMLResponse = xmldoc;

                //Close XMLReader
                objXMLReader.Close();
            }

            //Close HttpWebResponse
            objHttpWebResponse.Close();
        }
        catch (WebException we)
        {
            //TODO: Add custom exception handling
            throw new Exception(we.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            //Close connections
            objRequestStream.Close();
            objResponseStream.Close();
            objHttpWebResponse.Close();

            //Release objects
            objXMLReader = null;
            objRequestStream = null;
            objResponseStream = null;
            objHttpWebResponse = null;
            objHttpWebRequest = null;
        }

        //Return
        return XMLResponse;
    }







        public string ResponseText
        {
            get
            {
                return resText;
            }
        }

    }

