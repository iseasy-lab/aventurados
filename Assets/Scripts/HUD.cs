using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    //Pantalla de carga previa al nivel del juego
    [SerializeField] private GameObject loadInProgress;
    
    //Progressbar del tiempo
    [SerializeField] private Slider progressBar;
    
    private int currentScene;
    
    private float currentTime = -1;

    [FormerlySerializedAs("Puntos")] public TextMeshProUGUI puntos;
    
    //Impresion del nombre de usuario
    [SerializeField] private TextMeshProUGUI displayNamePlayer;
    
    //Seleccion de personajes
    private int index;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI name;
    
    
    //Seleccion de escenas
    private int indexScene;
    [SerializeField] private Image imageScene;
    [SerializeField] private TextMeshProUGUI nameScene;

    //Menu de reporte
    [SerializeField] private GameObject menuReport;
    [SerializeField] private TextMeshProUGUI stepCounter;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        displayNamePlayer.text = "Bienvenido: "+ PlayerPrefs.GetString("displayName", "");
        //Debug.Log("nombre del displayname es: " + PlayerPrefs.GetString(PlayFabConstants.SavedUsername, ""));
        
        //ProgressBar
        
        currentScene = SceneManager.GetActiveScene().buildIndex;
        
        //Seleccion de personaje
        index = PlayerPrefs.GetInt("IndexPlayer");
        
        if(index > gameManager.charactersList.Count - 1)
        {
            index = 0;
        }
        
        //Seleccion de escena
        indexScene = PlayerPrefs.GetInt("IndexScene");
        
        if(indexScene > gameManager.scenesList.Count - 1)
        {
            indexScene = 0;
        }
        
        ChangeScene();

        if (currentScene == 6)
        {
            currentTime = Options.GlobalVar.currentTime;
            progressBar.maxValue = currentTime;
        }
    }

    void Update()
    {
        puntos.text = gameManager.PuntosTotales.ToString();
        
        // Resta el tiempo transcurrido desde el último frame
        currentTime -= Time.deltaTime;
        //Debug.Log("tiempo del nivel: " + currentTime);
        progressBar.value = currentTime;
        //gameManager.SumarPuntos(1);
        // Si se cumple el tiempo deseado, llama al método GameOver
        if (currentTime < 0.5f && currentTime > -0.5f)
        {
            gameManager.GameOver();
            Time.timeScale = 0f;
            menuReport.SetActive(true);
            stepCounter.text = Reports.Reports.StepCounter.ToString();
        }
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space))
        {
            Pause();
        }
    }

    public void ActualizarPuntos(int puntosTotales)
    {
        puntos.text = puntosTotales.ToString();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        Time.timeScale = 1f;
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    
    public void Quit()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1f;
    }
    
    private void ChangeScene()
    {
        //Seleccion de personajes
        PlayerPrefs.SetInt("IndexPlayer", index);
        image.sprite = gameManager.charactersList[index].image;
        name.text = gameManager.charactersList[index].name;
        
        //Seleccion de escenas
        PlayerPrefs.SetInt("IndexScene", indexScene);
        imageScene.sprite = gameManager.scenesList[indexScene].imageScene;
        nameScene.text = gameManager.scenesList[indexScene].nameScene;
    }
    
    #region ButtonCharacter
    
    public void NextCharacter()
    {
       
        if (index == gameManager.charactersList.Count - 1)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }
        ChangeScene();
    }
    
    public void PrevCharacter()
    {
       
        if (index == 0)
        {
            index = gameManager.charactersList.Count - 1;
        }
        else
        {
            index -= 1;
        }
        ChangeScene();
    }
    #endregion
    
    #region ButtonScene
    public void NextScene()
    {
       
        if (indexScene == gameManager.scenesList.Count - 1)
        {
            indexScene = 0;
        }
        else
        {
            indexScene += 1;
        }
        ChangeScene();
    }
    
    public void PrevScene()
    {
       
        if (indexScene == 0)
        {
            indexScene = gameManager.scenesList.Count - 1;
        }
        else
        {
            indexScene -= 1;
        }
        ChangeScene();
    }
    #endregion

    public void NextLevel()
    {
        Resume();
        int indexNextLevel = indexScene + 1;
        
        if(indexNextLevel > gameManager.scenesList.Count - 1)
        {
            indexNextLevel = 0;
        }
        
        
        //Seleccion de escenas
        PlayerPrefs.SetInt("IndexScene", indexNextLevel);
        imageScene.sprite = gameManager.scenesList[indexNextLevel].imageScene;
        nameScene.text = gameManager.scenesList[indexNextLevel].nameScene;
        
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void PlayLevel()
    {
       //AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Cargar escena del nivel del juego
        StartCoroutine(LoadAsync());
        loadInProgress.SetActive(true);
    }

    IEnumerator LoadAsync()
    {
        loadInProgress.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        while(!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / .9f);
            
            //Debug.Log("progeso de carga: " + progress);
            yield return null;
        }
        
        //yield return null;
    }
    
    
   


}
