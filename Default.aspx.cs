using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    string uspsUserId = "711JAY000663";
    string Service;
    string ZipOrigination;
    string ZipDestination;
    string Pounds;
    string Ounces;
    string Container;
    //string Size;
    string Width;
    string Length;
    string Height;
   // string Girth;
    string packageType;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    void loadData()
    {
        ZipOrigination = TextBoxZipForm.Text;
        ZipDestination = TextBoxZipTo.Text;
        Service = DropDownListService.SelectedValue;
        Pounds = TextBoxPounds.Text;
        Ounces = TextBoxOunces.Text;
        packageType = DropDownListPackageType.SelectedValue;
        
        if (DropDownListPackageType.SelectedValue.Equals("PARCEL"))
        {
            Width = TextBoxWidth.Text;
            Height = TextBoxHeight.Text;
            Length = TextBoxLenght.Text;
            Container = "RECTANGULAR";
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        loadData();
        XmlDocument doc = getXml();
        doc = PostXMLTransaction("http://production.shippingapis.com/ShippingAPI.dll", doc);
        XmlNodeList xnList = doc.SelectNodes("/RateV4Response/Package/Error");
        if (null != xnList && xnList.Count>0)
        {
            XmlNode desc = xnList.Item(0).ChildNodes.Item(2);
            Response.Write("<h3> Error: " + desc.InnerText + "</h3>");
        }
        else
        {

            xnList = doc.SelectNodes("/RateV4Response/Package/Postage/Rate");
            String p = xnList.Item(0).InnerText;

            //Response.Write(FormatXMLString(rdoc) + "<br/> Shipping Cost: " + Rate.InnerText);
            Response.Write("<h2> Shipping Cost: " + p + "</h2>");
        }
    }

    protected void DropDownListShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListPackageType.SelectedValue.Equals("PARCEL"))
            PanelDimension.Visible = true;
        else
            PanelDimension.Visible = false;
    }


    private XmlDocument getXml()
    {
        XmlDocument doc = new XmlDocument();

        XmlNode RateV4Request = doc.CreateElement("RateV4Request");
        XmlAttribute USERID = doc.CreateAttribute("USERID");
        USERID.Value = uspsUserId;
        RateV4Request.Attributes.Append(USERID);

        {
            XmlNode Revision = doc.CreateElement("Revision");
            RateV4Request.AppendChild(Revision);

            XmlNode Package = doc.CreateElement("Package");
            XmlAttribute ID = doc.CreateAttribute("ID");
            ID.Value = "1ST";
            Package.Attributes.Append(ID);
            {
                XmlNode nService = doc.CreateElement("Service");
                nService.AppendChild(doc.CreateTextNode(Service));
                Package.AppendChild(nService);
                if (DropDownListService.SelectedValue.Equals("FIRST CLASS"))
                {
                    XmlNode nFirstClassMailType = doc.CreateElement("FirstClassMailType");
                    nFirstClassMailType.AppendChild(doc.CreateTextNode(packageType));
                    Package.AppendChild(nFirstClassMailType);
                }

                XmlNode nZipOrigination = doc.CreateElement("ZipOrigination");
                nZipOrigination.AppendChild(doc.CreateTextNode(ZipOrigination));
                Package.AppendChild(nZipOrigination);


                XmlNode nZipDestination = doc.CreateElement("ZipDestination");
                nZipDestination.AppendChild(doc.CreateTextNode(ZipDestination));
                Package.AppendChild(nZipDestination);

                XmlNode nPounds = doc.CreateElement("Pounds");
                nPounds.AppendChild(doc.CreateTextNode(Pounds));
                Package.AppendChild(nPounds);

                XmlNode nOunces = doc.CreateElement("Ounces");
                nOunces.AppendChild(doc.CreateTextNode(Ounces));
                Package.AppendChild(nOunces);

                

                if (DropDownListPackageType.SelectedValue.Equals("PARCEL"))
                {
                    XmlNode nContainer = doc.CreateElement("Container");
                    nContainer.AppendChild(doc.CreateTextNode("RECTANGULAR"));
                    Package.AppendChild(nContainer);

                    XmlNode nSize = doc.CreateElement("Size");
                    nSize.AppendChild(doc.CreateTextNode("LARGE"));
                    Package.AppendChild(nSize);

                    XmlNode nWidth = doc.CreateElement("Width");
                    nWidth.AppendChild(doc.CreateTextNode(Width));
                    Package.AppendChild(nWidth);
                    XmlNode nLength = doc.CreateElement("Length");
                    nLength.AppendChild(doc.CreateTextNode(Length));
                    Package.AppendChild(nLength);
                    XmlNode nHeight = doc.CreateElement("Height");
                    nHeight.AppendChild(doc.CreateTextNode(Height));
                    Package.AppendChild(nHeight);
                }
                else
                {
                    XmlNode nContainer = doc.CreateElement("Container");
                    Package.AppendChild(nContainer);

                    XmlNode nSize = doc.CreateElement("Size");
                    nSize.AppendChild(doc.CreateTextNode("REGULAR"));
                    Package.AppendChild(nSize);

                    XmlNode nMachinable = doc.CreateElement("Machinable");
                    nMachinable.AppendChild(doc.CreateTextNode("true"));
                    Package.AppendChild(nMachinable);
                }

               
            }
            RateV4Request.AppendChild(Package);
        }
        doc.AppendChild(RateV4Request);

        doc.Save(Console.Out);
        return doc;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        loadData();
        Response.Write(FormatXMLString(getXml()));

    }


    public string FormatXMLString(XmlDocument xd)
    {

        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        XmlTextWriter xtw = null;
        try
        {
            xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xd.WriteTo(xtw);
        }
        finally
        {
            if (xtw != null)
                xtw.Close();
        }
        return sb.ToString();
    }




    //=========================================================
    //process http request

    public static XmlDocument PostXMLTransaction(string v_strURL, XmlDocument v_objXMLDoc)
    {
        XmlDocument XMLResponse = new XmlDocument();
        HttpWebRequest objHttpWebRequest;
        HttpWebResponse objHttpWebResponse = null;
        Stream objRequestStream = null;
        Stream objResponseStream = null;
        XmlTextReader objXMLReader;
        objHttpWebRequest = (HttpWebRequest)WebRequest.Create(v_strURL);

        try
        {
            string postData = "API=RateV4&XML=";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(v_objXMLDoc.InnerXml);
            objHttpWebRequest.Method = "POST";
            objHttpWebRequest.ContentLength = bytes.Length + byteArray.Length;
            objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";

            objRequestStream = objHttpWebRequest.GetRequestStream();
            objRequestStream.Write(byteArray, 0, byteArray.Length);
            objRequestStream.Write(bytes, 0, bytes.Length);
            objRequestStream.Close();

            objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

            if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                objResponseStream = objHttpWebResponse.GetResponseStream();
                objXMLReader = new XmlTextReader(objResponseStream);
                XMLResponse.Load(objXMLReader);
                objXMLReader.Close();
            }
            objHttpWebResponse.Close();
        }
        catch (WebException we)
        {
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
        return XMLResponse;
    }


    protected void DropDownListService_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}