using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Colision : MonoBehaviour
{
    
    public GameManager GameManager;
    public GameObject Player;
    public Vector3 posicionInicial = new Vector3(0, 1.25f, -3);
    
    public void OnCollisionEnter(Collision collision)
    {
            Debug.Log("La nave colision");
            GameManager.SumarPuntos(1);
            Player.transform.position = posicionInicial;
            
        
    }
    
    
}
