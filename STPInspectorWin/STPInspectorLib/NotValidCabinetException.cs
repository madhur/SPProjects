using System;

namespace STPInspectorLib
{
    public class NotValidCabinetException:SystemException
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
