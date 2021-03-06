﻿using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Win32;
using QvcTesterTool.Commands;
using QvcTesterTool.Data;
using QvcTesterTool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using QvcTesterTool.ViewModel;
using QvcTesterTool.Helpers;

namespace QvcTesterTool
{
    public class Core
    {
        #region Private Fields

        private Device _selectedDevice;
        private WebBuild _selectedWebBuild;
        private ObservableCollection<Device> _devices;
        private ObservableCollection<WebBuild> _webBuilds;
        private Dispatcher _dispatcher;
        private StatusBarViewModel _statusBar;
        private LogButton _logButton;

        #endregion

        #region Public Properties and Indexers

        public Device SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                if (this._selectedDevice == value || value == null) return;
                _selectedDevice = value;
                _selectedDevice.UpdatePackagesList();
            }
        }

        public ObservableCollection<Device> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<Device>();
                }
                return _devices;
            }
            private set
            {
                _devices = value;
                UpdateDevicesList();
            }
        }

        public ObservableCollection<WebBuild> WebBuilds
        {
            get
            {
                
                //if (_webBuilds == null)
                //{
                //    _webBuilds = new ObservableCollection<WebBuild>();
                //}
                return _webBuilds;
            }

            private set
            {
                _webBuilds = value;
                _firstResultDataView = new CollectionViewSource();
                _firstResultDataView.Source = _webBuilds;
            }
        }

        public WebBuild SelectedWebBuild
        {
            get
            {
                return _selectedWebBuild;
            }
            set
            {
                if (this._selectedWebBuild == value || value == null) return;
                _selectedWebBuild = value;
            }
        }

        public StatusBarViewModel StatusBar
        {
            get
            {
                return _statusBar;
            }
            set
            {
                _statusBar = value;
            }
        }

        public LogButton LogButton
        {
            get
            {
                return _logButton;
            }
            set
            {
                _logButton = value;
            }
        }

        #endregion

        #region Constructor

        public Core()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void UpdateDevicesList()
        {
            var devices = AdbShell.GetDevices().Select((id) => new Device(id)).ToList();
            if (devices.Count > 0)
            {
                var myIdSets = new List<string>(devices.Select(c => c.Id));
                Devices.Clear();
                myIdSets.ForEach((x) => Devices.Add(new Device(x)));
                SelectedDevice = new Device(String.Empty);
            }
            else
            {
                Devices.Clear();
                SelectedDevice = new Device(String.Empty);
            }
            //Devices.Clear();
            //var devices = AdbShell.GetDevices().Select((id) => new Device(id)).ToList();
            //if (devices.Count > 0)
            //{
            //    SelectedDevice = new Device(String.Empty);
            //}
            //else
            //{
            //    SelectedDevice = new Device(String.Empty);
            //}
            //devices.ForEach((x) => Devices.Add(x));
        }

        public void RemoveUsbEventHandler()
        {
            var devices = AdbShell.GetDevices().Select((id) => new Device(id)).ToList();
            if (devices.Count > 0)
            {
                var myIdSets = new List<string>(devices.Select(c => c.Id));
                var result = Devices.Where(r => myIdSets.Contains(r.Id)).ToList();

                Devices.Clear();
                result.ForEach((x) => Devices.Add(x));

                if (SelectedDevice != null && !result.Any(c => c.Id == SelectedDevice.Id) && Devices.Count > 0)
                {
                    SelectedDevice = Devices[0];
                }
            }
            else
            {
                Devices.Clear();
                SelectedDevice = new Device(String.Empty);
            }
        }

        public void InsertUsbEventHandler()
        {

        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            Devices = new ObservableCollection<Device>();
            WebBuilds = new ObservableCollection<WebBuild>();
            _statusBar = new StatusBarViewModel();
            _logButton = new LogButton(){Text = "Start log"};

            UpdateWebBuilds();

            _dispatcher = Dispatcher.CurrentDispatcher;
            StartUsbEvent();
            //if (Devices.Count > 0 && !String.IsNullOrEmpty(Devices[0].Id))
            //{
            //    SelectedDevice = Devices[0];
            //    SelectedDevice.UpdatePackagesList();
            //}
            //else
            //{
            //    SelectedDevice = new Device("");
            //}

        }

        private void StartUsbEvent()
        {
            using (var watcher = new ManagementEventWatcher())
            {
                var query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_USBHub'");
                watcher.EventArrived += new EventArrivedEventHandler((x, y) => _dispatcher.Invoke(UpdateDevicesList, DispatcherPriority.Send));
                watcher.Query = query;
                watcher.Start();
            }

            using (var watcher = new ManagementEventWatcher())
            {
                var query = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_USBHub'");
                watcher.EventArrived += new EventArrivedEventHandler((x, y) => _dispatcher.Invoke(RemoveUsbEventHandler, DispatcherPriority.Send));
                watcher.Query = query;
                watcher.Start();
            }
        }

        #endregion

        #region Update WebBuilds Command

        private ICommand _updateWebBuildsCommand;

        public ICommand UpdateWebBuildsCommand
        {
            get
            {
                if (_updateWebBuildsCommand == null)
                {
                    _updateWebBuildsCommand = new RelayCommand(
                        param => this.UpdateWebBuilds(),
                        param => this.CanUpdateWebBuilds()
                    );
                }
                return _updateWebBuildsCommand;
            }
        }

        private bool CanUpdateWebBuilds()
        {
            return true;
        }

        private void UpdateWebBuilds()
        {
            WebBuilds.Clear();
            string[] buildTypes = { "qa", "stage" };
            string[] buildKinds = { "tabletopt", "fragment_ci" };
            string[] buildCultures = { "us", "uk", "de" };

            Array.ForEach(buildCultures, c => Array.ForEach(buildTypes, t =>
                                           Array.ForEach(buildKinds, k =>
                                           WebBuilds.Add(new WebBuild(c, t, k)))));
        }

        #endregion //Update Command

        #region Update Command

        private ICommand _updateCommand;

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand(
                        param => this.UpdateObject(),
                        param => this.CanUpdate()
                    );
                }
                return _updateCommand;
            }
        }

        private bool CanUpdate()
        {
            return true;
        }

        private void UpdateObject()
        {
            UpdateDevicesList();
        }

        #endregion //Update Command

        #region Reset Command

        private ICommand _resetCommand;

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(
                        param => this.ResetObject(),
                        param => this.CanReset()
                    );
                }
                return _resetCommand;
            }
        }

        private bool CanReset()
        {
            return (_selectedDevice != null && !String.IsNullOrEmpty(_selectedDevice.Id));
        }

        private void ResetObject()
        {
            AdbShell.ResetDevice(_selectedDevice.Id);
        }

        #endregion //Reset Command

        #region Log Command

        private ICommand _logCommand;

        public ICommand LogCommand
        {
            get
            {
                if (_logCommand == null)
                {
                    _logCommand = new RelayCommand(
                        param => this.StartLog()
                    );
                }

                if (_logButton.Text == "Start log")
                {
                    _logCommand = new RelayCommand(
                        param => this.StartLog()
                        );
                }

                if (_logButton.Text == "Stop log")
                {
                    _logCommand = new RelayCommand(
                        param => this.StopLog()
                    );
                }
                return _logCommand;
            }
        }

 

        private void StartLog()
        {
            AdbShell.StartLog(_selectedDevice.Id);
        }

        private void StopLog()
        {


            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = DateTime.Now.ToString(CultureInfo.InvariantCulture) + ".txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                //StatusBar.DeviceStatusBar = "Downloading file...";
                AdbShell.StopAndGetLog(_selectedDevice.Id, path);
            }  

        }

        #endregion //Reset Command

        #region Screenshot Command

        private ICommand _screenshotCommand;

        public ICommand ScreenshotCommand
        {
            get
            {
                if (_screenshotCommand == null)
                {
                    _screenshotCommand = new RelayCommand(
                        param => this.TakeScreenshot(),
                        param => this.CanScreenshot()
                    );
                }
                return _screenshotCommand;
            }
        }

        private bool CanScreenshot()
        {
            return (_selectedDevice != null && !String.IsNullOrEmpty(_selectedDevice.Id));
        }

        private void TakeScreenshot()
        {
            var screenshotName = AdbShell.MakeScreenshot(_selectedDevice.Id);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = screenshotName + ".png";
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                //StatusBar.DeviceStatusBar = "Downloading file...";
                AdbShell.TakeScreenshot(_selectedDevice.Id, screenshotName, path);
            }  


        }

        #endregion //Reset Command

        #region Download Command

        private ICommand _downloadCommand;

        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(
                        DownloadObject,
                        param => this.CanDownload()
                    );
                }
                return _downloadCommand;
            }
        }

        private bool CanDownload()
        {
            return (_selectedWebBuild != null && (SelectedWebBuild.BuildNumber != "-"));
        }

        private void DownloadObject(object build)
        {
            string downloadLink;
            string buildParameters = build as string;
            string[] parameters = buildParameters.Split('_');
            string webAdress;

            if (parameters.Length == 4)
            {
                webAdress = DataStrings.webAdress.Replace("*culture*", parameters[0]).Replace("*type*", parameters[1]).Replace("*kind*", parameters[2] + "_" + parameters[3]);
            }
            else if (parameters.Length == 3)
            {
                webAdress = DataStrings.webAdress.Replace("*culture*", parameters[0]).Replace("*type*", parameters[1]).Replace("*kind*", parameters[2]);
            }

            else
            {
                return;
            }

            string webSource;
            string pattrn = "<div class=\"link\"><a href=\"(.*).apk";
            Regex regexWeb = new Regex(pattrn);

            
            using (var client = new WebClient())
            {
                webSource = client.DownloadString(new Uri(webAdress));
            }
            

            downloadLink = regexWeb.Match(webSource).Groups[1].Value + ".apk";

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string pattern = "(/.*/.*/.*/.*/.*/.*/)(.*)";
            Regex regex = new Regex(pattern);
            string filename = regex.Match(downloadLink).Groups[2].Value;

            saveFileDialog.FileName = filename;
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                StatusBar.DownloadStatusBar = "Downloading file...";
                DownloadFile(downloadLink, path);
            }         
        }

        public void DownloadFile(string link, string path)
        {
            try
            {
                Task.Run(() =>
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(link), path);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                    }
                });

            }
            catch
            {
                return;
            }
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _dispatcher.Invoke(()=>
            {
                StatusBar.DownloadStatusBar = "Downloading Complete.";
                StatusBarChangedAsync();
            });
        }

        private async void StatusBarChangedAsync()
        {
            await Task.Delay(3000);
            StatusBar.DownloadStatusBar = "";
        }

        #endregion //Download Command

        #region Sorting

        private CollectionViewSource _firstResultDataView;
        private string _sortColumn;
        private ListSortDirection _sortDirection;
        private ICommand _sortCommand;

        public ICommand SortCommand
        {
            get
            {
                if (_sortCommand == null)
                {
                    _sortCommand = new RelayCommand(Sort);
                }
                return _sortCommand;
            }
        }

        public ListCollectionView FirstResultDataView
        {
            get
            {
                return (ListCollectionView)_firstResultDataView.View;
            }
        }

        public void Sort(object parameter)
        {
            string column = parameter as string;
            if (_sortColumn == column)
            {
                // Toggle sorting direction 
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
            }

            _firstResultDataView.SortDescriptions.Clear();
            _firstResultDataView.SortDescriptions.Add(
                                     new SortDescription(_sortColumn, _sortDirection));
        }

        #endregion //Sorting

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

        #endregion // INotifyPropertyChanged Implementation

        #region Install apk

        public async void InstallApk(string path)
        {
            await AdbShell.InstallApkAsync(path, _selectedDevice.Id);
            _selectedDevice.UpdatePackagesList();
        }

        #endregion
    }
}