using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QvcTesterTool.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dispatcher _dispatcher;
        public ObservableCollection<string> Files
        {
            get
            {
                return _files;
            }
        }

        private ObservableCollection<string> _files = new ObservableCollection<string>();
    
        public MainWindow()
        {
          //  InitializeComponent();
            DataContext = new Core();
            _dispatcher = Dispatcher.CurrentDispatcher;
           
        }

        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new ImageBrush(new BitmapImage(new Uri(@"..\..\Resources\Pngs\androidapk.png", UriKind.Relative)));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new ImageBrush(new BitmapImage(new Uri(@"..\..\Resources\Pngs\androidapk.png", UriKind.Relative)));
            
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _files.Clear();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string filePath in files)
                {
                    _files.Add(filePath);
                }
                UploadFiles(files);

                
            }

            var listbox = sender as ListBox;
            listbox.Background = new ImageBrush(new BitmapImage(new Uri(@"..\..\Resources\Pngs\androidapk.png", UriKind.RelativeOrAbsolute)));
        }

        private void UploadFiles(string[] files)
        {
            ((Core)DataContext).InstallApk(files[0]);   
        }

        public void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Apk Files (*.apk)|*.apk";
            dialog.FilterIndex = 1;
            dialog.Multiselect = false;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string[] sFileNames = dialog.FileNames;
                UploadFiles(sFileNames);
            }
        }
    }
}
