using System;
using System.Collections.Generic;
using System.Text;

namespace CD_ALARM
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
