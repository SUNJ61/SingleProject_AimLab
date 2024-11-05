using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "NewGunData")]
public class GunData : ScriptableObject
{
    public Vector3 SlotTranform;

    public string GunType;

    public float Vertical_ReBound;
    public float Horizontal_ReBound;
    public float Delay;
    public float Fov; 

    public int BulletMax;
    public int SubBullet;
    public int SlotIdxData;

    public int[] FireBranch = new int[3];
    public int[] FireMode;
}
