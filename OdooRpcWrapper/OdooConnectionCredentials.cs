using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpcWrapper
{
    public class OdooConnectionCredentials
    {
        private string _suffix_host_url = "xmlrpc";
        private string _common_url = "common";
        private string _object_url = "object";        
        private int _UserId = -1;

        public OdooConnectionCredentials(string serverUrl, string dbName, string dbUser, string dbPassword)
        {
            this.ServerUrl = serverUrl;
            this.DbName = dbName;
            this.DbUser = dbUser;
            this.DbPassword = dbPassword;
        }

        public string ServerUrl { get; private set; }
        public string DbName { get; private set; }
        public string DbUser { get; private set; }
        public string DbPassword { get; private set; }
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }

        public string CommonUrl
        {
            get
            {
                return String.Format("{0}/{1}/{2}", ServerUrl, _suffix_host_url, _common_url);
            }
        }

        public string ObjectUrl
        {
            get
            {
                return String.Format("{0}/{1}/{2}", ServerUrl, _suffix_host_url, _object_url);
            }
        }

    }
}
