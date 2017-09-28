using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;

    public Vector3 currentOffset;
    public Vector3 defaultOffset;
    public Vector3 battleOffset;

    public float followSpeed = 5f;

    private void Awake()
    {
        defaultOffset = new Vector3(0, 0, transform.position.z);
    }

    private void Update()
    {
        if (BattleManager.instance.battleState == BattleManager.BattleState.Start)
        {
            currentOffset = defaultOffset;
        }
        else if (BattleManager.instance.battleState == BattleManager.BattleState.Battling)
        {
            currentOffset = battleOffset;
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + currentOffset, followSpeed * Time.deltaTime);

        //if (BattleManager.instance.battleState == BattleManager.BattleState.Start)
        //{
        //    transform.LookAt(target.transform);
        //}
    }
}
