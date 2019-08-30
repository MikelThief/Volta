﻿using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ElkaUWP.Infrastructure;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Propertiary.Services;

namespace ElkaUWP.Modularity.MapsModule.ViewModels
{

    public class MapsViewModel : BindableBase, INavigatedAware
    {
        public LocalNotificationManager NotificationManager { get; set; }

        private readonly ResourceLoader _resourceLoader =
         ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(MapsModuleInitializer));

        private readonly MapsService _mapsService;

        private FloorPlan _selectedFloorPlan;

        public FloorPlan SelectedFloorPlan
        {
            get => _selectedFloorPlan;
            set
            {
                SetProperty(storage: ref _selectedFloorPlan, value: value, propertyName: nameof(SelectedFloorPlan));
                RaisePropertyChanged(propertyName: nameof(FloorPlanUri));
            }
        }

        public Uri FloorPlanUri => SelectedFloorPlan.ImageUri;

        public ObservableCollection<FloorPlan> FloorPlansCollection { get; private set; }

        public MapsViewModel(MapsService mapsService)
        {
            _mapsService = mapsService;
            FloorPlansCollection = new ObservableCollection<FloorPlan>(
                collection: _mapsService.GetFloorPlans(building: Constants.FEiTBuilding));

            SelectedFloorPlan = FloorPlansCollection.Single(plan => plan.Level == 0);
        }


       public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public bool CheckIsInternetAvailable()
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                NotificationManager.Show(notification: new SimpleNotification
                {
                    TimeSpan = TimeSpan.FromSeconds(value: 4),
                    Text = _resourceLoader.GetString(resource: "No_Internet_Notification"),
                    Glyph = "\uF384",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = new SolidColorBrush(color: Constants.RedColor)
                }
                );
                return false;
            }
            else
            {
                 return true;
            }
        }

        public void UnableToFind()
        {
            NotificationManager.Show(notification: new SimpleNotification
            {
                TimeSpan = TimeSpan.FromSeconds(value: 4),
                Text = _resourceLoader.GetString(resource: "Unable_to_find"),
                Glyph = "\uE721",
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = new SolidColorBrush(color: Constants.RedColor)
            }
            );
        }


    }
}
