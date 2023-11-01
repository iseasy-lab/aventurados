using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Login {
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
                InfoRequestParameters = loginInfoParams
                /*
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
                */
            };

            PlayFabClientAPI.LoginWithPlayFab(request, loginSuccess, loginFailure);
        }
    }
}