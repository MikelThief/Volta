﻿using ElkaUWP.Modularity.MapsModule.ViewModels;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Mvvm;
using RavinduL.LocalNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.MapsModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsView : Page
    {
        private MapsViewModel ViewModel => DataContext as MapsViewModel;

        public MapsView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }



        private void NotificationGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationManager = new LocalNotificationManager(grid: NotificationGrid);
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainPivot.SelectedIndex == 1)
            {
                if(ViewModel.CheckIsInternetAvailable())
                {
                   VirtualWalk.Source = new Uri( "http://mapy.ii.pw.edu.pl/iv.PW_WEITI/");
                }

            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            String search = Search.Text;


        }

        private void Search_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                OK_Click(this, new RoutedEventArgs());
            }

        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
        }
    }
}
