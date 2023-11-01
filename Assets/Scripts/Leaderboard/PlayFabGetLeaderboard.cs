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
        private string displayName = null;

        
        public void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardRequest
                          {
                              StatisticName = leaderboardName,
                              StartPosition = startPosition,
                              MaxResultsCount = maxResultsCount,
                              //PlayFabId = PlayFabLogin.PlayFabId
                          };
            PlayFabClientAPI.GetLeaderboard(request,
                                            OnGetLeaderboardSuccess,
                                            OnGetLeaderboardFailure);
        }

        public void GetPlayerProfile(string playFabId)
        {
            var request = new GetPlayerProfileRequest()
            {
                PlayFabId = playFabId,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            };
            PlayFabClientAPI.GetPlayerProfile(request, OnDisplaySuccess, OnDisplayError);
                //result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName),
                //error => Debug.LogError(error.GenerateErrorReport()));
        }
        
        private void OnDisplaySuccess(GetPlayerProfileResult result)
        {
            Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName);
            displayName = result.PlayerProfile.DisplayName;
        }
        
        private void OnDisplayError(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
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
                GetPlayerProfile(playerLeaderboardEntry.PlayFabId);
                //displayName = UpdateDisplayName(playerLeaderboardEntry.PlayFabId).result.PlayerProfile.DisplayName;
                Debug.Log("dn " + displayName);
                leaderboard.AppendLine($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.PlayFabId} -- {playerLeaderboardEntry.DisplayName} --{playerLeaderboardEntry.StatValue}");
            }

            OnSuccess?.Invoke(leaderboard.ToString());
        }
    }
}
