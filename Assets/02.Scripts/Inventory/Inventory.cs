using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Dictionary<int, InventoryItem> SlotData; //슬롯 번호가 key값, 들어있는 정보는 총기 데이터
    [SerializeField] List<GameObject> SlotList;
    Transform PlayerSlot;
    private int slotIdx;
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
        PlayerSlot = transform.GetChild(1).transform;
        for(int i = 0; i < PlayerSlot.childCount; i++)
        {
            SlotList.Add(PlayerSlot.GetChild(i).gameObject);
        }
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
                break;

            case 1: //권총
                ActiveSlot(SlotIdx);
                break;

            case 2: //칼
                ActiveSlot(SlotIdx);
                break;

            case 3: //조건문 추가 필요, 공격자만 case에 들어오도록
                ActiveSlot(SlotIdx);
                break;
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

    private void GetItem(GameObject Item) //아이템을 얻을 때 (아이템 습득, 아이템 구매에서 호출)
    {
        Gun gun = Item.GetComponent<Gun>();
        int Idx = gun.gundata.SlotIdxData;

        SlotDataUpdate(Item, Idx);

        Transform itemSlot = SlotList[Idx].transform;
        Item.transform.SetParent(itemSlot);
    }

    private void DropItem(int Idx) //Idx가 0과 1일 때만 가능.
    {
        if(Idx == 0 || Idx == 1)
        {
            GameObject gun = SlotList[Idx];
            gun.transform.parent = null; //(부모제거) 하이라키 공간으로 이동
            DeleteSlotData(Idx); //드랍후 데이터 삭제.
            
            MeshCollider gunmesh = gun.GetComponent<MeshCollider>();
            Rigidbody gunrb = gun.GetComponent<Rigidbody>();

            gun.transform.position = transform.position + new Vector3(0f, 0f, 1.0f);
            gunmesh.enabled = true;
            gunrb.isKinematic = false;
        }
    }

    
    private void SlotDataUpdate(GameObject Item, int Idx) //데이터 추가
    {
        if(SlotData.ContainsKey(Idx)) //총을 먹기전에 해당 슬롯에 총이 존재할 경우
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
