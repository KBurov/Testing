using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Testing.Common;
using Testing.UI.Classes;

using Wanderer.Library.Common;
using Wanderer.Library.Wpf;

namespace Testing.UI.ViewModels
{
    public sealed class UserFormViewModel : BindableObject
    {
        #region Constants
        private const int PositionNumber = 9;

        private const string PositionResourceNameTemplate = "Position{0:00}Title";

        private const int LevelNumber = 6;

        private const string LevelResourceNameTemplate = "Level{0:00}Title";

        private const int ExperienceNumber = 6;

        private const string ExperienceResourceNameTemplate = "Experience{0:00}Title";
        #endregion

        private bool _isUserFormError;
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public string CurrentDate { get; private set; }

        public ICommand NextCommand { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global
        public InterfaceLanguages InterfaceLanguage
        {
            get { return AppController.CurrentInterfaceLanguage; }
            set
            {
                AppController.ApplyInterfaceLanguage(value);

                RaisePropertyChanged("InterfaceLanguage");

                RefreshPositions();
                RefreshLevels();
                RefreshExperiences();

                foreach (var d in DistributionChannelSource)
                    d.InterfaceLanguage = InterfaceLanguage;
                foreach (var r in RegionSource)
                    r.InterfaceLanguage = InterfaceLanguage;
            }
        }

        #region UserInfo
        public string LastName { get { return AppController.UserInfo.LastName; } set { AppController.UserInfo.LastName = value; } }

        public string FirstName { get { return AppController.UserInfo.FirstName; } set { AppController.UserInfo.FirstName = value; } }

        public string MiddleName { get { return AppController.UserInfo.MiddleName; } set { AppController.UserInfo.MiddleName = value; } }

        public ObservableCollection<string> Positions { get; private set; }

        public string Position { get { return AppController.UserInfo.Position; } set { AppController.UserInfo.Position = value; } }

        public bool IsPositionVisible { get { return Issues.IssueDb.UserInfoSettings.IsPositionVisible; } }

        public ObservableCollection<string> Levels { get; private set; }

        public string Level { get { return AppController.UserInfo.Level; } set { AppController.UserInfo.Level = value; } }

        public bool IsLevelVisible { get { return Issues.IssueDb.UserInfoSettings.IsLevelVisible; } }

        public ObservableCollection<string> Experiences { get; private set; }

        public string Experience { get { return AppController.UserInfo.Experience; } set { AppController.UserInfo.Experience = value; } }

        public bool IsExperienceVisible { get { return Issues.IssueDb.UserInfoSettings.IsExperienceVisible; } }

        public ObservableCollection<DistributionChannelItem> DistributionChannelSource { get; private set; }

        public DistributionChannels DistributionChannel { get { return AppController.UserInfo.DistributionChannel; } set { AppController.UserInfo.DistributionChannel = value; } }

        public ObservableCollection<RegionItem> RegionSource { get; private set; }

        public Regions Region { get { return AppController.UserInfo.Region; } set { AppController.UserInfo.Region = value; } }

        public bool IsUserFormError
        {
            get { return _isUserFormError; }
            set
            {
                _isUserFormError = value;

                RaisePropertyChanged("IsUserFormError");
            }
        }
        #endregion

        #region Issues Settings
        public bool IsTimeLimited { get { return Issues.IssueDb.IssuesSettings.IsTimeLimited; } }

        public string TimeLimit
        {
            get
            {
                return
                    Issues.IssueDb.IssuesSettings.TimeLimit.ToString(Issues.IssueDb.IssuesSettings.TimeLimit > 99
                                                                         ? "000"
                                                                         : "00") + ":00";
            }
        }
        #endregion

        public UserFormViewModel()
        {
            CurrentDate = DateTime.Today.ToShortDateString();
            NextCommand = new RelayCommand(StartTesting);

            Positions = new ObservableCollection<string>();
            Levels = new ObservableCollection<string>();
            Experiences = new ObservableCollection<string>();
            DistributionChannelSource = new ObservableCollection<DistributionChannelItem>
                                            {
                                                new DistributionChannelItem(DistributionChannels.KA),
                                                new DistributionChannelItem(DistributionChannels.OnTrade),
                                                new DistributionChannelItem(DistributionChannels.OffTrade)
                                            };
            RegionSource = new ObservableCollection<RegionItem> {new RegionItem(Regions.EastUkraine), new RegionItem(Regions.WestUkraine)};

            RefreshPositions();
            RefreshLevels();
            RefreshExperiences();
        }

        private void StartTesting(object o)
        {
            if (!CheckValues()) return;

            var page = o as Page;

            if (page == null || page.NavigationService == null) return;

            if (Issues.IssueDb.GetIssueNumber(DistributionChannel, Region) > 0)
            {
                if (Issues.IssueDb.IssuesSettings.IsTimeLimited)
                    AppController.StartTimer(Issues.IssueDb.IssuesSettings.TimeLimit * 60);

                page.NavigationService.Navigate(AppController.GetPage(ApplicationPages.IssuePage));
            }
            else
                page.NavigationService.Navigate(AppController.GetPage(ApplicationPages.ResultPage));
        }

        private bool CheckValues()
        {
            var result = true;

            if (string.IsNullOrEmpty(LastName)
                || string.IsNullOrEmpty(FirstName)
                || string.IsNullOrEmpty(MiddleName)
                || (IsPositionVisible && string.IsNullOrEmpty(Position))
                || (IsLevelVisible && string.IsNullOrEmpty(Level))
                || (IsExperienceVisible && string.IsNullOrEmpty(Experience)))
            {
                IsUserFormError = true;
                result = false;
            }
            else
                IsUserFormError = false;

            return result;
        }

        private void RefreshPositions()
        {
            Positions.Clear();

            for (var i = 1;i <= PositionNumber;i++)
            {
                var resource = Application.Current.TryFindResource(string.Format(PositionResourceNameTemplate, i));

                if (resource == null) continue;

                Positions.Add(resource.ToString());
            }
        }

        private void RefreshLevels()
        {
            Levels.Clear();

            for (var i = 1; i <= LevelNumber; i++)
            {
                var resource = Application.Current.TryFindResource(string.Format(LevelResourceNameTemplate, i));

                if (resource == null) continue;

                Levels.Add(resource.ToString());
            }
        }

        private void RefreshExperiences()
        {
            Experiences.Clear();

            for (var i = 1; i <= ExperienceNumber; i++)
            {
                var resource = Application.Current.TryFindResource(string.Format(ExperienceResourceNameTemplate, i));

                if (resource == null) continue;

                Experiences.Add(resource.ToString());
            }
        }
    }
}