using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class GlobalVar : MonoBehaviour
    {
        //Gestionar la dificultad con un boton que setee el valor de la velocidad cuando el bool de movimiento esta en falso y el personaje esta quieto
        //Si es facil se setee 0
        //Si es dificil se setee en -1

        //Gestionar la dificultad con un boton que setee el valor de la velocidad cuando el bool de movimiento esta en true y el personaje esta moviendo
        //Si es facil se setee 6
        //Si es dificil se setee en 3

        //Seleccion de Dificultad
        public static float velocidadPositiva;
        public static float velocidadNegativa;
        

        //public static float currentTime = 10f;
        //Umbral de velocidad de movimiento
        public static double speedThreshold = 80;

        public Slider sliderTime;
        public static float currentTime = 60f;

        public void Start()
        {
            //currentTime = sliderTime.value * 60;
            sliderTime.value = PlayerPrefs.GetFloat("sliderTime", 60f);
            velocidadPositiva = PlayerPrefs.GetFloat("velocidadPositiva");
            velocidadNegativa = PlayerPrefs.GetFloat("velocidadNegativa");
        }

        public void ChangeSliderTime(float valor)
        {
            currentTime = valor * 60f;
            PlayerPrefs.SetFloat("sliderTime", currentTime);
            Debug.Log("Tiempo de juego" + currentTime);
        }

        public void SetDifficultyEasy()
        {
            PlayerPrefs.SetFloat("velocidadPositiva", 6f);
            PlayerPrefs.SetFloat("velocidadNegativa", 0f);
            Debug.Log("Dificultad seteada en facil con velpos " + PlayerPrefs.GetFloat("velocidadPositiva") +
                      " con velneg " + PlayerPrefs.GetFloat("velocidadNegativa"));
        }

        public void SetDifficultyHard()
        {
            PlayerPrefs.SetFloat("velocidadPositiva", 3f);
            PlayerPrefs.SetFloat("velocidadNegativa", -1f);
            Debug.Log("Dificultad seteada en dificil con velpos" + PlayerPrefs.GetFloat("velocidadPositiva") +
                      " con velneg " + PlayerPrefs.GetFloat("velocidadNegativa"));
        }
    }
}