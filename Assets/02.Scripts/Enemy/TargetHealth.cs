using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : HealthUpdate
{
   public override void HitDamage(int damage)
   {
        base.HitDamage(damage);
   }
    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
