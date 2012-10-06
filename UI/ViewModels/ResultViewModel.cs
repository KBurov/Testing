using System;

using Testing.Common;

using Wanderer.Library.Common;

namespace Testing.UI.ViewModels
{
    public sealed class ResultViewModel : BindableObject
    {
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public string CurrentDate { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global
        #region UserInfo
        public string FullName { get { return string.Format("{0} {1} {2}", AppController.UserInfo.LastName, AppController.UserInfo.FirstName, AppController.UserInfo.MiddleName); } }

        public string Position { get { return AppController.UserInfo.Position; } }

        public bool IsPositionVisible { get { return Issues.IssueDb.UserInfoSettings.IsPositionVisible; } }

        public string Level { get { return AppController.UserInfo.Level; } }

        public bool IsLevelVisible { get { return Issues.IssueDb.UserInfoSettings.IsLevelVisible; } }

        public string Experience { get { return AppController.UserInfo.Experience; } }

        public bool IsExperienceVisible { get { return Issues.IssueDb.UserInfoSettings.IsExperienceVisible; } }
        #endregion

        #region Answers
        public uint Set1CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set1]; } }
        public uint Set2CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set2]; } }
        public uint Set3CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set3]; } }
        public uint Set4CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set4]; } }
        public uint Set5CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set5]; } }
        public uint Set6CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set6]; } }
        public uint Set7CorrectAnswers { get { return AppController.UserInfo.CorrectAnswers[IssueSets.Set7]; } }

        public uint Set1IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set1]; } }
        public uint Set2IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set2]; } }
        public uint Set3IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set3]; } }
        public uint Set4IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set4]; } }
        public uint Set5IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set5]; } }
        public uint Set6IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set6]; } }
        public uint Set7IncorrectAnswers { get { return AppController.UserInfo.IncorrectAnswers[IssueSets.Set7]; } }

        public uint Set1CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set1]; } }
        public uint Set2CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set2]; } }
        public uint Set3CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set3]; } }
        public uint Set4CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set4]; } }
        public uint Set5CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set5]; } }
        public uint Set6CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set6]; } }
        public uint Set7CorrectAnswerPoints { get { return AppController.UserInfo.CorrectAnswerPoints[IssueSets.Set7]; } }
        #endregion

        public ResultViewModel()
        {
            CurrentDate = DateTime.Today.ToShortDateString();
        }
    }
}
