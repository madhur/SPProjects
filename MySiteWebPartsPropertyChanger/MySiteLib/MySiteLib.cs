using System;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace MySiteLib
{
    /// <summary>
    /// Library class for common functions related to MySite
    /// </summary>
    public class MySiteLib
    {
        #region Private Variables

        private string _mySiteHostUrl;
        private ServerContext _context;
        private static MySiteLib _singlInstance;
        private static Object syncObj=new object();

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the server context
        /// </summary>
        /// <returns></returns>
        public  string Initialize()
        {
               
                UserProfileManager profileManager = new UserProfileManager(_context);
                _mySiteHostUrl=profileManager.MySiteHostUrl;
                return _mySiteHostUrl;

        }

        /// <summary>
        /// Returns the singleton instance of this class
        /// </summary>
        /// <returns></returns>
        public static MySiteLib GetInstance()
        {
            lock (syncObj)
            {
                if (_singlInstance == null)
                {
                    _singlInstance = new MySiteLib();
                    _singlInstance._context = ServerContext.Default;

                    if (_singlInstance._context == null)
                        return null;

                    return _singlInstance;
                }
                return _singlInstance;
            }
        }

        /// <summary>
        /// Propogates the changes to all user's MySite
        /// </summary>
        /// <param name="dtChanges"></param>
        /// <returns></returns>
        public string CommitChanges(DataTable dtChanges)
        {
            UserProfileManager profileManager = new UserProfileManager(_context);
            StringBuilder errors = new StringBuilder();
            foreach (UserProfile profile in profileManager)
            {

                if (profile.PersonalSite == null)
                {
                    continue;
                }

                SPSite mySite = profile.PersonalSite;
                using (SPWeb myWeb = mySite.OpenWeb())
                {
                    string defaultFileName;
                    if (ConfigurationSettings.AppSettings[Constants.defaultFileSettings] != null)
                    {
                        defaultFileName = ConfigurationSettings.AppSettings[Constants.defaultFileSettings];
                        if (string.IsNullOrEmpty(defaultFileName))
                            defaultFileName = Constants.defaultFile;
                    }
                    else
                    {
                        defaultFileName = Constants.defaultFile;
                    }
                    SPFile defaultFile;
                    try
                    {
                        defaultFile = myWeb.Files[defaultFileName];
                    }
                    catch (UnauthorizedAccessException)
                    {
                        errors.AppendFormat(Constants.unauthorizedExceptionError, profile.PersonalUrl);
                        continue;
                    }

                    SPLimitedWebPartManager lmWpm = defaultFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in lmWpm.WebParts)
                    {
                        DataRow[] propChanges = dtChanges.Select("WebPart='" + wp.GetType().AssemblyQualifiedName + "'");
                        foreach (DataRow changeRow in propChanges)
                        {
                            string propName = changeRow["Property"].ToString();
                            System.Reflection.PropertyInfo propInfo;
                            try
                            {
                                propInfo = wp.GetType().GetProperty(propName);
                            }
                            catch (AmbiguousMatchException)
                            {

                                //Issue for height and width properties
                                propInfo = wp.GetType().GetProperty(propName, typeof(string));

                                //errors.AppendFormat(Constants.AmbiguousMatchExceptionMessage, propName, wp.Title);
                               // continue;
                            }

                            try
                            {
                                if (propInfo.PropertyType != typeof(Int32) && propInfo.PropertyType != typeof(string) && propInfo.PropertyType != typeof(Boolean))
                                {
                                    Object enumObj = Enum.Parse(propInfo.PropertyType, changeRow["Value"].ToString());
                                    propInfo.SetValue(wp,enumObj,null);
                                }
                                else if(propInfo.PropertyType==typeof(Boolean))
                                {

                                    propInfo.SetValue(wp,Boolean.Parse(changeRow["Value"].ToString()),null);
                                }
                                else
                                {
                                    propInfo.SetValue(wp, changeRow["Value"], null);
                                }
                            }
                            catch (TargetInvocationException)
                            {
                                errors.AppendFormat(Constants.invocationExceptionError, propName, wp.Title);

                            }
                            catch (ArgumentException)
                            {
                                errors.AppendFormat(Constants.argumentExceptionError, propName, wp.Title);
                            }
                            lmWpm.SaveChanges(wp);
                            

                        }




                    }

                }

                mySite.Dispose();
            }

            return errors.ToString();
        }
    


        /// <summary>
        /// Returns the list of webparts in mysite.
        /// </summary>
        /// <returns></returns>
        public SPLimitedWebPartCollection GetWebPartsList()
        {
            string userAccount;
            UserProfileManager profileManager = new UserProfileManager(_context);
            if (ConfigurationSettings.AppSettings[Constants.userAccountSetting] != null)
            {
                userAccount = ConfigurationSettings.AppSettings[Constants.userAccountSetting];
                if (string.IsNullOrEmpty(userAccount))
                    userAccount = Environment.UserName;
            }
            else
            {
                userAccount = Environment.UserName;
            }
            UserProfile defProfile;
            try
            {
                defProfile = profileManager.GetUserProfile(userAccount);
            }
            catch (Microsoft.Office.Server.UserProfiles.UserNotFoundException)
            {
                throw new UserNotFoundException();
            }

            if (defProfile.PersonalSite == null)
            {
                throw new MySiteDoesNotExistException();
                
            }

            SPSite mySite = defProfile.PersonalSite;

            using (SPWeb myWeb = mySite.OpenWeb())
            {
                string defaultFileName;
                if (ConfigurationSettings.AppSettings[Constants.defaultFileSettings] != null)
                {
                    defaultFileName = ConfigurationSettings.AppSettings[Constants.defaultFileSettings];
                    if (string.IsNullOrEmpty(defaultFileName))
                        defaultFileName = Constants.defaultFile;
                }
                else
                {
                    defaultFileName = Constants.defaultFile;
                }

                SPFile defaultFile = myWeb.Files[defaultFileName];

                SPLimitedWebPartManager lmWpm= defaultFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

                return lmWpm.WebParts;
            }
        }
        #endregion


    }
}
