using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "NewGunData")]
public class GunData : ScriptableObject
{
    public Vector3 SlotTranform;

    public string GunType;

    public float VerticalReBound;
    public float HorizontalReBound;
    public float Delay;
    public float Fov; 

    public int BulletMax;
    public int SubBullet;
    public int SlotIdxData;
    public int BodyDamage;
    public int HeadDamage;

    public int[] FireBranch = new int[3];
    public int[] FireMode;
}
