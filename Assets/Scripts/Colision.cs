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
    private string tag = "Player";
    

    public void OnCollisionEnter(Collision collision)
    {
            Player = GameObject.FindGameObjectWithTag(tag);
            Debug.Log("colision y asignacion del punto");
            GameManager.SumarPuntos(1);
            Player.transform.position = posicionInicial;
    }
    
    
}
