using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    public Dictionary<int, PoolingData> Data = new Dictionary<int, PoolingData>();

    [SerializeField] List<GameObject> SoundBox_Pool;
    [SerializeField] List<GameObject> Target_Pool;
    [SerializeField] List<GameObject> AR_Pool;
    [SerializeField] List<GameObject> MG_Pool;
    [SerializeField] List<GameObject> SR_Pool;
    [SerializeField] List<GameObject> PST_Pool;

    private GameObject SoundBox_Prefab;
    private GameObject Target_Prefab;
    private GameObject AR_Prefab;
    private GameObject MG_Prefab;
    private GameObject SR_Prefab;
    private GameObject PST_Prefab;

    private readonly int SoundBox_Max = 30;
    private readonly int Target_Max = 10;
    private readonly int AR_Max = 10;
    private readonly int MG_Max = 10;
    private readonly int SR_Max = 10;
    private readonly int PST_Max = 10;

    private readonly string SoundBox_Group = "SoundBoxGroup";
    private readonly string Target_Group = "TargetGroup";
    private readonly string AR_Group = "ARGroup";
    private readonly string MG_Group = "MGGroup";
    private readonly string SR_Group = "SRGroup";
    private readonly string PST_Group = "PSTGroup";

    private readonly string SoundBox_Obj = "SoundBox";
    private readonly string Target_Obj = "Target";
    private readonly string AR_Obj = "M4";
    private readonly string MG_Obj = "MP5K";
    private readonly string SR_Obj = "Kar98";
    private readonly string PST_Obj = "Ghost";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SoundBox_Prefab = Resources.Load<GameObject>(SoundBox_Obj);
        Target_Prefab = Resources.Load<GameObject>(Target_Obj);
        AR_Prefab = Resources.Load<GameObject>(AR_Obj);
        MG_Prefab = Resources.Load<GameObject>(MG_Obj);
        SR_Prefab = Resources.Load<GameObject>(SR_Obj);
        PST_Prefab = Resources.Load<GameObject>(PST_Obj);

        Data.Add(0, new PoolingData(SoundBox_Pool, SoundBox_Prefab, SoundBox_Group, SoundBox_Obj, SoundBox_Max));
        Data.Add(1, new PoolingData(Target_Pool, Target_Prefab, Target_Group, Target_Obj, Target_Max));
        Data.Add(2, new PoolingData(AR_Pool, AR_Prefab, AR_Group, AR_Obj, AR_Max));
        Data.Add(3, new PoolingData(MG_Pool, MG_Prefab, MG_Group, MG_Obj, MG_Max));
        Data.Add(4, new PoolingData(SR_Pool, SR_Prefab, SR_Group, SR_Obj, SR_Max));
        Data.Add(5, new PoolingData(PST_Pool, PST_Prefab, PST_Group, PST_Obj, PST_Max));

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
