using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Testing.UI.Pages
{
    /// <summary>
    /// Interaction logic for IssuePage.xaml
    /// </summary>
    public partial class IssuePage
    {
        private readonly SolidColorBrush _middleTimerBrush = new SolidColorBrush(Colors.Orange);
        private readonly SolidColorBrush _lastTimerBrush = new SolidColorBrush(Colors.Red);

        public IssuePage()
        {
            InitializeComponent();

            if (Issues.IssueDb.IssuesSettings.IsTimeLimited)
                AppController.RemainingSecondsChanged += OnRemainingSecondsChanged;
        }

        private void MaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.MaximizeRestore();
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Minimize();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Close();
        }

        private void OnRemainingSecondsChanged(uint remainingSeconds)
        {
            if (remainingSeconds == 0)
            {
                AppController.RemainingSecondsChanged -= OnRemainingSecondsChanged;

                AppController.StopTimer();

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(GoToTimerOverPage));
            }
            else
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => RefreshTimerLabel(remainingSeconds)));
        }

        private void RefreshTimerLabel(uint remainingSeconds)
        {
            if (remainingSeconds <= Issues.IssueDb.IssuesSettings.TimeLimit * 60 / 10)
                _timerLabel.Foreground = _lastTimerBrush;
            else if (remainingSeconds <= Issues.IssueDb.IssuesSettings.TimeLimit * 60 / 2)
                _timerLabel.Foreground = _middleTimerBrush;

            var minutes = remainingSeconds / 60;
            var seconds = remainingSeconds % 60;

            _timerLabel.Content = string.Format("{0}:{1}", minutes.ToString(minutes > 99 ? "000" : "00"), seconds.ToString("00"));
        }

        private void GoToTimerOverPage()
        {
            if (NavigationService != null)
                NavigationService.Navigate(AppController.GetPage(ApplicationPages.TimeOverPage));
        }
    }
}
