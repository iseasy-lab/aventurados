using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI Puntos;

    // Update is called once per frame

    void Update()
    {
        Puntos.text = gameManager.PuntosTotales.ToString();
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


}
