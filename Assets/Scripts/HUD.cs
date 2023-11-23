using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    
    private float currentTime = Options.GlobalVar.currentTime;

    public TextMeshProUGUI Puntos;
    
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
    }

    void Update()
    {
        Puntos.text = gameManager.PuntosTotales.ToString();
        
        // Resta el tiempo transcurrido desde el último frame
        currentTime -= Time.deltaTime;
        //GameManager.SumarPuntos(1);
        // Si se cumple el tiempo deseado, llama al método GameOver
        if (currentTime <= 0f)
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Cargar escena del nivel del juego
    }


}
