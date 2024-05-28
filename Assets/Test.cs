using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    
    void Start()
    {
        Debug.Log("test");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log ("update");
        if(Input.GetKeyUp(KeyCode.Escape))
        {

        }
    }
}
