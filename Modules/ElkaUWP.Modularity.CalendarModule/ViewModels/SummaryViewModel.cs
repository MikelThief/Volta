﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Anotar.NLog;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Extensions;
using ElkaUWP.DataLayer.Usos.Services;
using MvvmDialogs;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {
        private Uri _calFileHyperlink;
        private Uri _webCalFeedHyperlink;
        private readonly IDialogService _dialogService;

        private TimeTableService _timeTableService;

        public Uri ICalFileHyperlink
        {
            get => _calFileHyperlink;
            private set => SetProperty(storage: ref _calFileHyperlink, value: value, propertyName: nameof(ICalFileHyperlink));
        }

        public Uri WebCalFeedHyperlink
        {
            get => _webCalFeedHyperlink;
            private set => SetProperty(storage: ref _webCalFeedHyperlink, value: value, propertyName: nameof(WebCalFeedHyperlink));
        }
        public ObservableCollection<UserDeadline> UserDeadlines = new ObservableCollection<UserDeadline>();

        public ObservableCollection<CalendarEvent> CalendarEvents = new ObservableCollection<CalendarEvent>();


        #region CreateEventFlyout
        private DateTime? _createDeadlineFlyoutDateTime;
        private string _createDeadlineFlyoutTitle;
        private string _createDeadlineFlyoutDescription;


        public DateTime? CreateDeadlineFlyoutDateTime
        {
            get => _createDeadlineFlyoutDateTime;
            set => SetProperty(storage: ref _createDeadlineFlyoutDateTime, value: value, propertyName: nameof(CreateDeadlineFlyoutDateTime));
        }



        public string CreateDeadlineFlyOutTitle
        {
            get => _createDeadlineFlyoutTitle;
            set => SetProperty(storage: ref _createDeadlineFlyoutTitle, value: value, propertyName: nameof(CreateDeadlineFlyOutTitle));
        }

        public string CreateDeadlineFlyoutDescription
        {
            get => _createDeadlineFlyoutDescription;
            set => SetProperty(storage: ref _createDeadlineFlyoutDescription, value: value,
                propertyName: nameof(CreateDeadlineFlyoutDescription));
        }

        public DelegateCommand CreateEventCommand { get; private set; }
        public DelegateCommand<UserDeadline> RemoveUserDeadlineCommand { get; private set; }
        public AsyncCommand DownloadScheduleFromUsosCommand { get; private set; }
        #endregion

        public SummaryViewModel(TimeTableService timeTableService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _timeTableService = timeTableService;
            CreateEventCommand = new DelegateCommand(executeMethod: CreateNewDeadline);
            RemoveUserDeadlineCommand = new DelegateCommand<UserDeadline>(executeMethod: RemoveUserDeadline);
            DownloadScheduleFromUsosCommand = new AsyncCommand(executeAsync: DownloadSechuleFromUsosAsync);
            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
        }

        private async Task DownloadSechuleFromUsosAsync()
        {
            // TODO: Change ScheduleAppointmentCollection to own collection + mapping
            var result = await _timeTableService.GetTimeTableActivitiesForStudentAsync();

            foreach (var item in result)
            {
                var tempapt = item.AsScheduleAppointment();
                CalendarEvents.Add(tempapt);
            }
        }

        private void RemoveUserDeadline(UserDeadline obj)
        {
            UserDeadlines.Remove(item: obj);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {

            try
            {
                ICalFileHyperlink = new Uri(uriString: _timeTableService.GetICalFileUri());
            }
            catch (UriFormatException ufexc)
            {
                LogTo.WarnException(message: "Bad Uri string!\nStackTrace: " + ufexc.StackTrace + "\nException: ", exception: ufexc);
            }
            catch (NullReferenceException nrexc)
            {
                LogTo.WarnException(message: "No USOS user id specified. It should be obtained earlier!\nStackTrace: " + nrexc.StackTrace + "\nException: ", exception: nrexc);
            }

            try
            {
                WebCalFeedHyperlink = new Uri(uriString: _timeTableService.GetWebCalFeedUri());
            }
            catch (UriFormatException ufexc)
            {
                LogTo.WarnException(message: "Bad Uri string!\nStackTrace: " + ufexc.StackTrace + "\nException: ", exception: ufexc);
            }

        }

        public void CreateNewDeadline()
        {
            UserDeadlines.Add(item: new UserDeadline(date: CreateDeadlineFlyoutDateTime.Value, header: CreateDeadlineFlyOutTitle, description: CreateDeadlineFlyoutDescription));
            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
        }

        public async Task OpenCalendarEventDialog(DateTime startDateTime, CalendarEvent appointment)
        {
            var vm = appointment is null ? new CalendarEventDialogViewModel(proposedStartTime: startDateTime) : new CalendarEventDialogViewModel(appointment: appointment);

            var result = await _dialogService.ShowContentDialogAsync(viewModel: vm);

            if (result != ContentDialogResult.Primary) return;

            if (!(appointment is null))
            {
                CalendarEvents.Remove(item: appointment);
            }

            var item = vm.GetScheduleAppointment();
            CalendarEvents.Add(item: item);
        }
    }
}