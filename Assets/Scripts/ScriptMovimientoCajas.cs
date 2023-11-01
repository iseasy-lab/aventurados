using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(MeshCollider))]

public class ScriptMovimientoCajas : MonoBehaviour
{
    public float Velocidad = 3;
    Rigidbody rb;

    public int Valor = 1;
    public GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        rb.velocity = Vector3.back * Velocidad + Vector3.right * Random.Range(-1, 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("ColisionSuministro");
            Destroy(this.gameObject);
            GameManager.SumarPuntos(Valor);
            GameManager.PasarNivel(0);
        }   
    }
}
