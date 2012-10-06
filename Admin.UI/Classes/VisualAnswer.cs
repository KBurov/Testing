using System;
using System.IO;

using Testing.Common;
using Wanderer.Library.Common;

namespace Testing.Admin.UI.Classes
{
    public class VisualAnswer : BindableObject
    {
        private readonly Answer _answer;
        private readonly string _currentDirectory;

        private InterfaceLanguages _issueLanguage;

        internal VisualAnswer(Answer answer, string currentDirectory)
        {
            if (answer == null)
                throw new ArgumentNullException("answer");

            _answer = answer;
            _currentDirectory = string.IsNullOrEmpty(currentDirectory) ? string.Empty : currentDirectory;
        }

        public InterfaceLanguages IssueLanguage
        {
            get { return _issueLanguage; }
            set
            {
                _issueLanguage = value;

                RaisePropertyChanged("IssueLanguage");
                RaisePropertyChanged("Content");
            }
        }

        #region Answer properties
        public bool IsCorrect
        {
            get { return _answer.IsCorrect; }
            set
            {
                _answer.IsCorrect = value;

                RaisePropertyChanged("IsCorrect");
            }
        }

        public uint OrderNo
        {
            get { return _answer.OrderNo; }
            set
            {
                _answer.OrderNo = value;

                RaisePropertyChanged("OrderNo");
            }
        }

        public string Content
        {
            get { return IssueLanguage == InterfaceLanguages.Ukraine ? _answer.ContentUA : _answer.ContentRU; }
            set
            {
                if (IssueLanguage == InterfaceLanguages.Ukraine)
                    _answer.ContentUA = value;
                else
                    _answer.ContentRU = value;

                RaisePropertyChanged("Content");
            }
        }

        public string ContentUA
        {
            get { return _answer.ContentUA; }
            set
            {
                _answer.ContentUA = string.IsNullOrEmpty(value) ? string.Empty : value;

                RaisePropertyChanged("Content");
            }
        }

        public string ContentRU
        {
            get { return _answer.ContentRU; }
            set
            {
                _answer.ContentRU = string.IsNullOrEmpty(value) ? string.Empty : value;

                RaisePropertyChanged("Content");
            }
        }

        public bool Selected
        {
            get { return _answer.Selected; }
            set
            {
                _answer.Selected = value;

                RaisePropertyChanged("Selected");
            }
        }
// ReSharper disable AssignNullToNotNullAttribute
        public string ImageFileName { get { return Path.Combine(_currentDirectory, _answer.ImageFileName); } }
// ReSharper restore AssignNullToNotNullAttribute
        public string OriginalImageFileName { get { return _answer.ImageFileName; } }
        #endregion
    }
}