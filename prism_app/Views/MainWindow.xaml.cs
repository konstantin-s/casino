using System;
using Unity;
using Prism.Regions;
using System.Windows;
using Prism.Ioc;
using prism_app.ViewModels;

namespace prism_app.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppLog _logger;
        private MainWindowViewModel _vm;

        public MainWindow(AppLog logger)
        {
            InitializeComponent();

            _logger = logger;
            _vm = DataContext as MainWindowViewModel;

            _logger.Log($@"StartViewModel HERE ☺!");

            this.Loaded += (sender, args) => _vm.MainWindow_Loaded();
        }
    }
}