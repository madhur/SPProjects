

namespace MySiteLib
{
    /// <summary>
    /// Exception class for User not found exceptional case
    /// </summary>
    public class UserNotFoundException:System.SystemException
    {
        public override string Message
        {
            get
            {
                return Constants.userNotFoundException;
            }
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
