using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void PulsarJugar()
    {
        Debug.Log("Ir a jugar");
        //Revisar la redirección del botón jugar
        SceneManager.LoadScene(2);
    }
    
    public void PulsarLeaderboard()
    {
        Debug.Log("Ir a leaderboards");
        SceneManager.LoadScene(4);
    }

    public void PulsarOpciones()
    {
        Debug.Log("Ir a Opciones");
        SceneManager.LoadScene(5);
    }
    
    public void PulsarRegresar()
    {
        Debug.Log("Ir al menu");
        SceneManager.LoadScene(1);
    }
    
    public void PulsarSalir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }


}
