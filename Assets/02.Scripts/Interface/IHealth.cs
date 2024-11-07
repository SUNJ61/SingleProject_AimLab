using UnityEngine;

public interface IHealth
{
    public void HitDamage(int damage);
    public void Die();
    public void AddHealth(int heal);
    public void AddAmor(int amor);
}
