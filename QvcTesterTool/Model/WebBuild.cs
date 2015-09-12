using JustForTestConsole.Data;
using QvcTesterTool.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace QvcTesterTool.Model
{
    public class WebBuild: INotifyPropertyChanged
    {
        #region Private Fields

        private string _build;
        private string _buildDate;
        private string _buildNumber;
        private bool _isAvailable;
        private string _webAdress;
        private string _sourceCode;

        private string _culture;
        private string _buildType;
        private string _buildKind;

        #endregion //Private Fields

        #region Public Properties

        public string BuildDate
        {
            get
            {
                return _buildDate;
            }
            set
            {
                _buildDate = value;
                OnPropertyChanged("BuildDate");
            }
        }

        public string BuildNumber
        {
            get
            {
                return _buildNumber;
            }
            set
            {
                _buildNumber = value;
                OnPropertyChanged("BuildNumber");
            }
        }

        public string Build
        {
            get
            {
                return _build;
            }
            set
            {
                _build = value;
                OnPropertyChanged("Build");
            }
        }

        public bool IsAvailable
        {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
                OnPropertyChanged("IsAvailable");
            }
        } 

        #endregion //Public Properties

        #region Constructor

        public WebBuild(string culture, string buildType, string buildKind)
        {
            Initialize(culture, buildType, buildKind);
        }

        #endregion //Constructor

        #region Private Methods

        private void Initialize(string culture, string buildType, string buildKind)
        {
            _buildType = buildType;
            _buildKind = buildKind;
            _culture = culture;

            _webAdress = DataStrings.webAdress.Replace("*culture*", culture).Replace("*type*", buildType).Replace("*kind*", buildKind);
            Task.Run(new Action(CheckWebBuild));
        }

        private string GetSourceCode(string webAdress)
        {
            using (var client = new WebClient())
            {
                try 
                {
                    return client.DownloadString(webAdress); 
                }
                catch
                {
                    IsAvailable = false;
                    BuildDate = "-";
                    BuildNumber = "-";
                    return "-";
                }
            }
        } 

        #endregion //Private Methods

        #region Public Methods

        public string GetBuildNumber()
        {
            string pattern = "(<br />Build )[^\\d]*(\\d+)\\s";
            Regex regex = new Regex(pattern);
            return regex.Match(_sourceCode).Groups[2].Value;
        }

        public string GetBuildDate()
        {
            string pattern = "(<p>Build Date: )(.*)</p>";
            Regex regex = new Regex(pattern);
            return regex.Match(_sourceCode).Groups[2].Value;
        }

        public string GetBuild()
        {
            return String.Format("{0}_{1}_{2}", _culture, _buildType, _buildKind);
        }

        public void CheckWebBuild()
        {
            IsAvailable = true;
            _sourceCode = GetSourceCode(_webAdress);
            Build = GetBuild();
            if (IsAvailable)
            {
                BuildNumber = GetBuildNumber();
                BuildDate = GetBuildDate();
            }
        } 

        #endregion //Public Methods

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
