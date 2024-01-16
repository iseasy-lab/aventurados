using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Login {
    public class PlayfabLogin : MonoBehaviour {
        
        [SerializeField] private LoginUi loginUi;
        [SerializeField] private RegisterUi registerUi;
        [SerializeField] private GameObject loginInProgress;
        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject registerPanel;
        
        public void Start() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

            //Filling login credentials based on local saves
            loginUi.username.text = PlayerPrefs.GetString(PlayFabConstants.SavedUsername, "");

            if (PlayFabClientAPI.IsClientLoggedIn()) {
                Debug.LogWarning("Usuario logeado");
            }
        }

        public void Login(ILogin loginMethod, object loginParams) {
            loginMethod.Login(loginInfoParams, OnLoginSuccess, OnLoginFailure, loginParams);
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
                errorMessage = "El nombre de usuario debe tener almenos 5 caracteres";
                
            } else if (loginUi.password.text.Length < 8) {
                errorMessage = "La contraseña debe tener almenos 8 caracteres";
                
            }

            if (errorMessage.Length > 0) {
                Debug.Log(errorMessage);
                EditorUtility.DisplayDialog("Credenciales Inválidas", errorMessage, "Aceptar");
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
                InfoRequestParameters = loginInfoParams,
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnLoginFailure);

            loginInProgress.SetActive(true);
        }

        bool ValidateRegisterData() {
            //Validating data
            string errorMessage = "";

            if (!registerUi.email.text.Contains("@")) {
                errorMessage = "E-mail inválido";
                
            } else if (registerUi.email.text.Length < 5) {
                errorMessage = "El e-mail debe tener almenos 5 caracteres";
               
            } else if (registerUi.username.text.Length < 5) {
                errorMessage = "El nomber de usuario debe tener almenos 5 caracteres";
                
            } else if (registerUi.password.text.Length < 8) {
                errorMessage = "La contraseña debe tener almenos 8 caracteres";
                
            } else if (!registerUi.password.text.Equals(registerUi.verifyPassword.text)) {
                errorMessage = "Las contraseñas no coinciden";
                
            }

            if (errorMessage.Length > 0) {
                Debug.Log(errorMessage);
                EditorUtility.DisplayDialog("Credenciales Inválidas", errorMessage, "Aceptar");
                return false;
            }

            return true;
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
            Debug.Log("Register Success!");
            EditorUtility.DisplayDialog("Usuario Registrado", "Usuario registrado con éxito", "Aceptar");

            PlayerPrefs.SetString("USERNAME", registerUi.username.text);
            //PlayerPrefs.SetString("DISPLAYNAME", registerUi.username.text);
            PlayerPrefs.SetString("PW", registerUi.password.text);

            Debug.Log(result.PlayFabId);
            Debug.Log(result.Username);
            //Debug.Log(result.DisplayName);
            loginInProgress.SetActive(false);
            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
        }

        #endregion

        #region GuestLogin

        public void GuestLogin() {
            Login(new GuestLogin(), new GuestLogin.GuestLoginParameters(PlayerPrefs.GetString("GUEST_ID")));
        }
        
        #endregion

        private void OnLoginSuccess(LoginResult result) {
            Debug.Log("Login Success!");
            Debug.Log("identificador playfab: " + result.PlayFabId);
            
            GetPlayerProfile(result.PlayFabId);
            Debug.Log("nombre usuario playfab: " + result.InfoResultPayload.PlayerProfile.DisplayName);
            PlayFabConstants.displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
            
            PlayerPrefs.SetString(PlayFabConstants.SavedUsername, loginUi.username.text);
            PlayerPrefs.SetString("displayName", result.InfoResultPayload.PlayerProfile.DisplayName);
            loginInProgress.SetActive(false);

            SceneManager.LoadScene(3);

        }

        private readonly GetPlayerCombinedInfoRequestParams loginInfoParams =
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
            Debug.Log("Login failure: " + error.Error + "  " + error.ErrorDetails + error + "  " +
                           error.ApiEndpoint + "  " + error.ErrorMessage);
            EditorUtility.DisplayDialog("Error", "Usuario o contraseña invalidos", "Ok");
            loginInProgress.SetActive(false);
            Debug.Log(error.GenerateErrorReport());
        }
        
        public void GetPlayerProfile(string playFabId)
        {
            var request = new GetAccountInfoRequest()
            {
                PlayFabId = playFabId,
            };
            PlayFabClientAPI.GetAccountInfo(request, OnDisplaySuccess, OnDisplayError);
            
        }
        
        private void OnDisplaySuccess(GetAccountInfoResult result)
        {
            Debug.Log("The player's Username profile is: " + result.AccountInfo.Username);
            if (result.AccountInfo.Username != null)
            {
                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                    {
                        DisplayName = result.AccountInfo.Username,
                    },
                    result =>
                    {
                        Debug.Log("The player's display name is now: " + result.DisplayName);
                        Debug.Log("The player's display name in PlatConstants: " + PlayFabConstants.displayName);
                    },
                  error => Debug.Log(error.GenerateErrorReport()));
            }
            else
            {
                System.Random rnd = new System.Random();
                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                    {
                        //DisplayName = "Jugador " + Random.Range(0, 1000).ToString(),
                        
                        DisplayName = "Jugador Invitado",
                    }, 
                    result => Debug.Log("The player's display name is now: " + result.DisplayName),
                    error => Debug.Log(error.GenerateErrorReport()));
            }
            
        }
        
        private void OnDisplayError(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }
}