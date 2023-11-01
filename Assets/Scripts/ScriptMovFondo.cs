using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMovFondo : MonoBehaviour
{
    public float velocidad;

    public GameObject ObjetoDebajo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.z <= -11)
        {
            transform.position = ObjetoDebajo.transform.position + new Vector3(0, 0, 11);
        }
        
        transform.position += new Vector3(0, 0, velocidad * Time.deltaTime);
    }
}
