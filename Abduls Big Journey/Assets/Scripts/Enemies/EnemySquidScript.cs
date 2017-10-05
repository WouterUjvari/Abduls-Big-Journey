using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquidScript : Enemy
{

    public override void Hit(int damage)
    {
        base.Hit(damage);
        //print("squid hit");
        anim.SetTrigger("pStagger");
    }

    public override void Attack()
    {

        //base.Attack();
        print("Attack!!!");
        anim.SetTrigger("pInkAttack");
    }

    public void Spit()
    {
        GameObject item = Instantiate(throwingItem, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

        Vector3 playerHitDirectionRandomizer = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));

        Vector3 playerHitDirection = new Vector3(Battle.instance.player.transform.position.x + playerHitDirectionRandomizer.x,
                                                 Battle.instance.player.transform.position.y + 5 + playerHitDirectionRandomizer.y,
                                                 Battle.instance.player.transform.position.z + playerHitDirectionRandomizer.z);
        Vector3 shootDirection = (playerHitDirection - transform.position).normalized;

        item.GetComponent<Rigidbody>().AddForce(shootDirection * throwForce);
    }
}
