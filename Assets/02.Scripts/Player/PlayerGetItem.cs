using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
   Transform CameraPivot;
   Inventory playerInventory;
   Ray camRay;

   int gunLayer;

   [SerializeField] bool isActive;
   public bool IsActive
   {
        get { return isActive; }
        set { isActive = value; }
   }
   private void Awake()
   {
        CameraPivot = transform.GetChild(0).transform;
        playerInventory = GetComponent<Inventory>();
        camRay = new Ray(CameraPivot.position, CameraPivot.forward);

        gunLayer = 1 << 6;
   }

   private void Update()
   {
        RaycastHit hit;
        Debug.DrawRay(CameraPivot.position, CameraPivot.forward * 5.0f, Color.red);

        if(Physics.Raycast(CameraPivot.position, CameraPivot.forward, out hit, 5.0f, gunLayer))
        {
            if(IsActive)
            {
                playerInventory.GetItem(hit.collider.gameObject);
            }
        }
   }
}
