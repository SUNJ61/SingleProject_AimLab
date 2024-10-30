using System.Collections.Generic;
using UnityEngine;

public class PoolingData
{//Ǯ�� ������ �� ����
    public List<GameObject> Pool_List;
    public GameObject Prefab;
    public string GroupName;
    public string ObjName;
    public int MaxPool;

    public PoolingData(List<GameObject> pool, GameObject prefab, string Groupname, string Objname, int maxPool)
    {
        Pool_List = pool;
        Prefab = prefab;
        GroupName = Groupname;
        ObjName = Objname;
        MaxPool = maxPool;
    }
}
