using UnityEngine;

public class InventoryItem
{
    public GameObject Gun;
    public GunData gunData;

    public InventoryItem (GameObject gun)
    {
        Gun = gun;
        Gun GunScript = gun.GetComponent<Gun>();
        gunData = GunScript.gundata;
    }
}
