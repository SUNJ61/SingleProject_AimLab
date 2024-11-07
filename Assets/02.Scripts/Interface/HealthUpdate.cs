using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpdate : MonoBehaviour, IHealth
{
     protected int MaxHealth = 100;
     protected int MaxAmor = 50;
     public int health { get; protected set; }
     public int amor { get; protected set;}
     public bool dead { get; protected set; }

     protected virtual void OnEnable()
     {
          health = MaxHealth;
          amor = 0;
          dead = false;
     }
     public virtual void HitDamage(int damage)
     {
          health -= damage;
          if (health <= 0 && !dead)
               Die();
     }
     public virtual void AddHealth(int addhealth)
     {
          if(!dead)
               health += addhealth;
     }
     public virtual void AddAmor(int addamor)
     {
          if(!dead)
               amor += addamor;
     }

     public virtual void Die()
     {
          if(!dead)
               dead = true;
     }
}
