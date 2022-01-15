using System;
using System.Collections.Generic;
using System.Text;

namespace CD_ALARM
{
    interface IFileService
    {
        List<AutomationObject> Open(string filename);
        void Save(string filename, List<AutomationObject> objectsList); 
    }
}
