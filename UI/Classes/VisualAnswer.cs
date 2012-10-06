using System;
using System.IO;

using Testing.Common;

using Wanderer.Library.Common;

namespace Testing.UI.Classes
{
    public class VisualAnswer : BindableObject
    {
        private readonly IAnswer _answer;
        private readonly string _currentDirectory;

        private bool _selectedByUser;
        private uint _userAnswer;

        internal VisualAnswer(IAnswer answer, string currentDirectory)
        {
            if (answer == null)
                throw new ArgumentNullException("answer");

            _answer = answer;
            _currentDirectory = string.IsNullOrEmpty(currentDirectory) ? string.Empty : currentDirectory;
        }

        #region Answer properties
        public uint OrderNo { get { return _answer.OrderNo; } }

        public string Content { get { return AppController.CurrentInterfaceLanguage == InterfaceLanguages.Ukraine ? _answer.ContentUA : _answer.ContentRU; } }
// ReSharper disable AssignNullToNotNullAttribute
        public string ImageFileName { get { return Path.Combine(_currentDirectory, _answer.ImageFileName); } }
// ReSharper restore AssignNullToNotNullAttribute
        public string OriginalImageFileName { get { return _answer.ImageFileName; } }

        public bool SelectedByUser
        {
            get { return _selectedByUser; }
            set
            {
                _selectedByUser = value;

                RaisePropertyChanged("SelectedByUser");
            }
        }

        public uint UserAnswer
        {
            get { return _userAnswer; }
            set
            {
                _userAnswer = value;

                RaisePropertyChanged("UserAnswer");
            }
        }
        #endregion
    }
}
