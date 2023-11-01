using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Leaderboard
{
    public class PlayFabGetLeaderboardAroundPlayer
    {
        public event Action<string> OnSuccess;
        public string displayName = null;
        public string pubid; 
        public void GetLeaderboardAroundPlayer(string playerId, int maxResultsCount, string leaderboardName)
        {
            pubid = playerId;
            var request = new GetLeaderboardAroundPlayerRequest
                          {
                              PlayFabId = playerId,
                              MaxResultsCount = maxResultsCount,
                              StatisticName = leaderboardName
                          };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, GetLeaderboardAroundPlayerFailure);
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
            displayName = result.PlayerProfile.DisplayName;
            Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName);
            //displayName = result.PlayerProfile.DisplayName;
        }
        
        private void OnDisplayError(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        private void GetLeaderboardAroundPlayerFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
            
        }

        private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult response)
        {
            var leaderboard = new StringBuilder();
            
            Debug.Log("displayName antes de llamada la funcion Get" + displayName);
            foreach (var playerLeaderboardEntry in response.Leaderboard)
            {
                GetPlayerProfile(pubid);
                Debug.Log("displayName despues de llamada la funcion Get: " + displayName + "pubid: " + pubid);
                leaderboard.AppendLine($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.PlayFabId} - {displayName} - {playerLeaderboardEntry.StatValue}");
            }

            OnSuccess?.Invoke(leaderboard.ToString());
        }
    }
}
