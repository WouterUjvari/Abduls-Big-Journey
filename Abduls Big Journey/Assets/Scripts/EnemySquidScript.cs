using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquidScript : Enemy {

    public override void Hit(int damage)
    {
        base.Hit(damage);
        //print("squid hit");
        anim.SetTrigger("pStagger");
    }

    public override void Attack()
    {
        
        base.Attack();
        print("Attack!!!");
        anim.SetTrigger("pInkAttack");
    }


}
