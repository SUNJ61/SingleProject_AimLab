using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "NewGunData")]
public class GunData : ScriptableObject
{
    public string GunType;

    public float Vertical_ReBound;
    public float Horizontal_ReBound;
    public float Delay;

    public int BulletMax;
    public int SubBullet;

    public int[] FireBranch = new int[3];
    public int[] FireMode;
}
