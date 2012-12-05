using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Testing.Admin.UI.Classes;
using Testing.Common;

using Wanderer.Library.Common;
using Wanderer.Library.Wpf;

namespace Testing.Admin.UI.ViewModels
{
    public sealed class IssueViewModel : BindableObject
    {
        private const int SelectionIssueAnswerNumber = 5;
        private const int MaxImageCount = 30;
        private const int OrderIssueAnswerNumber = 10;

        private readonly IssueSets _set;
        private readonly string _currentDirectory;

        private IIssue _currentIssue;
        private int _currentIssueNumber;
        private InterfaceLanguages _currentIssueLanguage;
        private int _selectedAnswerIndex;
        // Error flags
        private bool _isIssueContentError;
        private bool _isDistributionChannelOrRegionError;

        private IIssue CurrentIssue
        {
            get { return _currentIssue; }
            set
            {
                _currentIssue = value;

                Answers.Clear();
                PlacementCorrectAnswer1.Clear();
                PlacementCorrectAnswer2.Clear();
                PlacementCorrectAnswer3.Clear();
                PlacementCorrectAnswer4.Clear();
                PlacementCorrectAnswer5.Clear();

                if (!IsIssueSelected) return;

                SelectedAnswerIndex = IssueType == IssueTypes.ImageSelection || IssueType == IssueTypes.Placement
                                          ? -1
                                          : 0;

                foreach (var a in CurrentIssue.Answers)
                    Answers.Add(new VisualAnswer(a as Answer, _currentDirectory) {IssueLanguage = IssueLanguage});

                if (IssueType != IssueTypes.Placement) return;
// ReSharper disable AssignNullToNotNullAttribute
                foreach (var imageFileName in CurrentIssue.PlacementCorrectAnswer1)
                    PlacementCorrectAnswer1.Add(Path.Combine(_currentDirectory, imageFileName));
                foreach (var imageFileName in CurrentIssue.PlacementCorrectAnswer2)
                    PlacementCorrectAnswer2.Add(Path.Combine(_currentDirectory, imageFileName));
                foreach (var imageFileName in CurrentIssue.PlacementCorrectAnswer3)
                    PlacementCorrectAnswer3.Add(Path.Combine(_currentDirectory, imageFileName));
                foreach (var imageFileName in CurrentIssue.PlacementCorrectAnswer4)
                    PlacementCorrectAnswer4.Add(Path.Combine(_currentDirectory, imageFileName));
                foreach (var imageFileName in CurrentIssue.PlacementCorrectAnswer5)
                    PlacementCorrectAnswer5.Add(Path.Combine(_currentDirectory, imageFileName));
// ReSharper restore AssignNullToNotNullAttribute
            }
        }

        private IList<IIssue> SetIssues { get { return Issues.IssueDb.GetIssuesBySet(_set); } }

        #region Set buttons
        public bool IsSet1ButtonEnabled { get { return _set != IssueSets.Set1; } }
        public bool IsSet2ButtonEnabled { get { return _set != IssueSets.Set2; } }
        public bool IsSet3ButtonEnabled { get { return _set != IssueSets.Set3; } }
        public bool IsSet4ButtonEnabled { get { return _set != IssueSets.Set4; } }
        public bool IsSet5ButtonEnabled { get { return _set != IssueSets.Set5; } }
        public bool IsSet6ButtonEnabled { get { return _set != IssueSets.Set6; } }
        public bool IsSet7ButtonEnabled { get { return _set != IssueSets.Set7; } }
        #endregion

        #region Issue common parts
        public bool IsIssueSelected { get { return CurrentIssue != null; } }

        private int CurrentIssueNumber
        {
            get { return _currentIssueNumber; }
            set
            {
                _currentIssueNumber = value < 0 ? 0 : (value > SetIssues.Count - 1) ? SetIssues.Count - 1 : value;

                RaisePropertyChanged("DisplayedIssueNumber");
            }
        }

        public string DisplayedIssueNumber { get { return string.Format("{0}. ", CurrentIssueNumber + 1); } }
// ReSharper disable InconsistentNaming
        public bool IsKADitributionChannel
// ReSharper restore InconsistentNaming
        {
            get
            {
                return IsIssueSelected &&
                       (CurrentIssue.DistributionChannel & DistributionChannels.KA) == DistributionChannels.KA;
            }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (value)
                    issue.DistributionChannel |= DistributionChannels.KA;
                else
                    issue.DistributionChannel ^= DistributionChannels.KA;

                RaisePropertyChanged("IsKADitributionChannel");
            }
        }

        public bool IsOnTradeDitributionChannel
        {
            get
            {
                return IsIssueSelected &&
                       (CurrentIssue.DistributionChannel & DistributionChannels.OnTrade) == DistributionChannels.OnTrade;
            }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (value)
                    issue.DistributionChannel |= DistributionChannels.OnTrade;
                else
                    issue.DistributionChannel ^= DistributionChannels.OnTrade;

                RaisePropertyChanged("IsOnTradeDitributionChannel");
            }
        }

        public bool IsOffTradeDitributionChannel
        {
            get
            {
                return IsIssueSelected &&
                       (CurrentIssue.DistributionChannel & DistributionChannels.OffTrade) ==
                       DistributionChannels.OffTrade;
            }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (value)
                    issue.DistributionChannel |= DistributionChannels.OffTrade;
                else
                    issue.DistributionChannel ^= DistributionChannels.OffTrade;

                RaisePropertyChanged("IsOffTradeDitributionChannel");
            }
        }

        public bool IsWestUkraine
        {
            get { return IsIssueSelected && (CurrentIssue.Region & Regions.WestUkraine) == Regions.WestUkraine; }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (value)
                    issue.Region |= Regions.WestUkraine;
                else
                    issue.Region ^= Regions.WestUkraine;

                RaisePropertyChanged("IsWestUkraine");
            }
        }

        public bool IsEastUkraine
        {
            get { return IsIssueSelected && (CurrentIssue.Region & Regions.EastUkraine) == Regions.EastUkraine; }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (value)
                    issue.Region |= Regions.EastUkraine;
                else
                    issue.Region |= Regions.EastUkraine;

                RaisePropertyChanged("IsEastUkraine");
            }
        }

        public bool IsDistributionChannelOrRegionError
        {
            get { return _isDistributionChannelOrRegionError; }
            set
            {
                _isDistributionChannelOrRegionError = value;
                RaisePropertyChanged("IsDistributionChannelOrRegionError");
            }
        }

        public InterfaceLanguages IssueLanguage
        {
            get { return _currentIssueLanguage; }
            set
            {
                _currentIssueLanguage = value;

                RaisePropertyChanged("IssueLanguage");

                foreach (var a in Answers)
                    a.IssueLanguage = IssueLanguage;

                RaiseInterfaceUpdate();
            }
        }

        public string IssueContent
        {
            get
            {
                return IsIssueSelected
                           ? (IssueLanguage == InterfaceLanguages.Ukraine
                                  ? CurrentIssue.ContentUA
                                  : CurrentIssue.ContentRU)
                           : string.Empty;
            }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                if (IssueLanguage == InterfaceLanguages.Ukraine)
                    issue.ContentUA = value;
                else
                    issue.ContentRU = value;

                RaisePropertyChanged("IssueContent");
            }
        }

        public bool IsIssueContentError
        {
            get { return _isIssueContentError; }
            set
            {
                _isIssueContentError = value;
                RaisePropertyChanged("IsIssueContentError");
            }
        }

        public uint CorrectAnswerPoints
        {
            get { return IsIssueSelected ? CurrentIssue.CorrectAnswerPoints : 0u; }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                issue.CorrectAnswerPoints = value;
                RaisePropertyChanged("CorrectAnswerPoints");
            }
        }

        public IssueTypes IssueType { get { return IsIssueSelected ? CurrentIssue.Type : IssueTypes.Selection; } }

        public ObservableCollection<VisualAnswer> Answers { get; private set; }

        public int AnswersCount { get { return MaxImageCount; } }

        public bool IsIssueAdditionalDetailsVisible { get { return IssueType == IssueTypes.SelectionInImage || IssueType == IssueTypes.Placement; } }

        public int DetailsColumnSpan { get { return IsIssueAdditionalDetailsVisible ? 1 : 3; } }
        #endregion

        #region Selection issue parts
        public bool IsSelectionIssueAnswerError
        {
            get
            {
// ReSharper disable AssignNullToNotNullAttribute
                return IsIssueSelected && CurrentIssue.Answers.Any(
                    a => string.IsNullOrEmpty(a.ContentUA) || string.IsNullOrEmpty(a.ContentRU));
// ReSharper restore AssignNullToNotNullAttribute
            }
        }
        #endregion

        #region Selection in image issue parts
        public uint CorrectAnswer
        {
            get { return IsIssueSelected ? CurrentIssue.Answers[0].OrderNo : 1u; }
            set
            {
                if (!IsIssueSelected) return;

                var answer = CurrentIssue.Answers[0] as Answer;

                if (answer == null) return;

                answer.OrderNo = value;

                RaisePropertyChanged("CorrectAnswer");
            }
        }

        public string ImageFileName
        {
            get
            {
                return !IsIssueSelected || string.IsNullOrEmpty(CurrentIssue.Answers[0].ImageFileName)
                           ? string.Empty
// ReSharper disable AssignNullToNotNullAttribute
                           : Path.Combine(_currentDirectory, CurrentIssue.Answers[0].ImageFileName);
// ReSharper restore AssignNullToNotNullAttribute
            }
            set
            {
                if (!IsIssueSelected) return;

                var answer = CurrentIssue.Answers[0] as Answer;

                if (answer == null) return;

                answer.ImageFileName = value;

                RaisePropertyChanged("ImageFileName");
            }
        }

        public bool IsImageFileNameError { get { return IsIssueSelected && string.IsNullOrEmpty(CurrentIssue.Answers[0].ImageFileName); } }
        #endregion

        #region Order issue parts
        public int SelectedAnswerIndex
        {
            get { return _selectedAnswerIndex; }
            set
            {
                _selectedAnswerIndex = value;

                RaisePropertyChanged("SelectedAnswerIndex");
            }
        }
        #endregion

        #region Placement issue parts
        private uint NumberOfShelves
        {
            get { return IsIssueSelected ? CurrentIssue.NumberOfShelves : 2u; }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                issue.NumberOfShelves = value;
                // Correct placement answer collections
                if (NumberOfShelves < 5 && PlacementCorrectAnswer5.Count > 0)
                {
                    PlacementCorrectAnswer5.Clear();
                    CurrentIssue.PlacementCorrectAnswer5.Clear();
                }
                if (NumberOfShelves < 4 && PlacementCorrectAnswer4.Count > 0)
                {
                    PlacementCorrectAnswer4.Clear();
                    CurrentIssue.PlacementCorrectAnswer4.Clear();
                }
                if (NumberOfShelves < 3 && PlacementCorrectAnswer3.Count > 0)
                {
                    PlacementCorrectAnswer3.Clear();
                    CurrentIssue.PlacementCorrectAnswer3.Clear();
                }

                RaisePropertyChanged("IsThirdShelveVisible");
                RaisePropertyChanged("IsFourthShelveVisible");
                RaisePropertyChanged("IsFifthShelveVisible");
            }
        }

        public uint PlacesOnShelf
        {
            get { return IsIssueSelected ? CurrentIssue.PlacesOnShelf : 4u; }
            set
            {
                var issue = CurrentIssue as Issue;

                if (issue == null) return;

                issue.PlacesOnShelf = value;
                // Correct placement answer collections
                while (PlacementCorrectAnswer1.Count > PlacesOnShelf)
                {
                    PlacementCorrectAnswer1.RemoveAt(PlacementCorrectAnswer1.Count - 1);
                    CurrentIssue.PlacementCorrectAnswer1.RemoveAt(CurrentIssue.PlacementCorrectAnswer1.Count - 1);
                }
                while (PlacementCorrectAnswer2.Count > PlacesOnShelf)
                {
                    PlacementCorrectAnswer2.RemoveAt(PlacementCorrectAnswer2.Count - 1);
                    CurrentIssue.PlacementCorrectAnswer2.RemoveAt(CurrentIssue.PlacementCorrectAnswer2.Count - 1);
                }
                while (PlacementCorrectAnswer3.Count > PlacesOnShelf)
                {
                    PlacementCorrectAnswer3.RemoveAt(PlacementCorrectAnswer3.Count - 1);
                    CurrentIssue.PlacementCorrectAnswer3.RemoveAt(CurrentIssue.PlacementCorrectAnswer3.Count - 1);
                }
                while (PlacementCorrectAnswer4.Count > PlacesOnShelf)
                {
                    PlacementCorrectAnswer4.RemoveAt(PlacementCorrectAnswer4.Count - 1);
                    CurrentIssue.PlacementCorrectAnswer4.RemoveAt(CurrentIssue.PlacementCorrectAnswer4.Count - 1);
                }
                while (PlacementCorrectAnswer5.Count > PlacesOnShelf)
                {
                    PlacementCorrectAnswer5.RemoveAt(PlacementCorrectAnswer5.Count - 1);
                    CurrentIssue.PlacementCorrectAnswer5.RemoveAt(CurrentIssue.PlacementCorrectAnswer5.Count - 1);
                }

                RaisePropertyChanged("PlacesOnShelf");
            }
        }

        public bool IsTwoShelves { get { return NumberOfShelves == 2u; } set { if (value) NumberOfShelves = 2u; } }

        public bool IsThreeShelves { get { return NumberOfShelves == 3u; } set { if (value) NumberOfShelves = 3u; } }

        public bool IsFourShelves { get { return NumberOfShelves == 4u; } set { if (value) NumberOfShelves = 4u; } }

        public bool IsFiveShelves { get { return NumberOfShelves == 5u; } set { if (value) NumberOfShelves = 5u; } }

        public bool IsThirdShelveVisible { get { return NumberOfShelves > 2u; } }

        public bool IsFourthShelveVisible { get { return NumberOfShelves > 3u; } }

        public bool IsFifthShelveVisible { get { return NumberOfShelves > 4u; } }
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public ObservableCollection<uint> AvaliablePlaces { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global
        public ObservableCollection<string> PlacementCorrectAnswer1 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer2 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer3 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer4 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer5 { get; private set; }
        #endregion

        #region Interface commands
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public ICommand SettingsCommand { get; private set; }

        public ICommand SetCommand { get; private set; }

        public ICommand BackCommand { get; private set; }

        public ICommand NextCommand { get; private set; }
        #endregion

        #region Issue commands
        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteIssueCommand { get; private set; }

        public ICommand ImageSelectionCommand { get; private set; }

        public ICommand AddImageCommand { get; private set; }

        public ICommand DeleteImageCommand { get; private set; }

        #region Create issue commands
        public ICommand CreateSelectionIssueCommand { get; private set; }

        public ICommand CreateSelectionInImageIssueCommand { get; private set; }

        public ICommand CreateImageSelectionIssueCommand { get; private set; }

        public ICommand CreateOrderIssueCommand { get; private set; }

        public ICommand CreatePlacementIssueCommand { get; private set; }
        #endregion

        #region Order issue commands
        public ICommand MoveAnswerUpCommand { get; private set; }

        public ICommand MoveAnswerDownCommand { get; private set; }
        #endregion

        #region Placement issue commands
        public ICommand AddPlacementCorrectAnswerCommand { get; private set; }

        public ICommand DeletePlacementCorrectAnswerCommand { get; private set; }
        #endregion
// ReSharper restore UnusedAutoPropertyAccessor.Global
        #endregion

        public IssueViewModel(IssueSets set)
        {
            _set = set;
            _currentDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            // Interface commands
            SettingsCommand = new RelayCommand(Settings);
            SetCommand = new RelayCommand(GoToSetPage);
            BackCommand = new RelayCommand(o => PreviousIssue(), o => CurrentIssueNumber > 0);
            NextCommand = new RelayCommand(o => NextIssue(), o => CurrentIssueNumber < (SetIssues.Count - 1));
            // Issue commands
            SaveCommand = new RelayCommand(o => SaveIssues());
            DeleteIssueCommand = new RelayCommand(o => DeleteIssue(), o => IsIssueSelected);
            ImageSelectionCommand = new RelayCommand(o => ImageSelection());
            AddImageCommand = new RelayCommand(o => AddImage(), o => Answers.Count < MaxImageCount);
            DeleteImageCommand = new RelayCommand(o => DeleteImage(), o => SelectedAnswerIndex >= 0);
            // Order issue commands
            MoveAnswerUpCommand = new RelayCommand(o => MoveAnswerUp());
            MoveAnswerDownCommand = new RelayCommand(o => MoveAnswerDown());
            // Placement issue commands
            AddPlacementCorrectAnswerCommand = new RelayCommand(AddPlacementCorrectAnswer, o => SelectedAnswerIndex >= 0);
            DeletePlacementCorrectAnswerCommand = new RelayCommand(DeletePlacementCorrectAnswer);
            // Create issue commands
            CreateSelectionIssueCommand = new RelayCommand(o => CreateSelectionIssue());
            CreateSelectionInImageIssueCommand = new RelayCommand(o => CreateSelectionInImageIssue());
            CreateImageSelectionIssueCommand = new RelayCommand(o => CreateImageSelectionIssue());
            CreateOrderIssueCommand = new RelayCommand(o => CreateOrderIssue());
            CreatePlacementIssueCommand = new RelayCommand(o => CreatePlacementIssue());
            // Initialize answer collection
            Answers = new ObservableCollection<VisualAnswer>();
            // Initialize placement issue parts
            AvaliablePlaces = new ObservableCollection<uint> {4u, 5u, 6u, 7u, 8u, 9u, 10u};
            PlacementCorrectAnswer1 = new ObservableCollection<string>();
            PlacementCorrectAnswer2 = new ObservableCollection<string>();
            PlacementCorrectAnswer3 = new ObservableCollection<string>();
            PlacementCorrectAnswer4 = new ObservableCollection<string>();
            PlacementCorrectAnswer5 = new ObservableCollection<string>();
            // Initialize first issue
            if (SetIssues.Count > 0)
            {
                CurrentIssueNumber = 0;
                CurrentIssue = SetIssues[CurrentIssueNumber];
            }
            // Set default issue language
            IssueLanguage = AppController.CurrentInterfaceLanguage;
        }

        #region Interface methods
        private void Settings(object o)
        {
            if (!CheckValues()) return;

            var page = o as Page;

            if (page != null && page.NavigationService != null)
                page.NavigationService.Navigate(AppController.GetPage(ApplicationPages.SettingsPage));
        }

        private void GoToSetPage(object o)
        {
            if (!CheckValues()) return;

            var parameters = o as Tuple<Page, object>;

            if (parameters != null && parameters.Item1 != null && parameters.Item1.NavigationService != null)
            {
                int temp;

                if (int.TryParse(parameters.Item2.ToString(), out temp))
                    parameters.Item1.NavigationService.Navigate(AppController.GetPage((ApplicationPages) temp));
            }
        }

        private void PreviousIssue()
        {
            if (!CheckValues()) return;

            CurrentIssueNumber--;
            CurrentIssue = SetIssues[CurrentIssueNumber];

            RaiseInterfaceUpdate();
        }

        private void NextIssue()
        {
            if (!CheckValues()) return;

            CurrentIssueNumber++;
            CurrentIssue = SetIssues[CurrentIssueNumber];

            RaiseInterfaceUpdate();
        }
        #endregion

        #region Issue commands
        private void SaveIssues()
        {
            if (CheckValues())
                Issues.SaveIssueDb();
        }

        private void DeleteIssue()
        {
            if (!IsIssueSelected) return;
// ReSharper disable AssignNullToNotNullAttribute
            var imagesToDelete = Answers.Where(a => !string.IsNullOrEmpty(a.OriginalImageFileName)).Select(a => a.ImageFileName).ToList();
// ReSharper restore AssignNullToNotNullAttribute
            Issues.IssueDb.Issues.Remove(CurrentIssue);

            CurrentIssueNumber--;
            CurrentIssue = SetIssues.Count > 0 ? SetIssues[CurrentIssueNumber] : null;

            RaiseInterfaceUpdate();
            CheckValues();

            foreach (var imageFileName in imagesToDelete)
// ReSharper disable AssignNullToNotNullAttribute
                File.Delete(imageFileName);
// ReSharper restore AssignNullToNotNullAttribute
        }

        private void ImageSelection()
        {
            var openDialog = new OpenFileDialog {DefaultExt = "*.jpg", Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*", RestoreDirectory = true};

            if (openDialog.ShowDialog() != true) return;

            var imageFileName = Path.Combine(IssueDbAssistant.ImageFolder,
                                             string.Format("Set{0}Issue{1}ImageSelection{2}", ((int) _set) + 1,
                                                           CurrentIssueNumber + 1,
                                                           Path.GetExtension(openDialog.SafeFileName)));
// ReSharper disable AssignNullToNotNullAttribute
            File.Copy(openDialog.FileName, imageFileName, true);
// ReSharper restore AssignNullToNotNullAttribute
            ImageFileName = imageFileName;

            RaiseInterfaceUpdate();
        }

        private void AddImage()
        {
            var openDialog = new OpenFileDialog
                                 {DefaultExt = "*.jpg", Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*", Multiselect = true, RestoreDirectory = true};

            if (openDialog.ShowDialog() != true) return;

            foreach (var fileName in openDialog.FileNames)
            {
                var number = CurrentIssue.Answers.Count;

                if (number >= MaxImageCount) break;

                number++;

                var imageFileName = Path.Combine(IssueDbAssistant.ImageFolder,
                                                 string.Format("Set{0}Issue{1}Placement{2}{3}", ((int) _set) + 1, CurrentIssueNumber + 1, number.ToString("00"),
                                                               Path.GetExtension(fileName)));
                while (File.Exists(imageFileName))
                {
                    number++;
                    imageFileName = Path.Combine(IssueDbAssistant.ImageFolder,
                                                 string.Format("Set{0}Issue{1}Placement{2}{3}", ((int) _set) + 1, CurrentIssueNumber + 1, number.ToString("00"),
                                                               Path.GetExtension(fileName)));
                }
// ReSharper disable AssignNullToNotNullAttribute
                File.Copy(fileName, imageFileName, true);
// ReSharper restore AssignNullToNotNullAttribute
                var newImageAnswer = new Answer {ImageFileName = imageFileName, Selected = false};

                CurrentIssue.Answers.Add(newImageAnswer);
                Answers.Add(new VisualAnswer(newImageAnswer, _currentDirectory));
            }

            RaiseInterfaceUpdate();
        }

        private void DeleteImage()
        {
            var fullImageFileName = Answers[SelectedAnswerIndex].ImageFileName;

            if (IssueType == IssueTypes.Placement)
            {
                var imageFileName = Answers[SelectedAnswerIndex].OriginalImageFileName;

                while (PlacementCorrectAnswer1.Contains(fullImageFileName))
                    PlacementCorrectAnswer1.Remove(fullImageFileName);
                while (PlacementCorrectAnswer2.Contains(fullImageFileName))
                    PlacementCorrectAnswer2.Remove(fullImageFileName);
                while (PlacementCorrectAnswer3.Contains(fullImageFileName))
                    PlacementCorrectAnswer3.Remove(fullImageFileName);
                while (PlacementCorrectAnswer4.Contains(fullImageFileName))
                    PlacementCorrectAnswer4.Remove(fullImageFileName);
                while (PlacementCorrectAnswer5.Contains(fullImageFileName))
                    PlacementCorrectAnswer5.Remove(fullImageFileName);

                while (CurrentIssue.PlacementCorrectAnswer1.Contains(imageFileName))
                    CurrentIssue.PlacementCorrectAnswer1.Remove(imageFileName);
                while (CurrentIssue.PlacementCorrectAnswer2.Contains(imageFileName))
                    CurrentIssue.PlacementCorrectAnswer2.Remove(imageFileName);
                while (CurrentIssue.PlacementCorrectAnswer3.Contains(imageFileName))
                    CurrentIssue.PlacementCorrectAnswer3.Remove(imageFileName);
                while (CurrentIssue.PlacementCorrectAnswer4.Contains(imageFileName))
                    CurrentIssue.PlacementCorrectAnswer4.Remove(imageFileName);
                while (CurrentIssue.PlacementCorrectAnswer5.Contains(imageFileName))
                    CurrentIssue.PlacementCorrectAnswer5.Remove(imageFileName);
            }

            CurrentIssue.Answers.RemoveAt(SelectedAnswerIndex);

            Answers.RemoveAt(SelectedAnswerIndex);

            RaiseInterfaceUpdate();
// ReSharper disable AssignNullToNotNullAttribute
            File.Delete(fullImageFileName);
// ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region Order issue commands
        private void MoveAnswerUp()
        {
            if (SelectedAnswerIndex <= 0) return;

            var newIndex = SelectedAnswerIndex - 1;
// ReSharper disable InconsistentNaming
            var tempContentUA = Answers[newIndex].ContentUA;
            var tempContentRU = Answers[newIndex].ContentRU;
// ReSharper restore InconsistentNaming
            Answers[newIndex].ContentUA = Answers[SelectedAnswerIndex].ContentUA;
            Answers[newIndex].ContentRU = Answers[SelectedAnswerIndex].ContentRU;

            Answers[SelectedAnswerIndex].ContentUA = tempContentUA;
            Answers[SelectedAnswerIndex].ContentRU = tempContentRU;

            SelectedAnswerIndex--;

            RaiseInterfaceUpdate();
        }

        private void MoveAnswerDown()
        {
            if (SelectedAnswerIndex >= OrderIssueAnswerNumber || SelectedAnswerIndex < 0) return;

            var newIndex = SelectedAnswerIndex + 1;
// ReSharper disable InconsistentNaming
            var tempContentUA = Answers[newIndex].ContentUA;
            var tempContentRU = Answers[newIndex].ContentRU;
// ReSharper restore InconsistentNaming
            Answers[newIndex].ContentUA = Answers[SelectedAnswerIndex].ContentUA;
            Answers[newIndex].ContentRU = Answers[SelectedAnswerIndex].ContentRU;

            Answers[SelectedAnswerIndex].ContentUA = tempContentUA;
            Answers[SelectedAnswerIndex].ContentRU = tempContentRU;

            SelectedAnswerIndex++;

            RaiseInterfaceUpdate();
        }
        #endregion

        #region Placement issue commands
        private void AddPlacementCorrectAnswer(object o)
        {
            var shelfNumber = o.ToString();
            var imageFileName = Answers[SelectedAnswerIndex].OriginalImageFileName;
            var fullImageFileName = Answers[SelectedAnswerIndex].ImageFileName;

            switch (shelfNumber)
            {
                case "1":
                    CurrentIssue.PlacementCorrectAnswer1.Add(imageFileName);
                    PlacementCorrectAnswer1.Add(fullImageFileName);
                    break;
                case "2":
                    CurrentIssue.PlacementCorrectAnswer2.Add(imageFileName);
                    PlacementCorrectAnswer2.Add(fullImageFileName);
                    break;
                case "3":
                    CurrentIssue.PlacementCorrectAnswer3.Add(imageFileName);
                    PlacementCorrectAnswer3.Add(fullImageFileName);
                    break;
                case "4":
                    CurrentIssue.PlacementCorrectAnswer4.Add(imageFileName);
                    PlacementCorrectAnswer4.Add(fullImageFileName);
                    break;
                case "5":
                    CurrentIssue.PlacementCorrectAnswer5.Add(imageFileName);
                    PlacementCorrectAnswer5.Add(fullImageFileName);
                    break;
            }
        }

        private void DeletePlacementCorrectAnswer(object o)
        {
            var listView = o as ListView;

            if (listView == null || listView.SelectedIndex == -1) return;

            var selectedIndex = listView.SelectedIndex;
            var visualPlacementCorrectAnswer = listView.ItemsSource as ObservableCollection<string>;

            if (visualPlacementCorrectAnswer == null) return;
            // Determine shelf number
            IList<string> originalPlacementCorrectAnswer = null;

            if (visualPlacementCorrectAnswer == PlacementCorrectAnswer1)
                originalPlacementCorrectAnswer = CurrentIssue.PlacementCorrectAnswer1;
            else if (visualPlacementCorrectAnswer == PlacementCorrectAnswer2)
                originalPlacementCorrectAnswer = CurrentIssue.PlacementCorrectAnswer2;
            else if (visualPlacementCorrectAnswer == PlacementCorrectAnswer3)
                originalPlacementCorrectAnswer = CurrentIssue.PlacementCorrectAnswer3;
            else if (visualPlacementCorrectAnswer == PlacementCorrectAnswer4)
                originalPlacementCorrectAnswer = CurrentIssue.PlacementCorrectAnswer4;
            else if (visualPlacementCorrectAnswer == PlacementCorrectAnswer5)
                originalPlacementCorrectAnswer = CurrentIssue.PlacementCorrectAnswer5;

            if (originalPlacementCorrectAnswer == null) return;

            visualPlacementCorrectAnswer.RemoveAt(selectedIndex);
            originalPlacementCorrectAnswer.RemoveAt(selectedIndex);
        }

        #region Create issue commands
        private void CreateSelectionIssue()
        {
            if (!CheckValues()) return;

            CreateIssue(IssueTypes.Selection, SelectionIssueProcessing);

            RaiseInterfaceUpdate();
        }

        private void CreateSelectionInImageIssue()
        {
            if (!CheckValues()) return;

            CreateIssue(IssueTypes.SelectionInImage, SelectionInImageIssueProcessing);

            RaiseInterfaceUpdate();
        }

        private void CreateImageSelectionIssue()
        {
            if (!CheckValues()) return;

            CreateIssue(IssueTypes.ImageSelection, null);

            RaiseInterfaceUpdate();
        }

        private void CreateOrderIssue()
        {
            if (!CheckValues()) return;

            CreateIssue(IssueTypes.Order, OrderIssueProcessing);

            RaiseInterfaceUpdate();
        }

        private void CreatePlacementIssue()
        {
            if (!CheckValues()) return;

            CreateIssue(IssueTypes.Placement, PlacementIssueProcessing);

            RaiseInterfaceUpdate();
        }
        #endregion
        #endregion

        private void CreateIssue(IssueTypes issueType, Action<IIssue> issueProcessing)
        {
            var issue = new Issue
            {
                DistributionChannel = DistributionChannels.KA | DistributionChannels.OnTrade | DistributionChannels.OffTrade,
                Region = Regions.WestUkraine | Regions.EastUkraine,
                Type = issueType,
                Set = _set
            };

            if (issueProcessing != null)
                issueProcessing(issue);

            Issues.IssueDb.Issues.Add(issue);

            CurrentIssueNumber = SetIssues.Count - 1;
            CurrentIssue = SetIssues[CurrentIssueNumber];
        }

        private static void SelectionIssueProcessing(IIssue issue)
        {
            for (var i = 0;i < SelectionIssueAnswerNumber;i++)
                issue.Answers.Add(new Answer {IsCorrect = i == 0});
        }

        private static void SelectionInImageIssueProcessing(IIssue issue)
        {
            issue.Answers.Add(new Answer());
        }

        private static void OrderIssueProcessing(IIssue issue)
        {
            for (var i = 0u;i < OrderIssueAnswerNumber;i++)
                issue.Answers.Add(new Answer {OrderNo = i});
        }

        private static void PlacementIssueProcessing(IIssue issue)
        {
            var i = issue as Issue;

            if (i == null) return;

            i.NumberOfShelves = 2u;
            i.PlacesOnShelf = 4u;
        }

        private bool CheckValues()
        {
            if (!IsIssueSelected)
                return true;

            var result = true;

            if (string.IsNullOrEmpty(CurrentIssue.ContentUA) || string.IsNullOrEmpty(CurrentIssue.ContentRU))
            {
                IsIssueContentError = true;
                result = false;
            }
            else
                IsIssueContentError = false;

            if ((!IsKADitributionChannel && !IsOnTradeDitributionChannel && !IsOffTradeDitributionChannel) || (!IsWestUkraine && !IsEastUkraine))
            {
                IsDistributionChannelOrRegionError = true;
                result = false;
            }
            else
                IsDistributionChannelOrRegionError = false;

            if (IssueType == IssueTypes.Selection || IssueType == IssueTypes.Order)
            {
                result = !IsSelectionIssueAnswerError;

                RaisePropertyChanged("IsSelectionIssueAnswerError");
            }

            if (IssueType == IssueTypes.SelectionInImage)
            {
                result = !IsImageFileNameError;

                RaisePropertyChanged("IsImageFileNameError");
            }

            return result;
        }

        private void RaiseInterfaceUpdate()
        {
            RaisePropertyChanged("IsIssueSelected");
            RaisePropertyChanged("IsKADitributionChannel");
            RaisePropertyChanged("IsOnTradeDitributionChannel");
            RaisePropertyChanged("IsOffTradeDitributionChannel");
            RaisePropertyChanged("IsWestUkraine");
            RaisePropertyChanged("IsEastUkraine");
            RaisePropertyChanged("IssueContent");
            RaisePropertyChanged("IssueType");
            RaisePropertyChanged("CorrectAnswerPoints");
            RaisePropertyChanged("IsIssueAdditionalDetailsVisible");
            RaisePropertyChanged("DetailsColumnSpan");
        }
    }
}