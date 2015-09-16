using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QvcTesterTool.ViewModel
{
    public class StatusBarViewModel : INotifyPropertyChanged
    {
        private string _downloadStatusBar;

        public string DownloadStatusBar
        {
            get { return _downloadStatusBar; }
            set
            {
                _downloadStatusBar = value; 
                OnPropertyChanged("DownloadStatusBar");
            }
        }

        #region INotifyPropertyChanged Implementation


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion //INotifyPropertyChanged Implementation    
    }
}
