using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;

    public Vector3 offset;
    public float followSpeed = 5f;

    private void Awake()
    {
        offset = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, followSpeed * Time.deltaTime);
        transform.LookAt(target.transform);
    }
}
