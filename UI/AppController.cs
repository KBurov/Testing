using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using Testing.Common;
using Testing.UI.Pages;
using Testing.UI.Properties;
using Testing.UI.ViewModels;

using Timer = System.Timers.Timer;

namespace Testing.UI
{
    internal static class AppController
    {
        public const string PlacementScrollViewerName = "_placementScrollViewer";

        private const string ResultTemplate = "<Row ss:AutoFitHeight=\"0\"><Cell><Data ss:Type=\"String\">{0}</Data></Cell><Cell><Data ss:Type=\"String\">{1}</Data></Cell><Cell><Data ss:Type=\"String\">{2}</Data></Cell><Cell><Data ss:Type=\"String\">{3}</Data></Cell><Cell><Data ss:Type=\"String\">{4}</Data></Cell><Cell><Data ss:Type=\"String\">{5}</Data></Cell></Row>";

        private static readonly IDictionary<InterfaceLanguages, Uri> _languageUri = new Dictionary<InterfaceLanguages, Uri>
                                                                                        {
                                                                                            {
                                                                                                InterfaceLanguages.Ukraine,
                                                                                                new Uri(
                                                                                                "/Testing.Resources;component/Strings/StringResources.uk-UA.xaml",
                                                                                                UriKind.Relative)
                                                                                                },
                                                                                            {
                                                                                                InterfaceLanguages.Russian,
                                                                                                new Uri(
                                                                                                "/Testing.Resources;component/Strings/StringResources.ru-RU.xaml",
                                                                                                UriKind.Relative)
                                                                                                }
                                                                                        };
        private static readonly IDictionary<ApplicationPages, Page> _pages = new Dictionary<ApplicationPages, Page>();
        private static readonly ReaderWriterLockSlim _pageLocker = new ReaderWriterLockSlim();
        private static readonly UserInfo _userInfo = new UserInfo { DistributionChannel = DistributionChannels.KA, Region = Regions.EastUkraine };
        private static readonly Timer _timer = new Timer(1000) { AutoReset = true };

        private static InterfaceLanguages _currentInterfaceLanguage = InterfaceLanguages.Ukraine;

        private static string SkinName { get { return Settings.Default.SkinName; } }

        public static InterfaceLanguages CurrentInterfaceLanguage { get { return _currentInterfaceLanguage; } }

        public static UserInfo UserInfo { get { return _userInfo; } }

        public static uint RemainingSeconds { get; set; }

        public static event Action<uint> RemainingSecondsChanged;

        public static void ApplySkin()
        {
            MergeResourceDictionary(new Uri(string.Format("/Testing.Resources;component/Skins/{0}.xaml", SkinName), UriKind.Relative));
        }

        public static void ApplyInterfaceLanguage()
        {
            switch (Settings.Default.InterfaceLanguage)
            {
                case "uk-UA":
                    ApplyInterfaceLanguage(InterfaceLanguages.Ukraine);
                    break;
                case "ru-RU":
                    ApplyInterfaceLanguage(InterfaceLanguages.Russian);
                    break;
            }
        }

        public static void ApplyInterfaceLanguage(InterfaceLanguages language)
        {
            RemoveResourceDictionary(language == InterfaceLanguages.Ukraine
                                         ? _languageUri[InterfaceLanguages.Russian]
                                         : _languageUri[InterfaceLanguages.Ukraine]);

            MergeResourceDictionary(_languageUri[language]);

            _currentInterfaceLanguage = language;
        }

        public static Page GetPage(ApplicationPages page)
        {
            Page result = null;

            using (_pageLocker.GetUpgradeableReadLock())
            {
                if (!_pages.ContainsKey(page))
                {
                    using (_pageLocker.GetWriteLock())
                    {
                        if (!_pages.ContainsKey(page))
                        {
                            switch (page)
                            {
                                case ApplicationPages.UserFormPage:
                                    _pages[page] = (result = new UserFormPage {DataContext = new UserFormViewModel()});
                                    break;
                                case ApplicationPages.IssuePage:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel()});
                                    break;
                                case ApplicationPages.TimeOverPage:
                                    _pages[page] = (result = new TimeOverPage());
                                    break;
                                case ApplicationPages.ResultPage:
                                    _pages[page] = (result = new ResultPage {DataContext = new ResultViewModel()});
                                    break;
                            }
                        }
                    }
                }
                else
                    result = _pages[page];
            }

            return result;
        }

        public static void MaximizeRestore()
        {
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal
                                                             ? WindowState.Maximized
                                                             : WindowState.Normal;
        }

        public static void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public static void Close()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.Shutdown();
        }

        public static void StartTimer(uint seconds)
        {
            RemainingSeconds = seconds;

            _timer.Elapsed += OnTimer;

            _timer.Start();
        }

        public static void StopTimer()
        {
            _timer.Elapsed -= OnTimer;

            _timer.Stop();
        }

        public static void SaveResult()
        {
            string template;

            using (var s = typeof(Issues).Assembly.GetManifestResourceStream("Testing.UI.Results.ResultFile.xml"))
// ReSharper disable AssignNullToNotNullAttribute
            using (var sr = new StreamReader(s))
// ReSharper restore AssignNullToNotNullAttribute
                template = sr.ReadToEnd();
// ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(Settings.Default.ResultsFolder))
                Directory.CreateDirectory(Settings.Default.ResultsFolder);
// ReSharper restore AssignNullToNotNullAttribute
            var fullName = string.Format("{0} {1} {2}", UserInfo.LastName, UserInfo.FirstName, UserInfo.MiddleName);
// ReSharper disable AssignNullToNotNullAttribute
            using (var sw = new StreamWriter(Path.Combine(Settings.Default.ResultsFolder, fullName + ".xml")))
// ReSharper restore AssignNullToNotNullAttribute
            {
                sw.Write(string.Format(
                    template,
                    Application.Current.FindResource("LastNameTitle"),
                    Application.Current.FindResource("FirstNameTitle"),
                    Application.Current.FindResource("MiddleNameTitle"),
                    Application.Current.FindResource("DateTitle"),
                    Application.Current.FindResource("PositionTitle"),
                    Application.Current.FindResource("LevelTitle"),
                    Application.Current.FindResource("ExperienceTitle"),
                    UserInfo.LastName, UserInfo.FirstName, UserInfo.MiddleName, fullName,
                    DateTime.Today.ToString("s"),
                    Issues.IssueDb.UserInfoSettings.IsPositionVisible ? UserInfo.Position : "-",
                    Issues.IssueDb.UserInfoSettings.IsLevelVisible ? UserInfo.Level : "-",
                    Issues.IssueDb.UserInfoSettings.IsExperienceVisible ? UserInfo.Experience : "-",
                    Application.Current.FindResource("ResultPageTitle"),
                    Application.Current.FindResource("SetTitle"),
                    Application.Current.FindResource("IssueTitle"),
                    Application.Current.FindResource("AnswerTitle"),
                    Application.Current.FindResource("IsCorrectTitle"),
                    Application.Current.FindResource("CorrectAnswerTitle"),
                    Application.Current.FindResource("AnswerPointsTitle"),
                    GetResults()));
            }
        }

        private static void MergeResourceDictionary(Uri dictionaryUri)
        {
            // Load ResourceDictionary into memory
// ReSharper disable AssignNullToNotNullAttribute
            var dictionary = Application.LoadComponent(dictionaryUri) as ResourceDictionary;
// ReSharper restore AssignNullToNotNullAttribute
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            mergedDictionaries.Add(dictionary);
        }

        private static void RemoveResourceDictionary(Uri dictionaryUri)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            var dictionaryForRemove = mergedDictionaries.SingleOrDefault(d => d.Source == dictionaryUri);

            if (dictionaryForRemove != null)
                mergedDictionaries.Remove(dictionaryForRemove);
        }

        private static void OnTimer(object source, ElapsedEventArgs e)
        {
            if (RemainingSeconds > 0)
                RemainingSeconds--;

            var rsc = RemainingSecondsChanged;

            if (rsc != null)
                rsc(RemainingSeconds);
        }

        private static string GetResults()
        {
            var builder = new StringBuilder();
            var yesTitle = Application.Current.FindResource("YesTitle");
            var noTitle = Application.Current.FindResource("NoTitle");

            foreach (var answerSet in UserInfo.Answers)
            {
                for (var i = 0;i < answerSet.Value.Count;i++)
                {
                    var userAnswer = answerSet.Value[i];

                    builder.AppendFormat(
                        ResultTemplate,
                        (int) answerSet.Key + 1,
                        i + 1,
                        userAnswer.Answer,
                        userAnswer.IsCorrect ? yesTitle : noTitle,
                        userAnswer.CorrectAnswer,
                        userAnswer.Points);
                }
            }

            return builder.ToString();
        }
    }
}
