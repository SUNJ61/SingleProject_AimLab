using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
   public GunData gundata;
   private GunData OriginScriptableObject;
   private Coroutine DropCoroutine;

   private int MaxBullet;
   private int SubBullet;
   
   private void Start()
   {
      OriginScriptableObject = Resources.Load<GunData>(this.name + "GunData");
      gundata = Instantiate(OriginScriptableObject);

      MaxBullet = gundata.BulletMax;
      SubBullet = gundata.SubBullet;
   }

   public void DropState() //버리면 코루틴 시작.
   {
      DropCoroutine = StartCoroutine(ActiveFalseGun());
   }

   public void GetState() //다시 먹으면 코루틴 종료
   {
      if(DropCoroutine != null)
         StopCoroutine(DropCoroutine);
   }

   private void BulletReset()
   {
      gundata.BulletMax = MaxBullet;
      gundata.SubBullet = SubBullet;
   }

   IEnumerator ActiveFalseGun() //30초 뒤 총기 비활성화
   {
      yield return new WaitForSeconds(30.0f);
      BulletReset();
      gameObject.SetActive(false);
   }
}
