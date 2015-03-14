using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpcWrapper
{
    public class OdooModel
    {
        private readonly string _modelName;
        private readonly OdooAPI _api;
        private readonly List<string> _fields = new List<string>();

        public OdooModel(string modelName, OdooAPI api)
        {
            _api = api;
            _modelName = modelName;
        }

        public List<OdooRecord> Search(object[] filter)
        {
            List<OdooRecord> records = new List<OdooRecord>();

            int[] ids = _api.Search(_modelName, filter);
            object[] result = _api.Read(_modelName, ids, _fields.ToArray());

            foreach(object entry in result)
            {
                XmlRpcStruct vals = (XmlRpcStruct)entry;

                // Get ID
                int id = (int) vals["id"];
                OdooRecord record = new OdooRecord(_api, _modelName, id);

                // Get other values
                foreach(string field in _fields)
                {
                    record.SetValue(field, vals[field]);
                }
                
                records.Add(record);
            }

            return records;            
        }
                
        public void AddField(string field)
        {
            // Todo: check through API if model contains field
            if(!_fields.Contains(field))
            {
                _fields.Add(field);
            }
        }

        public void AddFields(List<string> fields)
        {
            foreach(string field in fields)
            {
                AddField(field);
            }
        }

        public void Remove(List<OdooRecord> records)
        {
            int[] toRemove = records.Where(r => r.Id >= 0)
                                    .Select(r => r.Id)
                                    .ToList<Int32>()
                                    .ToArray();

            _api.Remove(_modelName, toRemove);                                  
        }

        public void Remove(OdooRecord record)
        {
            Remove(new List<OdooRecord>() { record });
        }

        public void Save(List<OdooRecord> records)
        {
            foreach(OdooRecord record in records)
            {
                record.Save();
            }
        }

        public void Save(OdooRecord record)
        {
            Save(new List<OdooRecord>(){ record });
        }

        public OdooRecord CreateNew()
        {
            return new OdooRecord(_api, _modelName, -1);
        }
    }
}
