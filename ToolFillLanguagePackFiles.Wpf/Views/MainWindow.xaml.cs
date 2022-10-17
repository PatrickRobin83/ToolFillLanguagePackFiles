﻿using System.Windows;
using ToolFillLanguagePackFiles.Wpf.ViewModels;

namespace ToolFillLanguagePackFiles.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }
}
