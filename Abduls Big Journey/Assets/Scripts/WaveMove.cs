using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour {

    public float speedModifier = 1;
    private float timer;
    public float lifetime = 1000;

    private void FixedUpdate()
    {    
        transform.Translate(speedModifier * 0.5f, 0, 0);

        timer++;
        if (timer >= lifetime)
        {
            timer = 0;
            Destroy(gameObject);
        }
    }
}
