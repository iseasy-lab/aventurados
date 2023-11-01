using Login;

using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Leaderboard
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Button _getLeaderboardButton;
        //[SerializeField] private Button _getLeaderboardAroundPlayerButton;
        [SerializeField] private Button _getPlayerScoreButton;
        [SerializeField] private Button _regresarButton;
        [SerializeField] private TextMeshProUGUI _resultsText;
       
        private const string LeaderboardName = "Leaderboard";

        private string _playerId;
        private PlayfabLogin pfLogin = new PlayfabLogin();
        
        //private Login.PlayfabLogin _playFabLogin;
        //private PlayFabUpdatePlayerStatistics _playFabUpdatePlayerStatistics;
        private PlayFabGetLeaderboardAroundPlayer _playFabGetLeaderboardAroundPlayer;
        private PlayFabGetLeaderboard _playFabGetLeaderboard;

        private void Start()
        {
            AddListeners();
            CreatePlayFabServices();
            
            if (PlayFabClientAPI.IsClientLoggedIn()) {
                Debug.Log("Usuario logeado");
            }
            //PlayFabClientAPI.GetUserData(request);
        }
        
        
        private void CreatePlayFabServices()
        {
            var request = new GetUserDataRequest
            {
                PlayFabId = _playerId,
            };
            PlayFabClientAPI.GetUserData(request, OnGetUserDataSuccess, OnGetUserDataFailure);
            Debug.Log("Request: " + request);
            
            pfLogin.ReturnPlayFabId(_playerId);

            //_playFabUpdatePlayerStatistics = new PlayFabUpdatePlayerStatistics();

            _playFabGetLeaderboardAroundPlayer = new PlayFabGetLeaderboardAroundPlayer();
            _playFabGetLeaderboardAroundPlayer.OnSuccess += result => _resultsText.SetText(result);

            _playFabGetLeaderboard = new PlayFabGetLeaderboard();
            _playFabGetLeaderboard.OnSuccess += result => _resultsText.SetText(result);
        }
        private void AddListeners()
        {
            _getLeaderboardButton.onClick.AddListener(OnGetLeaderboardButtonPressed);
            //_getLeaderboardAroundPlayerButton.onClick.AddListener(OnGetLeaderboardAroundPlayerButtonPressed);
            _getPlayerScoreButton.onClick.AddListener(OnGetPlayerScoreButtonPressed);
            _regresarButton.onClick.AddListener(OnRegresarButtonPressed);
            
        }
        
        private void OnGetLeaderboardButtonPressed()
        {
            Debug.Log("Generando leaderboard");
            _playFabGetLeaderboard.GetLeaderboardEntries(0, 10, LeaderboardName);
        }
        private void OnGetPlayerScoreButtonPressed()
        {
            Debug.Log("Generando score del jugador: " + _playerId);
            //_playFabGetLeaderboardAroundPlayer.UpdateDisplayName(_playerId);
            _playFabGetLeaderboardAroundPlayer.GetLeaderboardAroundPlayer(_playerId, 1, LeaderboardName);
        }
        private void OnRegresarButtonPressed()
        {
            Debug.Log("Regresando al menu principal");
            SceneManager.LoadScene(1);
        }

        

        //private void OnGetLeaderboardAroundPlayerButtonPressed()
        //{
        //    _playFabGetLeaderboardAroundPlayer
        //       .GetLeaderboardAroundPlayer(_playerId, 3, LeaderboardName);
        //}

        private void OnGetUserDataSuccess(GetUserDataResult result)
        {
            Debug.Log("Id: " + _playerId);
            }
        private void OnGetUserDataFailure(PlayFabError error)
        {
            Debug.Log("Error al obtener datos del jugador");
        }
    }
        
    
}
