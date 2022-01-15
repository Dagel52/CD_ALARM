using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace CD_ALARM
{
    public class JsonFileService : IFileService
    {
        public List<AutomationObject> Open(string filename)
        {
            List<AutomationObject> objects = new List<AutomationObject>();
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<AutomationObject>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                objects = jsonFormatter.ReadObject(fs) as List<AutomationObject>;
            }

            return objects;
        }

        public void Save(string filename, List<AutomationObject> objectsList)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<AutomationObject>));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, objectsList);
            }
        }
    }
}
