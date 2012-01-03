using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
namespace SiteQuotaWebPart
{
    /// <summary>
    /// Main class for the webpart
    /// </summary>
    public class SiteQuotaWebPart:WebPart
    {
        #region Properties
        /// <summary>
        /// Gets or sets the XSL text.
        /// </summary>
        /// <value>The XSL text.</value>
        [Personalizable(PersonalizationScope.Shared)]
        public string XslText
        {
            get;
            set;
        }

        #endregion

        #region Events

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            SPSite mySite = SPContext.Current.Site;
            long maxLevel = 0;
            long usage = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite elevatedSite = new SPSite(mySite.ID))
                {
                    maxLevel = elevatedSite.Quota.StorageMaximumLevel;
                    usage = elevatedSite.Usage.Storage;

                }
            });

            //If there is not quota, no point in continuing
            if (maxLevel == 0)
                return;


            StringWriter sWriter = new StringWriter();
            XmlTextWriter txWriter = new XmlTextWriter(sWriter);
            txWriter.WriteStartDocument();
            txWriter.WriteStartElement("SPWeb");
            txWriter.WriteElementString("StorageMaximumLeveL", maxLevel.ToString());
            txWriter.WriteElementString("Storage", usage.ToString());
            txWriter.WriteEndDocument();
            txWriter.Flush();
            txWriter.Close();

            XmlDocument xmlInput = new XmlDocument();
            xmlInput.LoadXml(sWriter.ToString());


            XPathDocument xPathDoc;

            if (string.IsNullOrEmpty(XslText))
            {
                Stream xsltStream = GetType().Assembly.GetManifestResourceStream("SiteQuotaWebPart.Default.xslt");
                byte[] xsltBytes = new byte[xsltStream.Length];
                xsltStream.Read(xsltBytes, 0, Convert.ToInt32(xsltStream.Length));
                //StringWriter sWriter = new StringWriter();
                //xsltStream.r


                XslText = Encoding.UTF8.GetString(xsltBytes);

                xsltStream.Seek(0, SeekOrigin.Begin);
                try
                {
                    xPathDoc = new XPathDocument(xsltStream);
                }
                catch (XmlException ex)
                {
                    Controls.Add(new LiteralControl(ex.Message));
                    return;
                }
            }
            else
            {
                //Skip the BOM from the string
                StringReader sReader = new StringReader(XslText.Substring(1));
                //  XmlTextReader xmlReader = new XmlTextReader(sReader);

                XmlReader xmlReader = XmlReader.Create(sReader);
                try
                {
                    xPathDoc = new XPathDocument(xmlReader, XmlSpace.Default);
                }
                catch (XmlException ex)
                {
                    Controls.Add(new LiteralControl(ex.Message));
                    return;
                }
            }


            XslCompiledTransform xmlTransform = new XslCompiledTransform();
            xmlTransform.Load(xPathDoc);

            StringWriter outputWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(outputWriter);

            xmlTransform.Transform(xmlInput, xmlWriter);


            Controls.Add(new LiteralControl(outputWriter.ToString()));
            SetPersonalizationDirty();


            base.CreateChildControls();
        } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a collection of custom <see cref="T:System.Web.UI.WebControls.WebParts.EditorPart"/> controls that can be used to edit a <see cref="T:System.Web.UI.WebControls.WebParts.WebPart"/> control when it is in edit mode.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Web.UI.WebControls.WebParts.EditorPartCollection"/> that contains custom <see cref="T:System.Web.UI.WebControls.WebParts.EditorPart"/> controls associated with a <see cref="T:System.Web.UI.WebControls.WebParts.WebPart"/> control.
        /// </returns>
        public override EditorPartCollection CreateEditorParts()
        {
            List<EditorPart> editors = new List<EditorPart>();
            XslEditor xsl = new XslEditor { Title = "Edit Xsl", ID = ("xslid" + ID) };
            editors.Add(xsl);

            return new EditorPartCollection(editors);
        }

        #endregion
        
    }
}
