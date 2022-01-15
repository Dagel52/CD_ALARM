using System;
using System.Collections.Generic;
using System.Text;

namespace CD_ALARM
{
    public class AutomationObject
    {
        private string _name;
        private string _serverE3Path;
        private string _localE3Path;
        private string _description = "Введите описание объекта";

        public AutomationObject(string name, string serverE3Path, string localE3Path)
        {
            _name = name;
            _serverE3Path = serverE3Path;
            _localE3Path = localE3Path;
        }

        public AutomationObject()
        {

        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ServerE3
        {
            get { return _serverE3Path; }
            set { _serverE3Path = value; }
        }

        public string LocalE3
        {
            get { return _localE3Path; }
            set { _localE3Path = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
