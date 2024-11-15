using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private GameObject Bomb;
    private Inventory inventory;

    private bool isUse;
    public bool IsUse
    {
        get { return isUse; }
        set
        {
            isUse = value;
            if(inventory.SlotIdx == 3 && GameManager.instance.AIGunMatchLevelIdx == 0)
                UseDefuse(IsUse);
            else if(inventory.SlotIdx == 3 && GameManager.instance.AIGunMatchLevelIdx == 1)
                UseBomb(IsUse);
        }
    }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void UseDefuse(bool isuse)
    {
        if(Bomb != null)
        {
            BombState bombstate = Bomb.GetComponent<BombState>();
            bombstate.IsDefuse = isuse;
        }
    }

    private void UseBomb(bool isuse)
    {
        //폭탄 설치 함수 설정
    }

    private void OnColliderEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bomb"))
            Bomb = col.gameObject;
    }
    private void OnColliderExit(Collider col)
    {
        if (col.gameObject.CompareTag("Bomb"))
            Bomb = null;
    }
}
