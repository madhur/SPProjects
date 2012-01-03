using System;

namespace STPInspectorLib
{
    public class ManifestNotFoundException:SystemException
    {
        public override string Message
        {
            get
            {
                return Constants.invalidCabinet;
            }
            
        }

       
    }
}
