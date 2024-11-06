using UnityEngine;

public class Gun : MonoBehaviour
{
   public GunData gundata;
   private GunData OriginScriptableObject;

   private void Awake()
   {
      OriginScriptableObject = Resources.Load<GunData>(this.name + "GunData");
      gundata = Instantiate(OriginScriptableObject);
   }
}
