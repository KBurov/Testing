using System.Collections.Generic;
using Testing.Common;

namespace Testing.UI
{
    internal sealed class UserInfo
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Position { get; set; }

        public string Level { get; set; }

        public string Experience { get; set; }

        public DistributionChannels DistributionChannel { get; set; }

        public Regions Region { get; set; }

        public IDictionary<IssueSets, IList<UserAnswer>> Answers { get; private set; }

        public IDictionary<IssueSets, uint> CorrectAnswers { get; private set; }

        public IDictionary<IssueSets, uint> IncorrectAnswers { get; private set; }

        public IDictionary<IssueSets, uint> CorrectAnswerPoints { get; private set; }

        public UserInfo()
        {
            Answers = new Dictionary<IssueSets, IList<UserAnswer>>
                          {
                              {IssueSets.Set1, new List<UserAnswer>()},
                              {IssueSets.Set2, new List<UserAnswer>()},
                              {IssueSets.Set3, new List<UserAnswer>()},
                              {IssueSets.Set4, new List<UserAnswer>()},
                              {IssueSets.Set5, new List<UserAnswer>()},
                              {IssueSets.Set6, new List<UserAnswer>()},
                              {IssueSets.Set7, new List<UserAnswer>()}
                          };
            CorrectAnswers = new Dictionary<IssueSets, uint>
                                 {
                                     {IssueSets.Set1, 0},
                                     {IssueSets.Set2, 0},
                                     {IssueSets.Set3, 0},
                                     {IssueSets.Set4, 0},
                                     {IssueSets.Set5, 0},
                                     {IssueSets.Set6, 0},
                                     {IssueSets.Set7, 0},
                                 };
            IncorrectAnswers = new Dictionary<IssueSets, uint>
                                   {
                                       {IssueSets.Set1, 0},
                                       {IssueSets.Set2, 0},
                                       {IssueSets.Set3, 0},
                                       {IssueSets.Set4, 0},
                                       {IssueSets.Set5, 0},
                                       {IssueSets.Set6, 0},
                                       {IssueSets.Set7, 0},
                                   };
            CorrectAnswerPoints = new Dictionary<IssueSets, uint>
                                      {
                                          {IssueSets.Set1, 0},
                                          {IssueSets.Set2, 0},
                                          {IssueSets.Set3, 0},
                                          {IssueSets.Set4, 0},
                                          {IssueSets.Set5, 0},
                                          {IssueSets.Set6, 0},
                                          {IssueSets.Set7, 0},
                                      };
        }
    }
}