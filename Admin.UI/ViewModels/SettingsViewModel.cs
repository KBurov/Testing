using System.Windows.Controls;
using System.Windows.Input;

using Testing.Common;

using Wanderer.Library.Common;
using Wanderer.Library.Wpf;

namespace Testing.Admin.UI.ViewModels
{
    public sealed class SettingsViewModel : BindableObject
    {
        #region Variables
        private readonly UserInfoSettings _userInfoSettings;
        private readonly IssuesSettings _issuesSettings;
        #endregion

        #region UserInfo Settings
        public bool IsPositionVisible
        {
            get { return _userInfoSettings.IsPositionVisible; }
            set
            {
                _userInfoSettings.IsPositionVisible = value;

                RaisePropertyChanged("IsPositionVisible");
            }
        }

        public bool IsLevelVisible
        {
            get { return _userInfoSettings.IsLevelVisible; }
            set
            {
                _userInfoSettings.IsLevelVisible = value;

                RaisePropertyChanged("IsLevelVisible");
            }
        }

        public bool IsExperienceVisible
        {
            get { return _userInfoSettings.IsExperienceVisible; }
            set
            {
                _userInfoSettings.IsExperienceVisible = value;

                RaisePropertyChanged("IsExperienceVisible");
            }
        }
        #endregion

        #region Issues Settings
        public bool IsTimeLimited
        {
            get { return _issuesSettings.IsTimeLimited; }
            set
            {
                _issuesSettings.IsTimeLimited = value;

                RaisePropertyChanged("IsTimeLimited");
            }
        }

        public uint TimeLimit
        {
            get { return _issuesSettings.TimeLimit; }
            set
            {
                _issuesSettings.TimeLimit = value;

                RaisePropertyChanged("TimeLimit");
            }
        }
        #endregion

        public InterfaceLanguages InterfaceLanguage
        {
            get { return AppController.CurrentInterfaceLanguage; }
            set
            {
                AppController.ApplyInterfaceLanguage(value);

                RaisePropertyChanged("InterfaceLanguage");
            }
        }
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public ICommand EditCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global
        public SettingsViewModel()
        {
            _userInfoSettings = Issues.IssueDb.UserInfoSettings as UserInfoSettings;
            _issuesSettings = Issues.IssueDb.IssuesSettings as IssuesSettings;

            EditCommand = new RelayCommand(EditIssues);
            SaveCommand = new RelayCommand(o => Issues.SaveIssueDb());
        }

        private void EditIssues(object o)
        {
            var page = o as Page;

            if (page != null && page.NavigationService != null)
                page.NavigationService.Navigate(AppController.GetPage(ApplicationPages.IssueSet1Page));
        }
    }
}