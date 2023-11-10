using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Leaderboard
{
    public class PlayFabGetLeaderboard
    {
        public event Action<string> OnSuccess;
        
        public void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardRequest
                          {
                              StatisticName = leaderboardName,
                              StartPosition = startPosition,
                              MaxResultsCount = maxResultsCount,
                          };
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
        }

        private void OnGetLeaderboardFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }

        private void OnGetLeaderboardSuccess(GetLeaderboardResult response)
        {
            var leaderboard = new StringBuilder();
            foreach (var playerLeaderboardEntry in response.Leaderboard)
            {
                leaderboard.AppendLine($"{playerLeaderboardEntry.Position + 1}.                         {playerLeaderboardEntry.DisplayName}                               {playerLeaderboardEntry.StatValue}");
            }

            OnSuccess?.Invoke(leaderboard.ToString());
        }
    }
}
