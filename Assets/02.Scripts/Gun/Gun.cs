using UnityEngine;

public class Gun : MonoBehaviour
{
   public GunData gundata;
   private GunChange gunChange;

   private void GetGun() //총을 구매하거나, 총을 먹었을 때나, 총을 변경했을 때 SendMessage로 호출할 함수.
   {
        gunChange = transform.parent.parent.parent.GetComponent<GunChange>();
        gunChange.PlayerGunData = gundata;
   }
}
