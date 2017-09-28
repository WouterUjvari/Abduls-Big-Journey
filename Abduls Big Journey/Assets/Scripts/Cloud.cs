using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public GameObject cloudOrange;
    public GameObject cloudBlue;

    void OnCollisionEnter2D (Collision2D other)
    {
        
        if (other.gameObject.tag == "CloudOrange")
        {          
            gameObject.transform.position = cloudBlue.transform.GetChild(0).transform.position;
            
        }

        if (other.gameObject.tag == "CloudBlue")
        {
            gameObject.transform.position = cloudOrange.transform.GetChild(0).transform.position;
        }
    }

     
}
