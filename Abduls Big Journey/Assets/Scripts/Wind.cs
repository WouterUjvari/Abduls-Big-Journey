using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public GameObject cloudOrange;
    public GameObject cloudBlue;

    public Vector2 windDirectionAndStrenght = new Vector2(0, -9.81f);

    void LateUpdate()
    {
        Physics2D.gravity = windDirectionAndStrenght;
    }
}
