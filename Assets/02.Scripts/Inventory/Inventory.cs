using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Dictionary<int, InventoryItem> SlotData = new Dictionary<int, InventoryItem>(); //슬롯 번호가 key값, 들어있는 정보는 총기 데이터
    [SerializeField] List<GameObject> SlotList;
    Transform PlayerSlot;
    PlayerFire playerFire;

    private float Timeprev;

    private readonly float delay = 0.1f;

    private int slotIdx = 0;
    public int SlotIdx
    {
        get { return slotIdx; }
        set
        {
            slotIdx = value;
            SelectSlot(SlotIdx);
        }
    }


    private void Awake()
    {
        PlayerSlot = transform.GetChild(0).GetChild(1).transform;
        playerFire = GetComponent<PlayerFire>();

        for(int i = 0; i < PlayerSlot.childCount; i++)
        {
            SlotList.Add(PlayerSlot.GetChild(i).gameObject);
        }

        Timeprev = Time.time;
    }

    private void OnEnable()
    {
        ActiveSlot(0); //플레이어 퀵슬롯 기본 상태 설정
        //칼 과 폭탄 배치하기
    }

    private void SelectSlot(int SlotIdx) //슬롯 선택
    {
        switch (SlotIdx)
        {
            case 0: //mg, ar ,sr 착용
                ActiveSlot(SlotIdx);
                GunDataUpdate(SlotIdx);
                break;

            case 1: //권총
                ActiveSlot(SlotIdx);
                GunDataUpdate(SlotIdx);
                break;

            case 2: //칼
                ActiveSlot(SlotIdx);
                GunDataUpdate(SlotIdx);
                break;

            case 3: //조건문 추가 필요, 공격자만 case에 들어오도록
                ActiveSlot(SlotIdx);
                GunDataUpdate(SlotIdx);
                break;
        }
    }

    private void GunDataUpdate(int SlotIdx)
    {
        if (SlotList[SlotIdx].transform.childCount != 0 && (SlotIdx == 0 || SlotIdx == 1))
        {
            playerFire.GunObj = SlotData[SlotIdx].Gun;
            playerFire.CanFire = true;
        }
        else
        {
            playerFire.CanFire = false;
        }
    }

    private void ActiveSlot(int SlotIdx) //인벤토리 활성화
    {
        for(int i = 0; i < PlayerSlot.childCount; i++)
        {
            if(i == SlotIdx)
                SlotList[i].SetActive(true);
            else
                SlotList[i].SetActive(false);
        }
    }

    public void GetItem(GameObject Item) //아이템을 얻을 때 (아이템 습득, 아이템 구매에서 호출)
    {
        Gun gun = Item.GetComponent<Gun>();
        MeshCollider gunmesh = Item.GetComponent<MeshCollider>();
        Rigidbody gunrb = Item.GetComponent<Rigidbody>();

        int Idx = gun.gundata.SlotIdxData;

        SlotDataUpdate(Item, Idx);

        gunmesh.isTrigger = true;
        gunrb.isKinematic = true;

        Transform itemSlot = SlotList[Idx].transform;
        Item.transform.SetParent(itemSlot);
        Item.transform.localPosition = gun.gundata.SlotTranform;
        Item.transform.localRotation = Quaternion.identity;

        if(SlotIdx == gun.gundata.SlotIdxData)
        {
            playerFire.GunObj = Item;
            playerFire.CanFire = true;
        }
    }

    public void PlayerDropItem()
    {
        if(SlotList[SlotIdx].transform.childCount != 0)
        {
            DropItem(SlotIdx);
        }
    }

    private void DropItem(int Idx) //Idx가 0과 1일 때만 가능.
    {
        if(Idx == 0 || Idx == 1)
        {
            GameObject gun = SlotList[Idx].transform.GetChild(0).gameObject;
            gun.transform.parent = null; //(부모제거) 하이라키 공간으로 이동
            playerFire.CanFire = false;
            DeleteSlotData(Idx); //드랍후 데이터 삭제.
            
            MeshCollider gunmesh = gun.GetComponent<MeshCollider>();
            Rigidbody gunrb = gun.GetComponent<Rigidbody>();

            gun.transform.position = transform.position + (transform.forward * 1.5f);
            gunmesh.isTrigger = false;
            gunrb.isKinematic = false;
        }
    }

    
    private void SlotDataUpdate(GameObject Item, int Idx) //데이터 추가
    {
        if(SlotList[Idx].transform.childCount != 0) //총을 먹기전에 해당 슬롯에 총이 존재할 경우
        {
            DropItem(Idx);
            SlotData.Add(Idx, new InventoryItem(Item));
        }
        else
            SlotData.Add(Idx, new InventoryItem(Item));
    }

    private void DeleteSlotData(int Idx) //아이템 인벤토리에서 삭제 될 때
    {
        SlotData.Remove(Idx);
    }
}
