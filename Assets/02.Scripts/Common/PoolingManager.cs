using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    public Dictionary<int, PoolingData> Data = new Dictionary<int, PoolingData>();

    private List<GameObject> SoundBox_Pool;

    private GameObject SoundBox_Prefab;

    private readonly int SoundBox_Max = 30;

    private readonly string SoundBox_Group = "SoundBoxGroup";
    private readonly string SoundBox_Obj = "SoundBox";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SoundBox_Prefab = Resources.Load<GameObject>(SoundBox_Obj);

        //Data.Add(0, new PoolingData(SoundBox_Pool, SoundBox_Prefab, SoundBox_Group, SoundBox_Obj, SoundBox_Max));

        for (int i = 0; i < Data.Count; i++)
            Pooling(i, Data);
    }

    private void Pooling(int key, Dictionary<int, PoolingData> data) //딕셔너리에 저장한 데이터로 오브젝트 풀링
    {
        GameObject Group = new GameObject(data[key].GroupName);
        for (int i = 0; i < data[key].MaxPool; i++)
        {
            var obj = Instantiate(data[key].Prefab, Group.transform);
            obj.transform.position = new Vector3(0f, -30f, 0f);
            obj.transform.rotation = Quaternion.identity;
            obj.name = $"{data[key].ObjName}";
            data[key].Pool_List.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject(int key) //딕셔너리 리스트에 저장된 오브젝트중 비활성화된 오브젝트 반환.
    {
        foreach (var obj in Data[key].Pool_List)
        {
            if (!obj.activeSelf)
                return obj;
        }
        return null;
    }
}
