using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
   Transform CameraPivot;
   Inventory playerInventory;
   RaycastHit hit;

   int gunLayer;

   [SerializeField] bool isActive;
   public bool IsActive
   {
     get { return isActive; }
     set 
     {
          isActive = value;
          if(IsActive)
               GetGun();
     }
   }

   [SerializeField] bool isDrop;
   public bool IsDrop
   {
     get {return isDrop;}
     set 
     {
          isDrop = value;
          if(IsDrop)
          {
               DropGun();
               IsDrop = false;
          }
     }
   }
   private void Awake()
   {
        CameraPivot = transform.GetChild(0).transform;
        playerInventory = GetComponent<Inventory>();

        gunLayer = 1 << 6;
   }

    private void GetGun()
    {
        if (Physics.Raycast(CameraPivot.position, CameraPivot.forward, out hit, 5.0f, gunLayer))
        {
          playerInventory.GetItem(hit.collider.gameObject);
        }

        IsActive = false;
    }

    private void DropGun()
    {
        playerInventory.PlayerDropItem();
    }
}
