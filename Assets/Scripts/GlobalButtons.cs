using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalButtons : MonoBehaviour
{
    //Pantalla de carga previa al nivel del juego
    [SerializeField] private GameObject loadInProgress;

    public void PulsarJugar()
    {
        Debug.Log("Ir a jugar");
        //Revisar la redirección del botón jugar
        //SceneManager.LoadScene(2);
        StartCoroutine(LoadAsync());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        loadInProgress.SetActive(true);
    }

    IEnumerator LoadAsync()
    {
        loadInProgress.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / .9f);
            loadInProgress.SetActive(true);
            //Debug.Log("progeso de carga: " + progress);
            yield return null;
        }

        //yield return null;
    }


    public void PulsarLeaderboard()
    {
        Debug.Log("Ir a leaderboards");
        SceneManager.LoadScene(7);
    }

    public void PulsarJuego()
    {
        Debug.Log("Ir al juego");
        SceneManager.LoadScene(4);
    }

    public void PulsarOpciones()
    {
        Debug.Log("Ir a Opciones");
        SceneManager.LoadScene(8);
    }

    public void PulsarRegresar()
    {
        Debug.Log("Ir al menu");
        SceneManager.LoadScene(3);
    }

    public void PulsarSalir()
    {
        Debug.Log("Salir del menu principal");
        SceneManager.LoadScene(0);
    }

    public void PulsarQuitMenu()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }

    public void PulsarCerrarSesion()
    {
        Debug.Log("CerrandoSesion");
        PlayFab.PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene(2);
    }
}