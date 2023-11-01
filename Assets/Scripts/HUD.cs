using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


}
