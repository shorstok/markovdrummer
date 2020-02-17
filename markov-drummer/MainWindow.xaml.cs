using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
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
using markov_drummer.Vm;
using MahApps.Metro.Controls;

namespace markov_drummer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MainVm _viewmodel;

        public MainWindow()
        {            
            _viewmodel = new MainVm();

            DataContext = _viewmodel;

            InitializeComponent();
            AddVersionToTitle();
        }

        private void AddVersionToTitle()
        {
            var ver = Assembly.GetEntryAssembly()?.GetName().Version;
            
            Title = $"Markov Drummer generator by Shorstok {ver?.Major}.{ver?.Minor}.{ver?.Revision}";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _viewmodel?.Shutdown();
            
            base.OnClosing(e);
        }
    }
}
