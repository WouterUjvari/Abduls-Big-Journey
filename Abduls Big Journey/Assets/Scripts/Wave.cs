using UnityEngine;

public class Wave : MonoBehaviour
{

    private Transform waveGraphic;
    private Quaternion waveGraphicRotation;

    public float waveRotationSpeed;

    private void Awake()
    {
        waveGraphic = transform.GetChild(0);

        waveGraphicRotation = waveGraphic.rotation;
    }

    private void Update()
    {
        transform.Rotate(-Vector3.forward * (Time.deltaTime * waveRotationSpeed));

        waveGraphic.rotation = waveGraphicRotation;
    }
}
