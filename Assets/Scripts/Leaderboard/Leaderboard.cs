using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Leaderboard
{
    public class Main : MonoBehaviour
    {
        [FormerlySerializedAs("_getLeaderboardButton")] [SerializeField] private Button getLeaderboardButton;
        [FormerlySerializedAs("_getPlayerScoreButton")] [SerializeField] private Button getPlayerScoreButton;
        [FormerlySerializedAs("_regresarButton")] [SerializeField] private Button regresarButton;
        [FormerlySerializedAs("_resultsText")] [SerializeField] private TextMeshProUGUI resultsText;
       
        private const string LeaderboardName = "Leaderboard";

        private string playerId;
        
       
        private PlayFabGetLeaderboardAroundPlayer playFabGetLeaderboardAroundPlayer;
        private PlayFabGetLeaderboard playFabGetLeaderboard;

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
            playFabGetLeaderboardAroundPlayer = new PlayFabGetLeaderboardAroundPlayer();
            playFabGetLeaderboardAroundPlayer.OnSuccess += result => resultsText.SetText(result);

            playFabGetLeaderboard = new PlayFabGetLeaderboard();
            playFabGetLeaderboard.OnSuccess += result => resultsText.SetText(result);
        }
        private void AddListeners()
        {
            getLeaderboardButton.onClick.AddListener(OnGetLeaderboardButtonPressed);
            getPlayerScoreButton.onClick.AddListener(OnGetPlayerScoreButtonPressed);
            regresarButton.onClick.AddListener(OnRegresarButtonPressed);
        }
        
        private void OnGetLeaderboardButtonPressed()
        {
            Debug.Log("Generando leaderboard");
            playFabGetLeaderboard.GetLeaderboardEntries(0, 10, LeaderboardName);
        }
        private void OnGetPlayerScoreButtonPressed()
        {
            Debug.Log("Generando score del jugador: " + playerId);
            playFabGetLeaderboardAroundPlayer.GetLeaderboardAroundPlayer(playerId, 1, LeaderboardName);
        }
        private void OnRegresarButtonPressed()
        {
            Debug.Log("Regresando al menu principal");
            SceneManager.LoadScene(1);
        }
        
    }
        
    
}
