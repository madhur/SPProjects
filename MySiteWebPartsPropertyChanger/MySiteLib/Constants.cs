using System;
using System.Collections.Generic;
using System.Text;

namespace MySiteLib
{
    public  class Constants
    {
        public  const string defaultFile = "Default.aspx";
        public  const string defaultFileSettings = "DefaultFile";
        public  const string unauthorizedExceptionError = "Unauthorized for {0}\n";
        public  const string invocationExceptionError = "Invocation Exception for property {0} on {1}\n";
        public const string argumentExceptionError = "Argument Exception for property {0} on {1}\n";
        public  const string userAccountSetting = "UserAccount";
        public  const string serverContextError = "Could not get Server Context.Please run this tool as Administrator";
        public  const string mySiteException="The MySite Does not exist for the current user. Please specify the Account to be used in the configuration file";
        public  const string userNotFoundException="The user does not exist in UserProfile database. Please check the account used in configuration file";
        public  const string commitConfirmMessage = "Are You sure you want to propogate changes to all users?";
        public const string AmbiguousMatchExceptionMessage = "Ambiguous Exception for property {0} on {1}\n";
    }
}
