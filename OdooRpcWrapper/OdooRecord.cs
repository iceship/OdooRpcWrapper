using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpcWrapper
{
    public class OdooRecord
    {
        private readonly OdooAPI _api;
        private readonly string _model;
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        private readonly List<string> _modifiedFields = new List<string>();
        int _id = -1;        
        
        public OdooRecord(OdooAPI api, string model, int id)
        {
            _model = model;
            _api = api;
            _id = id;
        }

        public bool SetValue(string field, object value)
        {
            if(_fields.ContainsKey(field))
            {
                if(!_modifiedFields.Contains(field))
                {
                    _modifiedFields.Add(field);
                }

                _fields[field] = value;
            }
            else
            {
                _fields.Add(field, value);
            }
            return true;
        }

        public object GetValue(string field)
        {
            if (_fields.ContainsKey(field))
            {
                return _fields[field];
            }
            else
            {
                return null;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public void Save()
        {
            XmlRpcStruct values = new XmlRpcStruct();

            if (_id >= 0)
            {
                foreach (string field in _modifiedFields)
                {
                    values[field] = _fields[field];
                }

                _api.Write(_model, new int[1] { _id }, values);
            }
            else
            {
                foreach (string field in _fields.Keys)
                {
                    values[field] = _fields[field];
                }

                _id = _api.Create(_model, values);
            }
        }
    }
}
