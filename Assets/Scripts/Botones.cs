using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonJugar : MonoBehaviour
{
    public void PulsarJugar()
    {
        Debug.Log("Ir a jugar");
        //Revisar la redirección del botón jugar
        SceneManager.LoadScene(3);
    }
    
    public void PulsarLeaderboard()
    {
        Debug.Log("Ir a leaderboards");
        SceneManager.LoadScene(4);
    }

    public void PulsarSalir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }


}
