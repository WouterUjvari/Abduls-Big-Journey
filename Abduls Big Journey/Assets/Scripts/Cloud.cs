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
            gameObject.transform.position = new Vector3(cloudBlue.transform.GetChild(0).transform.position.x, gameObject.transform.position.y, cloudBlue.transform.GetChild(0).transform.position.z); 
            //gameObject.transform.position - new Vector3(cloudBlue.transform.GetChild(0).transform.position.x, gameObject.transform.position.y, cloudBlue.transform.GetChild(0).transform.position.z);
            
        }

        if (other.gameObject.tag == "CloudBlue")
        {
            //gameObject.transform.position = cloudOrange.transform.GetChild(0).transform.position;
            gameObject.transform.position = new Vector3(cloudOrange.transform.GetChild(0).transform.position.x, gameObject.transform.position.y, cloudBlue.transform.GetChild(0).transform.position.z);
        }
    }

     
}
