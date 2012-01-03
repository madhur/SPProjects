using System;

namespace STPInspectorLib
{
    /// <summary>
    /// Encapsulates the SPFeature object
    /// </summary>
    public class FeatureItem
    {
        private Guid _featureID;
        private string _name;
        
        /// <summary>
        /// ID of the feature
        /// </summary>
        public string ID
        {
            get
            {
                return _featureID.ToString();
            }
            set
            {
                _featureID = new Guid(value);
            }
        }

        /// <summary>
        /// Name of the feature
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }


        }

        /// <summary>
        /// Constructs the Feature object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public FeatureItem(string name, string id)
        {
            Name = name;
            ID = id;
        }
    }
}
