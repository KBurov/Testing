using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Testing.Common;
using Testing.UI.Classes;

using Wanderer.Library.Common;
using Wanderer.Library.Wpf;

namespace Testing.UI.ViewModels
{
    public sealed class IssueViewModel : BindableObject
    {
        private const int OrderIssueAnswerNumber = 10;

        private readonly string _currentDirectory;

        private IssueSets _currentSet;
        private IIssue _currentIssue;
        private int _currentIssueNumber;
        private int _selectedAnswerIndex;

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

                if (IssueType == IssueTypes.Order)
                {
// ReSharper disable AssignNullToNotNullAttribute
                    var unsortedAnswers = CurrentIssue.Answers.OrderBy(a => a.OrderNo % 2 == 0 ? 0 : 1);
// ReSharper restore AssignNullToNotNullAttribute
                    foreach (var a in unsortedAnswers)
                        Answers.Add(new VisualAnswer(a, _currentDirectory));
                }
                else
                {
                    foreach (var a in CurrentIssue.Answers)
                        Answers.Add(new VisualAnswer(a, _currentDirectory));

                    if (IssueType != IssueTypes.Placement) return;

                    RaisePropertyChanged("PlacesOnShelf");
                    RaisePropertyChanged("IsThirdShelveVisible");
                    RaisePropertyChanged("IsFourthShelveVisible");
                    RaisePropertyChanged("IsFifthShelveVisible");
                }
            }
        }

        private IList<IIssue> SetIssues { get { return Issues.IssueDb.GetIssues(AppController.UserInfo.DistributionChannel, AppController.UserInfo.Region, _currentSet); } }

        #region Set Buttons
        public bool IsSet1ButtonEnabled { get { return _currentSet != IssueSets.Set1; } }
        public bool IsSet2ButtonEnabled { get { return _currentSet != IssueSets.Set2; } }
        public bool IsSet3ButtonEnabled { get { return _currentSet != IssueSets.Set3; } }
        public bool IsSet4ButtonEnabled { get { return _currentSet != IssueSets.Set4; } }
        public bool IsSet5ButtonEnabled { get { return _currentSet != IssueSets.Set5; } }
        public bool IsSet6ButtonEnabled { get { return _currentSet != IssueSets.Set6; } }
        public bool IsSet7ButtonEnabled { get { return _currentSet != IssueSets.Set7; } }
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

        public string IssueContent
        {
            get
            {
                return IsIssueSelected
                           ? (AppController.CurrentInterfaceLanguage == InterfaceLanguages.Ukraine
                                  ? CurrentIssue.ContentUA
                                  : CurrentIssue.ContentRU)
                           : string.Empty;
            }
        }

        public IssueTypes IssueType { get { return IsIssueSelected ? CurrentIssue.Type : IssueTypes.Selection; } }

        public ObservableCollection<VisualAnswer> Answers { get; private set; }

        public bool IsIssueAdditionalDetailsVisible { get { return IssueType == IssueTypes.SelectionInImage || IssueType == IssueTypes.Placement; } }

        public int DetailsColumnSpan { get { return IsIssueAdditionalDetailsVisible ? 1 : 3; } }
        #endregion

        #region Selection in image issue parts
        private uint CorrectAnswer { get { return IsIssueSelected ? CurrentIssue.Answers[0].OrderNo : 1u; } }

        public uint UserAnswer
        {
            get { return IsIssueSelected ? Answers[0].UserAnswer : 1u; }
            set
            {
                if (!IsIssueSelected) return;

                Answers[0].UserAnswer = value;

                RaisePropertyChanged("UserAnswer");
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
        }
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
        private uint NumberOfShelves { get { return IsIssueSelected ? CurrentIssue.NumberOfShelves : 2u; } }

        public uint PlacesOnShelf { get { return IsIssueSelected ? CurrentIssue.PlacesOnShelf : 4u; } }

        public bool IsThirdShelveVisible { get { return NumberOfShelves > 2u; } }

        public bool IsFourthShelveVisible { get { return NumberOfShelves > 3u; } }

        public bool IsFifthShelveVisible { get { return NumberOfShelves > 4u; } }

        public ObservableCollection<string> PlacementCorrectAnswer1 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer2 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer3 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer4 { get; private set; }

        public ObservableCollection<string> PlacementCorrectAnswer5 { get; private set; }
        #endregion
// ReSharper disable UnusedAutoPropertyAccessor.Global
        #region Interface commands
        public ICommand SkipCommand { get; private set; }

        public ICommand NextCommand { get; private set; }
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
        public IssueViewModel()
        {
            _currentDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            // Interface commands
            SkipCommand = new RelayCommand(o => SkipIssue());
            NextCommand = new RelayCommand(NextIssue);
            // Order issue commands
            MoveAnswerUpCommand = new RelayCommand(o => MoveAnswerUp());
            MoveAnswerDownCommand = new RelayCommand(o => MoveAnswerDown());
            // Placement issue commands
            AddPlacementCorrectAnswerCommand = new RelayCommand(AddPlacementCorrectAnswer, o => SelectedAnswerIndex >= 0);
            DeletePlacementCorrectAnswerCommand = new RelayCommand(DeletePlacementCorrectAnswer);
            // Initialize answer collection
            Answers = new ObservableCollection<VisualAnswer>();
            // Initialize placement issue parts
            PlacementCorrectAnswer1 = new ObservableCollection<string>();
            PlacementCorrectAnswer2 = new ObservableCollection<string>();
            PlacementCorrectAnswer3 = new ObservableCollection<string>();
            PlacementCorrectAnswer4 = new ObservableCollection<string>();
            PlacementCorrectAnswer5 = new ObservableCollection<string>();
            // Setup first issue
            SkipIssue();
        }

        #region Interface commands
        private void SkipIssue()
        {
            SetNewIssue(GetNextIssue());
        }

        private void NextIssue(object o)
        {
            CurrentIssue.IsAnswered = true;
            // Save answer
            var isCorrect = false;

            switch (IssueType)
            {
                case IssueTypes.Selection:
                    {
// ReSharper disable PossibleNullReferenceException
                        var selectedByUserIssueIndex = Answers.IndexOf(Answers.First(a => a.SelectedByUser)) + 1;
                        var correctIssueIndex = CurrentIssue.Answers.IndexOf(CurrentIssue.Answers.First(a => a.IsCorrect)) + 1;
// ReSharper restore PossibleNullReferenceException
                        AppController.UserInfo.Answers[_currentSet].Add(new UserAnswer
                                                                            {
                                                                                IsCorrect = (isCorrect = selectedByUserIssueIndex == correctIssueIndex),
                                                                                Answer = selectedByUserIssueIndex.ToString("0"),
                                                                                CorrectAnswer = correctIssueIndex.ToString("0"),
                                                                                Points = CurrentIssue.CorrectAnswerPoints
                                                                            });
                    }
                    break;
                case IssueTypes.SelectionInImage:
                    AppController.UserInfo.Answers[_currentSet].Add(new UserAnswer
                                                                        {
                                                                            IsCorrect = (isCorrect = UserAnswer == CorrectAnswer),
                                                                            Answer = UserAnswer.ToString("0"),
                                                                            CorrectAnswer = CorrectAnswer.ToString("0"),
                                                                            Points = CurrentIssue.CorrectAnswerPoints
                                                                        });
                    break;
                case IssueTypes.ImageSelection:
                    {
// ReSharper disable AssignNullToNotNullAttribute
                        var userSelection = Answers.Where(a => a.SelectedByUser).Select(a => a.OriginalImageFileName).ToList();
                        var correctSelection = CurrentIssue.Answers.Where(a => a.Selected).Select(a => a.ImageFileName).ToList();
// ReSharper restore AssignNullToNotNullAttribute
                        AppController.UserInfo.Answers[_currentSet].Add(new UserAnswer
                                                                            {
                                                                                IsCorrect =
                                                                                    (isCorrect =
                                                                                     userSelection.Count == correctSelection.Count &&
                                                                                     userSelection.All(correctSelection.Contains)),
                                                                                Answer = string.Empty,
                                                                                CorrectAnswer = string.Empty,
                                                                                Points = CurrentIssue.CorrectAnswerPoints
                                                                            });
                    }
                    break;
                case IssueTypes.Order:
                    AppController.UserInfo.Answers[_currentSet].Add(new UserAnswer
                                                                        {
// ReSharper disable AssignNullToNotNullAttribute
                                                                            IsCorrect = (isCorrect = !Answers.Where((answer, index) => answer.OrderNo != index).Any()),
// ReSharper restore AssignNullToNotNullAttribute
                                                                            Answer = string.Empty,
                                                                            CorrectAnswer = string.Empty,
                                                                            Points = CurrentIssue.CorrectAnswerPoints
                                                                        });
                    break;
                case IssueTypes.Placement:
                    isCorrect = PlacementCorrectAnswer1.Count == CurrentIssue.PlacementCorrectAnswer1.Count &&
                                !PlacementCorrectAnswer1.Where(
                                    (imageFileName, index) =>
                                    imageFileName !=
                                    Path.Combine(_currentDirectory, CurrentIssue.PlacementCorrectAnswer1[index])).Any();
                    isCorrect &= PlacementCorrectAnswer2.Count == CurrentIssue.PlacementCorrectAnswer2.Count &&
                                !PlacementCorrectAnswer2.Where(
                                    (imageFileName, index) =>
                                    imageFileName !=
                                    Path.Combine(_currentDirectory, CurrentIssue.PlacementCorrectAnswer2[index])).Any();
                    isCorrect &= NumberOfShelves < 3 ||
                                 (PlacementCorrectAnswer3.Count == CurrentIssue.PlacementCorrectAnswer3.Count &&
                                  !PlacementCorrectAnswer3.Where(
                                      (imageFileName, index) =>
                                      imageFileName !=
                                      Path.Combine(_currentDirectory, CurrentIssue.PlacementCorrectAnswer3[index])).Any());
                    isCorrect &= NumberOfShelves < 4 ||
                                 (PlacementCorrectAnswer4.Count == CurrentIssue.PlacementCorrectAnswer4.Count &&
                                  !PlacementCorrectAnswer4.Where(
                                      (imageFileName, index) =>
                                      imageFileName !=
                                      Path.Combine(_currentDirectory, CurrentIssue.PlacementCorrectAnswer4[index])).Any());
                    isCorrect &= NumberOfShelves < 5 ||
                                 (PlacementCorrectAnswer5.Count == CurrentIssue.PlacementCorrectAnswer5.Count &&
                                  !PlacementCorrectAnswer4.Where(
                                      (imageFileName, index) =>
                                      imageFileName !=
                                      Path.Combine(_currentDirectory, CurrentIssue.PlacementCorrectAnswer5[index])).Any());

                    AppController.UserInfo.Answers[_currentSet].Add(new UserAnswer
                                                                        {
                                                                            IsCorrect = isCorrect,
                                                                            Answer = string.Empty,
                                                                            CorrectAnswer = string.Empty,
                                                                            Points = CurrentIssue.CorrectAnswerPoints
                                                                        });
                    break;
            }

            if (isCorrect)
            {
                AppController.UserInfo.CorrectAnswers[_currentSet]++;
                AppController.UserInfo.CorrectAnswerPoints[_currentSet] += CurrentIssue.CorrectAnswerPoints;
            }
            else
                AppController.UserInfo.IncorrectAnswers[_currentSet]++;
            // Go to the either next issue or result page
            var issue = GetNextIssue();

            if (issue == null)
            {
                var page = o as Page;

                if (page != null && page.NavigationService != null)
                    page.NavigationService.Navigate(AppController.GetPage(ApplicationPages.ResultPage));
            }
            else
            {
                SetNewIssue(issue);
            }
        }
        #endregion

        #region Order issue commands
        private void MoveAnswerUp()
        {
            if (SelectedAnswerIndex <= 0) return;

            var index = SelectedAnswerIndex;
            var answer = Answers[index];

            Answers.RemoveAt(index);
            index--;
            Answers.Insert(index, answer);

            SelectedAnswerIndex = index;

            RaiseInterfaceUpdate();
        }

        private void MoveAnswerDown()
        {
            if (SelectedAnswerIndex >= OrderIssueAnswerNumber || SelectedAnswerIndex < 0) return;

            var index = SelectedAnswerIndex;
            var answer = Answers[index];

            Answers.RemoveAt(index);
            index++;
            Answers.Insert(index, answer);

            SelectedAnswerIndex = index;

            RaiseInterfaceUpdate();
        }
        #endregion

        #region Placement issue commands
        private void AddPlacementCorrectAnswer(object o)
        {
            var shelfNumber = o.ToString();
            var fullImageFileName = Answers[SelectedAnswerIndex].ImageFileName;

            switch (shelfNumber)
            {
                case "1":
                    PlacementCorrectAnswer1.Add(fullImageFileName);
                    break;
                case "2":
                    PlacementCorrectAnswer2.Add(fullImageFileName);
                    break;
                case "3":
                    PlacementCorrectAnswer3.Add(fullImageFileName);
                    break;
                case "4":
                    PlacementCorrectAnswer4.Add(fullImageFileName);
                    break;
                case "5":
                    PlacementCorrectAnswer5.Add(fullImageFileName);
                    break;
            }
        }

        private void DeletePlacementCorrectAnswer(object o)
        {
            var listView = o as ListView;

            if (listView == null || listView.SelectedIndex == -1) return;

            var visualPlacementCorrectAnswer = listView.ItemsSource as ObservableCollection<string>;

            if (visualPlacementCorrectAnswer == null) return;

            visualPlacementCorrectAnswer.RemoveAt(listView.SelectedIndex);
        }
        #endregion

        private IIssue GetNextIssue()
        {
            IIssue result = null;
            var issues = Issues.IssueDb.GetIssues(AppController.UserInfo.DistributionChannel, AppController.UserInfo.Region);
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
            var unansweredIssueNumber = issues.Count(i => !i.IsAnswered);

            if (unansweredIssueNumber > 0)
            {
                if (unansweredIssueNumber == 1 || !IsIssueSelected) return issues.First(i => !i.IsAnswered);

                var issueNumber = Issues.IssueDb.GetIssueNumber(AppController.UserInfo.DistributionChannel, AppController.UserInfo.Region);
                var currentIndex = issues.IndexOf(CurrentIssue);
                var newIssueIndex = (currentIndex == (issueNumber - 1)) ? 0 : currentIndex + 1;

                while (result == null && newIssueIndex != currentIndex)
                {
                    if (issues[newIssueIndex].IsAnswered)
                    {
                        newIssueIndex++;

                        if (newIssueIndex == issueNumber)
                            newIssueIndex = 0;
                    }
                    else
                        result = issues[newIssueIndex];
                }
            }
// ReSharper restore PossibleNullReferenceException
// ReSharper restore AssignNullToNotNullAttribute
            return result;
        }

        private void SetNewIssue(IIssue issue)
        {
// ReSharper disable PossibleNullReferenceException
            _currentSet = issue.Set;
// ReSharper restore PossibleNullReferenceException
            CurrentIssue = issue;
            CurrentIssueNumber = SetIssues.IndexOf(CurrentIssue);

            switch (IssueType)
            {
                case IssueTypes.Selection:
                    Answers[0].SelectedByUser = true;
                    break;
                case IssueTypes.SelectionInImage:
                    Answers[0].UserAnswer = 1u;
                    break;
            }

            RaiseInterfaceUpdate();
        }

        private void RaiseInterfaceUpdate()
        {
            RaisePropertyChanged("IsSet1ButtonEnabled");
            RaisePropertyChanged("IsSet2ButtonEnabled");
            RaisePropertyChanged("IsSet3ButtonEnabled");
            RaisePropertyChanged("IsSet4ButtonEnabled");
            RaisePropertyChanged("IsSet5ButtonEnabled");
            RaisePropertyChanged("IsSet6ButtonEnabled");
            RaisePropertyChanged("IsSet7ButtonEnabled");
            RaisePropertyChanged("IssueContent");
            RaisePropertyChanged("IssueType");
            RaisePropertyChanged("IsIssueAdditionalDetailsVisible");
            RaisePropertyChanged("DetailsColumnSpan");
        }
    }
}