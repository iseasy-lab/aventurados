using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace Login {
    /// <summary>
    /// Handle login scene events
    /// </summary>
    public class PlayfabLogin : MonoBehaviour {
        
        public string returnedPlayFabId;
        [SerializeField] private LoginUi loginUi;
        [SerializeField] private RegisterUi registerUi;
        [SerializeField] private GameObject loginInProgress;
        
        //public event Action<string> OnSuccess;

        public void Start() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

            //Filling login credentials based on local saves
            loginUi.username.text = PlayerPrefs.GetString(PlayFabConstants.SavedUsername, "");

            if (PlayFabClientAPI.IsClientLoggedIn()) {
                Debug.LogWarning("User is already logged in");
            }
        }

        public void Login(ILogin loginMethod, object loginParams) {
            loginMethod.Login(_loginInfoParams, OnLoginSuccess, OnLoginFailure, loginParams);

            loginInProgress.SetActive(true);
        }

        #region Email Login

        public void LoginWithEmail() {
            if (ValidateLoginData()) {
                Login(new EmailLogin(), new EmailLogin.EmailLoginParams(loginUi.username.text, loginUi.password.text));
            }
        }

        private bool ValidateLoginData() {
            //Validating data
            string errorMessage = "";

            if (loginUi.username.text.Length < 5) {
                errorMessage = "Username must be at least 5 characters";
            } else if (loginUi.password.text.Length < 8) {
                errorMessage = "Password must be at least 8 characters";
            }

            if (errorMessage.Length > 0) {
                Debug.LogError(errorMessage);
                return false;
            }

            return true;
        }

        #endregion

        #region Email Register

        public void Register() {
            if (!ValidateRegisterData()) return;

            var request = new RegisterPlayFabUserRequest {
                TitleId = PlayFabConstants.TitleID,
                Email = registerUi.email.text,
                Password = registerUi.password.text,
                Username = registerUi.username.text,
                InfoRequestParameters = _loginInfoParams,
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnLoginFailure);

            loginInProgress.SetActive(true);
        }

        bool ValidateRegisterData() {
            //Validating data
            string errorMessage = "";

            if (!registerUi.email.text.Contains("@")) {
                errorMessage = "E-mail invalido";
            } else if (registerUi.email.text.Length < 5) {
                errorMessage = "E-mail invalido";
            } else if (registerUi.username.text.Length < 5) {
                errorMessage = "Username debe tener almenos 5 caracteres";
            } else if (registerUi.password.text.Length < 8) {
                errorMessage = "Password debe tener almenos 8 caracteres";
            } else if (!registerUi.password.text.Equals(registerUi.verifyPassword.text)) {
                errorMessage = "Las contraseñas no coinciden";
            }

            if (errorMessage.Length > 0) {
                Debug.LogError(errorMessage);
                return false;
            }

            return true;
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
            Debug.Log("Register Success!");

            PlayerPrefs.SetString("USERNAME", registerUi.username.text);
            PlayerPrefs.SetString("PW", registerUi.password.text);

            Debug.Log(result.PlayFabId);
            Debug.Log(result.Username);
            loginInProgress.SetActive(false);
        }

        #endregion

        #region GuestLogin

        public void GuestLogin() {
            Login(new GuestLogin(), new GuestLogin.GuestLoginParameters(PlayerPrefs.GetString("GUEST_ID")));
        }
        
        #endregion

        private void OnLoginSuccess(LoginResult result) {
            Debug.Log("Login Success!");
            //OnSuccess?.Invoke(result.PlayFabId);
            Debug.Log(result.PlayFabId);
            returnedPlayFabId = result.PlayFabId;

            PlayerPrefs.SetString(PlayFabConstants.SavedUsername, loginUi.username.text);

            loginInProgress.SetActive(false);

            SceneManager.LoadScene(1);
            //Presentar Nombre de Usuario

            string name = null;
            if (result.InfoResultPayload.PlayerProfile != null)
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        public string ReturnPlayFabId(string playFabId)
        {
            Debug.Log("id returned= "+returnedPlayFabId);
            playFabId = returnedPlayFabId;
            return playFabId;
        }

        private readonly GetPlayerCombinedInfoRequestParams _loginInfoParams =
            new GetPlayerCombinedInfoRequestParams {
                GetUserAccountInfo = true,
                GetUserData = true,
                GetUserInventory = true,
                GetUserVirtualCurrency = true,
                GetUserReadOnlyData = true,
                GetPlayerProfile = true,
                GetPlayerStatistics = true,
                
            };

        private void OnLoginFailure(PlayFabError error) {
            Debug.LogError("Login failure: " + error.Error + "  " + error.ErrorDetails + error + "  " +
                           error.ApiEndpoint + "  " + error.ErrorMessage);
            loginInProgress.SetActive(false);
            Debug.LogError(error.GenerateErrorReport());
        }
        
        void OnSuccess(LoginResult result)
        {
            Debug.Log("Login Success!");
        }
        
        void OnError(PlayFabError error)
        {
            Debug.Log("Error while logging");
        }

        public void SendLeaderboard(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "Leaderboard",
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        }
        
        void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Leaderboard updated");
        }

        public void Exit() {
            Application.Quit();
        }
    }
}