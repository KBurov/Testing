using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Ionic.Zip;
using Ionic.Zlib;

[assembly: InternalsVisibleTo("Testing.Admin.UI")]

namespace Testing.Common
{
    public static class IssueDbAssistant
    {
        public const string ImageFolder = "images";

        private const string DbFileName = "IssueDb.xml";
        private const string ZipFileName = "IssueDb.zip";
        private const string ZipPassword = "123qweASD";

        public static IIssueDb LoadIssueDb()
        {
            var result = new IssueDb();

            if (!Directory.Exists(ImageFolder))
                Directory.CreateDirectory(ImageFolder);

            using (var ds = new IssuesDescriptionDS())
            {
                if (File.Exists(ZipFileName))
                {
                    using (var zip = ZipFile.Read(ZipFileName))
                    {
                        var zipEntry = zip[DbFileName];
                        // Read db
                        using (var stream = zipEntry.OpenReader(ZipPassword))
                            ds.ReadXml(stream, XmlReadMode.IgnoreSchema);
                        // Extract all images
                        foreach (var entry in zip)
                        {
                            if (entry.FileName != DbFileName)
                                entry.ExtractWithPassword(ExtractExistingFileAction.OverwriteSilently, ZipPassword);
                        }
                    }
                    // UserInfo settings
                    if (ds.UserInfoSettings.Rows.Count > 0)
                    {
                        var row = ds.UserInfoSettings.Rows[0] as IssuesDescriptionDS.UserInfoSettingsRow;
                        var userInfoSettings = result.UserInfoSettings as UserInfoSettings;

                        if (row != null && userInfoSettings != null)
                        {
                            userInfoSettings.IsPositionVisible = row.IsPositionVisible;
                            userInfoSettings.IsLevelVisible = row.IsLevelVisible;
                            userInfoSettings.IsExperienceVisible = row.IsExperienceVisible;
                        }
                    }
                    // Issues settings
                    if (ds.IssuesSettings.Rows.Count > 0)
                    {
                        var row = ds.IssuesSettings.Rows[0] as IssuesDescriptionDS.IssuesSettingsRow;
                        var issuesSettings = result.IssuesSettings as IssuesSettings;

                        if (row != null && issuesSettings != null)
                        {
                            issuesSettings.IsTimeLimited = row.IsTimeLimited;
                            issuesSettings.TimeLimit = row.TimeLimit;
                        }
                    }
                    // Issues
                    if (ds.Issues.Rows.Count > 0)
                    {
                        foreach (var r in ds.Issues.Rows)
                        {
                            var row = r as IssuesDescriptionDS.IssuesRow;

                            if (row != null)
                            {
                                var issue = new Issue
                                                {
                                                    DistributionChannel = (DistributionChannels) row.DistributionChannelID,
                                                    Region = (Regions) row.RegionID,
                                                    Type = (IssueTypes) row.TypeID,
                                                    Set = (IssueSets) row.SetID,
                                                    ContentUA = row.ContentUA,
                                                    ContentRU = row.ContentRU,
                                                    CorrectAnswerPoints = row.CorrectAnswerPoints,
                                                    NumberOfShelves = row.NumberOfShelves,
                                                    PlacesOnShelf = row.PlacesOnShelf
                                                };
                                // Answers
                                var answerRows = row.GetChildRows("Issues_Answers");

                                if (answerRows.Length > 0)
                                    foreach (var a in answerRows)
                                    {
                                        var answerRow = a as IssuesDescriptionDS.AnswersRow;

                                        if (answerRow != null)
                                            issue.Answers.Add(new Answer
                                                                  {
                                                                      IsCorrect = answerRow.IsCorrect,
                                                                      OrderNo = answerRow.OrderNo,
                                                                      ContentUA = answerRow.ContentUA,
                                                                      ContentRU = answerRow.ContentRU,
                                                                      Selected = answerRow.Selected,
                                                                      ImageFileName = answerRow.ImageFileName
                                                                  });

                                        if (issue.Type != IssueTypes.Placement) continue;
// ReSharper disable PossibleNullReferenceException
                                        var placementCorrectAnswerRows = answerRow.GetChildRows("Answers_PlacementCorrectAnswers");
// ReSharper restore PossibleNullReferenceException
                                        if (placementCorrectAnswerRows.Length > 0)
                                            foreach (var p in placementCorrectAnswerRows)
                                            {
                                                var placementCorrectAnswerRow =
                                                    p as IssuesDescriptionDS.PlacementCorrectAnswersRow;

                                                if (placementCorrectAnswerRow != null)
                                                    FillPlacementCorrectAnswer(issue,
                                                                               answerRow.ImageFileName,
                                                                               placementCorrectAnswerRow.ShelfNumber,
                                                                               placementCorrectAnswerRow.Position);
                                            }
                                    }

                                result.Issues.Add(issue);
                            }
                        }
                    }
                }
            }

            return result;
        }

        internal static void SaveIssueDb(IIssueDb issueDb)
        {
            if (issueDb == null)
                throw new ArgumentNullException("issueDb");

            using (var ds = new IssuesDescriptionDS())
            {
                // Fill dataset
                // UserInfo settings
                ds.UserInfoSettings.AddUserInfoSettingsRow(IsPositionVisible: issueDb.UserInfoSettings.IsPositionVisible,
                                                           IsLevelVisible: issueDb.UserInfoSettings.IsLevelVisible,
                                                           IsExperienceVisible: issueDb.UserInfoSettings.IsExperienceVisible);
                // Issues settings
                ds.IssuesSettings.AddIssuesSettingsRow(issueDb.IssuesSettings.IsTimeLimited, issueDb.IssuesSettings.TimeLimit);
                // Issues
                foreach (var issue in issueDb.Issues)
                {
                    var row = ds.Issues.AddIssuesRow(DistributionChannelID: (int) issue.DistributionChannel,
                                                     RegionID: (int) issue.Region,
                                                     TypeID: (int) issue.Type,
                                                     SetID: (int) issue.Set,
                                                     ContentUA: string.IsNullOrEmpty(issue.ContentUA) ? string.Empty : issue.ContentUA,
                                                     ContentRU: string.IsNullOrEmpty(issue.ContentRU) ? string.Empty : issue.ContentRU,
                                                     CorrectAnswerPoints: issue.CorrectAnswerPoints,
                                                     NumberOfShelves: issue.NumberOfShelves,
                                                     PlacesOnShelf: issue.PlacesOnShelf);
                    if (row != null)
                    {
                        // Answers
                        foreach (var answer in issue.Answers)
                        {
                            var answerRow = ds.Answers.AddAnswersRow(row,
                                                                     answer.IsCorrect,
                                                                     answer.OrderNo,
                                                                     string.IsNullOrEmpty(answer.ContentUA)
                                                                         ? string.Empty
                                                                         : answer.ContentUA,
                                                                     string.IsNullOrEmpty(answer.ContentRU)
                                                                         ? string.Empty
                                                                         : answer.ContentRU,
                                                                     answer.Selected,
                                                                     string.IsNullOrEmpty(answer.ImageFileName) ? string.Empty : answer.ImageFileName);

                            if (issue.Type != IssueTypes.Placement) continue;
// ReSharper disable AccessToForEachVariableInClosure
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable AccessToModifiedClosure
                            var placementCorrectAnswer1 = issue.PlacementCorrectAnswer1
                                .Select((s, index) => new {index, s})
                                .Where(a => a.s == answer.ImageFileName)
                                .Select(a => a.index);
                            var placementCorrectAnswer2 = issue.PlacementCorrectAnswer2
                                .Select((s, index) => new {index, s})
                                .Where(a => a.s == answer.ImageFileName)
                                .Select(a => a.index);
                            var placementCorrectAnswer3 = issue.PlacementCorrectAnswer3
                                .Select((s, index) => new {index, s})
                                .Where(a => a.s == answer.ImageFileName)
                                .Select(a => a.index);
                            var placementCorrectAnswer4 = issue.PlacementCorrectAnswer4
                                .Select((s, index) => new {index, s})
                                .Where(a => a.s == answer.ImageFileName)
                                .Select(a => a.index);
                            var placementCorrectAnswer5 = issue.PlacementCorrectAnswer5
                                .Select((s, index) => new {index, s})
                                .Where(a => a.s == answer.ImageFileName)
                                .Select(a => a.index);
// ReSharper restore AccessToModifiedClosure
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper restore AccessToForEachVariableInClosure
                            foreach (var i in placementCorrectAnswer1)
                                ds.PlacementCorrectAnswers.AddPlacementCorrectAnswersRow(answerRow, 1, i);
                            foreach (var i in placementCorrectAnswer2)
                                ds.PlacementCorrectAnswers.AddPlacementCorrectAnswersRow(answerRow, 2, i);
                            foreach (var i in placementCorrectAnswer3)
                                ds.PlacementCorrectAnswers.AddPlacementCorrectAnswersRow(answerRow, 3, i);
                            foreach (var i in placementCorrectAnswer4)
                                ds.PlacementCorrectAnswers.AddPlacementCorrectAnswersRow(answerRow, 4, i);
                            foreach (var i in placementCorrectAnswer5)
                                ds.PlacementCorrectAnswers.AddPlacementCorrectAnswersRow(answerRow, 5, i);
                        }
                    }
                }

                ds.AcceptChanges();
                // Save IssueDb
                ds.WriteXml(DbFileName);
                // Save to zip file
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.BestCompression;
                    zip.Password = ZipPassword;
                    zip.AddFile(DbFileName);

                    if (!Directory.Exists(ImageFolder))
                        Directory.CreateDirectory(ImageFolder);

                    zip.AddDirectory(ImageFolder, ImageFolder);
                    zip.Save(ZipFileName);
                }
                // Delete xml file
                File.Delete(DbFileName);
            }
        }

        private static void FillPlacementCorrectAnswer(IIssue issue, string imageFileName, int shelfNumber, int position)
        {
            IList<string> shelf = null;

            switch (shelfNumber)
            {
                case 1:
                    shelf = issue.PlacementCorrectAnswer1;
                    break;
                case 2:
                    shelf = issue.PlacementCorrectAnswer2;
                    break;
                case 3:
                    shelf = issue.PlacementCorrectAnswer3;
                    break;
                case 4:
                    shelf = issue.PlacementCorrectAnswer4;
                    break;
                case 5:
                    shelf = issue.PlacementCorrectAnswer5;
                    break;
            }

            if (shelf == null) return;

            if (shelf.Count <= position)
                for (var i = shelf.Count;i <= position;i++)
                    shelf.Add(string.Empty);

            shelf[position] = imageFileName;
        }
    }
}
