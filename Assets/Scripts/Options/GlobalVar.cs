using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    

public class GlobalVar : MonoBehaviour
{
    //public static float currentTime = 10f;
    //Umbral de velocidad de movimiento
    public static double speedThreshold = 80;

    public Slider sliderTime;
    public static float currentTime = 60f;
    
    public void Start()
    {
        //currentTime = sliderTime.value * 60;
        sliderTime.value = PlayerPrefs.GetFloat("sliderTime", 60f);
    }

    public void ChangeSliderTime(float valor)
    {
        currentTime = valor * 60f;
        PlayerPrefs.SetFloat("sliderTime", currentTime);
        Debug.Log("Tiempo de juego" + currentTime);
    }
}

}