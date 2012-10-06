using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Testing.Admin.UI.Pages;
using Testing.Admin.UI.Properties;
using Testing.Admin.UI.ViewModels;
using Testing.Common;

namespace Testing.Admin.UI
{
    internal static class AppController
    {
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

        private static InterfaceLanguages _currentInterfaceLanguage = InterfaceLanguages.Ukraine;

        private static string SkinName { get { return Settings.Default.SkinName; } }

        public static InterfaceLanguages CurrentInterfaceLanguage { get { return _currentInterfaceLanguage; } }

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
                                case ApplicationPages.IssueSet1Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set1)});
                                    break;
                                case ApplicationPages.IssueSet2Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set2)});
                                    break;
                                case ApplicationPages.IssueSet3Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set3)});
                                    break;
                                case ApplicationPages.IssueSet4Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set4)});
                                    break;
                                case ApplicationPages.IssueSet5Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set5)});
                                    break;
                                case ApplicationPages.IssueSet6Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set6)});
                                    break;
                                case ApplicationPages.IssueSet7Page:
                                    _pages[page] = (result = new IssuePage {DataContext = new IssueViewModel(IssueSets.Set7)});
                                    break;
                                case ApplicationPages.SettingsPage:
                                    _pages[page] = (result = new SettingsPage {DataContext = new SettingsViewModel()});
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
    }
}