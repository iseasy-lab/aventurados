using System;
using FMGames.Playfab.Login;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab.Login {
    public class EmailLogin : ILogin {
        public class EmailLoginParams {
            public string username;
            public string password;

            public EmailLoginParams(string username, string password) {
                this.username = username;
                this.password = password;
            }
        }
        
        public void Login(GetPlayerCombinedInfoRequestParams loginInfoParams, Action<LoginResult> loginSuccess, Action<PlayFabError> loginFailure, object loginParams) {
            EmailLoginParams emailLoginParams = loginParams as EmailLoginParams;
            if (emailLoginParams == null) {
                loginFailure.Invoke(new PlayFabError());
                Debug.LogError("Login Parameter is null");

                return;
            }
            
            var request = new LoginWithPlayFabRequest {
                TitleId = PlayFabConstants.TitleID,
                Username = emailLoginParams.username,
                Password = emailLoginParams.password,
                InfoRequestParameters = loginInfoParams,
            };

            PlayFabClientAPI.LoginWithPlayFab(request, loginSuccess, loginFailure);
        }
    }
}