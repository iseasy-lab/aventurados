using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
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

    public TextMeshProUGUI Puntos;
    
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


    
    private void Start()
    {
        gameManager = GameManager.Instance;
        displayNamePlayer.text = "Bienvenido: "+ PlayFabConstants.displayName;
        Debug.Log("nombre del displayname es: " + PlayerPrefs.GetString(PlayFabConstants.SavedUsername, ""));
        
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

        if (currentScene == 3)
        {
            currentTime = Options.GlobalVar.currentTime;
            progressBar.maxValue = currentTime;
        }
    }

    void Update()
    {
        Puntos.text = gameManager.PuntosTotales.ToString();
        
        // Resta el tiempo transcurrido desde el último frame
        currentTime -= Time.deltaTime;
        Debug.Log("tiempo del nivel: " + currentTime);
        progressBar.value = currentTime;
        //GameManager.SumarPuntos(1);
        // Si se cumple el tiempo deseado, llama al método GameOver
        if (currentTime < 0.5f && currentTime > -0.5f)
        {
            gameManager.GameOver();
        }
    }

    public void ActualizarPuntos(int puntosTotales)
    {
        Puntos.text = puntosTotales.ToString();
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
        SceneManager.LoadScene(3);
        Time.timeScale = 1f;
    }
    
    public void Quit()
    {
        SceneManager.LoadScene(1);
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

    public void PlayLevel()
    {
       //AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Cargar escena del nivel del juego
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadInProgress.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log("progeso de carga: " + progress);
            yield return null;
        }
    }
    
    
   


}
