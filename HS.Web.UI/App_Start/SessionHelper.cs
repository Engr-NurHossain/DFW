using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HS.Entities;
using HS.Framework;

namespace HS.Web.UI.App_Start
{
    public class SessionHelper
    {
        private const string KEY_PREFIX = "HS.Web.";
        private const string KEY_CLIENT = KEY_PREFIX + "Client";
        private const string KEY_PERMISSION = KEY_PREFIX + "Permission";
        private const string KEY_USERID_LIST = KEY_PREFIX + "UserIDList";
        public const string KEY_SELECTED_COMPANY = "SelectedCompany";
        public const string KEY_USER_PERMISSION = "UserPermissions";
        public const string KEY_CURRENT_LOGGEDIN_USER = "CurrentLoggedInUser";
        public const string KEY_CURRENT_USER_COMPANY_LIST = "CurrentUserCompanyList";
        //private const string KEY_ADMIN_CLIENT = KEY_PREFIX + "AdminClient";

        private HttpContext _Context = HttpContext.Current;

        public void Abandon()
        {
            _Context.Session.RemoveAll();
            _Context.Session.Abandon();
        }
        public void ClearCurrentSession()
        {
            _Context.Session.Clear();
        }

        public SessionHelper()
        {
        }

        public void Set(string key, object value)
        {
            _Context.Session[KEY_PREFIX + key] = value;
        }

        public object Get(string key)
        {
            return _Context.Session[KEY_PREFIX + key];
        }

        public void SetClient(UserLogin user)
        {
            _Context.Session[KEY_CLIENT] = user;
        }

        public Client GetClient()
        {

            return (Client)_Context.Session[KEY_CLIENT];
        }

        public void SetClient(Client client)
        {
            _Context.Session[KEY_CLIENT] = client;
        }

        public void Remove(string key)
        {
            _Context.Session.Remove(key);
        }

        public void RemoveClient()
        {
            Remove(KEY_CLIENT);
        }

        /// <summary>
        ///	Preserve a list of user ID in session for using between
        ///	multiple pages
        /// </summary>
        /// <param name="list">An array list containing ID of users</param>
        public void SetUserIDs(ArrayList list)
        {
            _Context.Session.Add(KEY_USERID_LIST, list);
        }

        /// <summary>
        /// Returns the list of user ID stored in session
        /// </summary>
        /// <returns>ArrayList containing user ID</returns>
        public ArrayList GetUserIDList()
        {
            return (ArrayList)_Context.Session.Contents[KEY_USERID_LIST];
        }

        /// <summary>
        /// Removes user ID list from session
        /// </summary>
        public void ClearUserIDList()
        {
            _Context.Session.Remove(KEY_USERID_LIST);
        }

        public Hashtable GetPermissions()
        {
            return (Hashtable)_Context.Session[KEY_PERMISSION];
        }

        public void Dispose()
        {
            _Context = null;
        }
    }
}