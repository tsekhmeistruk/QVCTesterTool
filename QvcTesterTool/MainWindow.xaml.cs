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

namespace QvcTesterTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Collection { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Collection = new ObservableCollection<string>()
            {
                "device1",
                "device2",
                "device3"
            };
            DataContext = this.Collection;

            
        }

        public void Add()
        {
            Collection.Add("newDevice");
        }

        public void Reset()
        {
            Collection = new ObservableCollection<string>()
            {
                "device1"
            };
        }
    }
}
