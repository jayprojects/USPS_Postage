using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    string Service;
    string ZipOrigination;
    string ZipDestination;
    string Pounds;
    string Ounces;
    string Container;
    string Size;
    string Width;
    string Length;
    string Height;
    string Girth;

    void loadData()
    {
        ZipOrigination = TextBoxZipForm.Text;
        ZipDestination = TextBoxZipTo.Text;
        Service = DropDownListService.SelectedValue;
        Pounds = TextBoxPounds.Text;
        Ounces = TextBoxOunces.Text;
        Size = DropDownListSize.SelectedValue;
        if (DropDownListShape.SelectedValue.Equals("PACKAGE"))
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
       // doc.Load("c:\\a.xml");
        XmlDocument rdoc = processHTTP.PostXMLTransaction("http://production.shippingapis.com/ShippingAPI.dll", doc);
        //XmlNode Rate = rdoc.DocumentElement.ChildNodes[0].ChildNodes[8].ChildNodes[1];

        XmlNodeList xnList = rdoc.SelectNodes("/RateV4Response/Package/Postage/Rate");
        String p = xnList.Item(0).InnerText;
        
        //Response.Write(FormatXMLString(rdoc) + "<br/> Shipping Cost: " + Rate.InnerText);
        Response.Write("<h2> Shipping Cost: " + p + "</h2>");
       


        //Response.Write("From: " + ZipOrigination + " To: " + ZipDestination + " Service: " + Service);
    }
    protected void DropDownListShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(DropDownListShape.SelectedValue.Equals("PACKAGE"))
            PanelDimension.Visible= true;
        else
            PanelDimension.Visible= false;
    }


    private XmlDocument getXml()
    {
        XmlDocument doc = new XmlDocument();
        //XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //doc.AppendChild(docNode);

        XmlNode RateV4Request = doc.CreateElement("RateV4Request");
        XmlAttribute USERID = doc.CreateAttribute("USERID");
        USERID.Value = "711JAY000663";
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

                XmlNode nFirstClassMailType = doc.CreateElement("FirstClassMailType");
                nFirstClassMailType.AppendChild(doc.CreateTextNode("LETTER"));
                Package.AppendChild(nFirstClassMailType);


                XmlNode nZipOrigination = doc.CreateElement("ZipOrigination");
                nZipOrigination.AppendChild(doc.CreateTextNode(ZipOrigination));
                Package.AppendChild(nZipOrigination);


                XmlNode nZipDestination = doc.CreateElement("ZipDestination");
                nZipDestination.AppendChild(doc.CreateTextNode(ZipDestination));
                Package.AppendChild(nZipDestination);
                
                XmlNode nPounds = doc.CreateElement("Pounds");
                nPounds.AppendChild(doc.CreateTextNode("0"));
                Package.AppendChild(nPounds);

                XmlNode nOunces = doc.CreateElement("Ounces");
                nOunces.AppendChild(doc.CreateTextNode(Ounces));
                Package.AppendChild(nOunces);



                if (DropDownListShape.SelectedValue.Equals("PACKAGE"))
                {
                    XmlNode nContainer = doc.CreateElement("Container");
                    nContainer.AppendChild(doc.CreateTextNode("RECTANGULAR"));
                    Package.AppendChild(nContainer);

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

                    
                }

                XmlNode nSize = doc.CreateElement("Size");
                nSize.AppendChild(doc.CreateTextNode(Size));
                Package.AppendChild(nSize);

                


                XmlNode nMachinable = doc.CreateElement("Machinable");
                nMachinable.AppendChild(doc.CreateTextNode("true"));
                Package.AppendChild(nMachinable);


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


    public  string FormatXMLString(XmlDocument xd)
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
}