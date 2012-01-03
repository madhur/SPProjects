using System.Collections.Generic;
using System.Xml.XPath;
using System.IO;

namespace STPInspectorLib
{
    /// <summary>
    /// Processes the manifest.xml file and extracts out feature ID's
    /// </summary>
    public class ManifestLib
    {
        /// <summary>
        /// Gets the list of features from manifest.xml
        /// </summary>
        /// <param name="manifestPath">Path to manifest.xml</param>
        /// <param name="web">TRUE for site feature, FALSE for site collection features</param>
        /// <returns>List of features</returns>
        public static List<FeatureItem> GetWebFeatures(string manifestPath,bool web)
        {
            using (FileStream fs=new FileStream(manifestPath,FileMode.Open,FileAccess.Read))
            {
                XPathDocument xpathDoc=new XPathDocument(fs);
                XPathNodeIterator featureItems;
                XPathNavigator xNav=xpathDoc.CreateNavigator();

                if(web)
                {
                    featureItems= xNav.Select(Constants.webFeaturesXPath);
                }
                else
                {
                    featureItems= xNav.Select(Constants.siteFeaturesXPath);
                }

                List<FeatureItem> featureList = new List<FeatureItem>();
                while (featureItems.MoveNext())
                {
                    featureList.Add(new FeatureItem(string.Empty,featureItems.Current.GetAttribute("ID",string.Empty)));

                }

                return featureList;
            }


        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        private ManifestLib()
        {
            
        }

    }
}
