using UnityEngine;
using UnityEngine.Serialization;


public class Colision : MonoBehaviour
{
    [FormerlySerializedAs("GameManager")] public GameManager gameManager;
    [FormerlySerializedAs("Player")] public GameObject player;
    public Vector3 posicionInicial = new Vector3(0, 1.25f, -3);
    private new string tag = "Player";


    public void OnCollisionEnter(Collision collision)
    {
        player = GameObject.FindGameObjectWithTag(tag);
        Debug.Log("colision y asignacion del punto");
        gameManager.SumarPuntos(1);
        player.transform.position = posicionInicial;
    }
}