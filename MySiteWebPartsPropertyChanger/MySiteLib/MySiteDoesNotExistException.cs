using System;

namespace MySiteLib
{
    /// <summary>
    /// Exception class for the My Site Does not Exist
    /// </summary>
    public class MySiteDoesNotExistException:SystemException
    {
        public override string Message
        {
            get
            {
                return Constants.mySiteException;
            }
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
