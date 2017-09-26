using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitWave : MonoBehaviour {

    public List<GameObject> waveList = new List<GameObject>();
    public float timer;
    public float frequency;

    private void FixedUpdate()
    {
        timer++;
        if (timer >= frequency)
        {
            timer = 0;
            //print("Wave");
            Instantiate(waveList[Random.Range(0, 4)], transform);
        }
    }
}
