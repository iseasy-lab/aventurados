using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    

public class GlobalVar : MonoBehaviour
{
    //public static float currentTime = 10f;
    public static double speedThreshold = 90;

    public Slider sliderTime;
    public static float currentTime;
    
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