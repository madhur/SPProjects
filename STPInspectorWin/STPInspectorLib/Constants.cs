namespace STPInspectorLib
{
    /// <summary>
    /// Contains definition to standard strings
    /// </summary>
    public static class Constants
    {
        public const string manifestFileName="Manifest.xml";
        public const string fileExt = "stp";
        public const string invalidCabinet = "The selected file is not valid site template file";
        public const string webFeaturesXPath = "/Web/WebFeatures/Feature";
        public const string siteFeaturesXPath = "/Web/SiteFeatures/Feature";
        public const string unResolvedStatus = "Failure: The site template is dependent upon features which are not installed on the system.";
        public const string resolvedStatus = "Success: Site template satisfies all feature dependencies";
        public const string accessDenied = "You do not sufficient permissions to access the file";
        public const string failSP = "Unable to obtain access to Sharepoint context; Will not be able to resolve feature Ids";

        public enum SPStatus
        {
            Resolved,
            NotResolved,
            Unavailable
        };
    }
}
