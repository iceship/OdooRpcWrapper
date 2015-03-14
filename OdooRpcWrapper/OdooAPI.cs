using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpcWrapper
{
    public class OdooAPI
    {
        private readonly OdooConnectionCredentials _credentials;
        private readonly WebProxy _networkProxy;
        private readonly bool _serverCertificateValidation;
        private IOdooObjectRpc _objectRpc;
        
        public OdooAPI(OdooConnectionCredentials credentials, bool immediateLogin = true, WebProxy networkProxy = null, bool serverCertificateValidation = true)
        {
            _serverCertificateValidation = serverCertificateValidation;
            _networkProxy = networkProxy;
            _credentials = credentials;

            if(immediateLogin)
            {
                Login();
            }
        }

        public bool Login()
        {
            IOdooCommonRpc loginRpc = XmlRpcProxyGen.Create<IOdooCommonRpc>();
            loginRpc.Url = _credentials.CommonUrl;
            
            if (_networkProxy != null)
            {
                loginRpc.Proxy = _networkProxy;
            }

            if (_serverCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }

            // Log in and get user id
            _credentials.UserId = loginRpc.login(_credentials.DbName, _credentials.DbUser, _credentials.DbPassword);
                
            // Create proxy for Object operations
            _objectRpc = XmlRpcProxyGen.Create<IOdooObjectRpc>();
            _objectRpc.Url = _credentials.ObjectUrl;

            return true;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // Implement Server Certificate validation logic here, if needed.
            return true;
        }

        public int Create(string model, XmlRpcStruct fieldValues)
        {
            return _objectRpc.create(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, "create", fieldValues);
        }

        public int[] Search(string model, object[] filter)
        {
            return _objectRpc.search(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, "search", filter);
        }

        public object[] Read(string model, int[] ids, object[] fields)
        {
            return _objectRpc.read(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, "read", ids, fields);
        }

        public bool Write(string model, int[] ids, XmlRpcStruct fieldValues)
        {
            return _objectRpc.write(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, "write", ids, fieldValues);
        }

        public bool Remove(string model, int[] ids)
        {
            return _objectRpc.unlink(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, "unlink", ids);
        }

        public bool Execute_Workflow(string model, string action, int id)
        {
            return _objectRpc.exec_workflow(_credentials.DbName, _credentials.UserId, _credentials.DbPassword, model, action, id);
        }

        public OdooModel GetModel(string model)
        {
            // TODO : check if model exists
            return new OdooModel(model, this);
        }
    }
}
