using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
namespace STPInspectorLib
{
    /// <summary>
    /// Processes and matches the features installed on the machine
    /// </summary>
    public class FeatureLib
    {
        /// <summary>
        /// Resolves the features installed on local machine
        /// </summary>
        /// <param name="featureItems">List of features</param>
        /// <returns>TRUE if one of the feature was unresolved</returns>
        public static Constants.SPStatus ResolveFeatures(List<FeatureItem> featureItems)
        {
            SPFarm farmObj=SPFarm.Local;
            if (farmObj == null)
            {
                return Constants.SPStatus.Unavailable;
            }
            bool unresolvedStatus = false;
            foreach(FeatureItem featureItem in featureItems)
            {
                if (farmObj.FeatureDefinitions[new Guid(featureItem.ID)] != null)
                {
                    featureItem.Name = farmObj.FeatureDefinitions[new Guid(featureItem.ID)].DisplayName;
                }
                else
                {
                    unresolvedStatus = true;
                }
            }

            if (unresolvedStatus)
                return Constants.SPStatus.NotResolved;
            else
                return Constants.SPStatus.Resolved;
          
        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        private FeatureLib()
        {
            
        }
    }
}
