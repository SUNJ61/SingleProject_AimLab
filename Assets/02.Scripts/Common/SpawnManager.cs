using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    public List<Transform> GetPoint(string GetPosName) //Pos리스트 가저오기.
    {
        List<Transform> PosList = new List<Transform>();
        var spawn = GameObject.Find(GetPosName).transform;
        if (spawn != null)
        {
            foreach (Transform pos in spawn)
                PosList.Add(pos);
        }

        return PosList;
    }

    public List<int> MakeIdxList<T>(List<T> list) //리스트 길이 만큼의 숫자를 가진 리스트 작성.
    {
        List<int> idxList = new List<int>();

        for (int i = 0; i < list.Count; i++)
            idxList.Add(i);

        return idxList;
    }  

    public GameObject SetActivePos(Transform spawnPos, int key) //특정 오브젝트 위치에 풀링한 오브젝트 소환하기
    {// 특정 위치에, 풀링매니저에서 소환 하고 싶은 오브젝트의 key값 
        if (spawnPos != null)
        {
            GameObject obj = PoolingManager.instance.GetObject(key);
            obj.transform.position = spawnPos.position;
            obj.transform.rotation = spawnPos.rotation;
            obj.SetActive(true);

            return obj; //소환한 오브젝트 반환
        }
        else
            return null;
    }

    public void SetActiveRandomPos(List<Transform> SpawnPoint, int key) // 랜덤 위치 소환
    {// 스폰포인트 리스트, 소환하고싶은 오브젝트의 키값
        List<GameObject> PoolList = PoolingManager.instance.Data[key].Pool_List;
        List<int> objIdxList = MakeIdxList(SpawnPoint);

        foreach (GameObject ex in PoolList)
        {
            int idx = GetRandomIdx(objIdxList);
            ex.transform.parent = SpawnPoint[idx];
            ex.transform.position = SpawnPoint[idx].position;
            ex.transform.rotation = SpawnPoint[idx].rotation;
            ex.SetActive(true);
        }

        GameObject exGroup = GameObject.Find(PoolingManager.instance.Data[key].GroupName);
        Destroy(exGroup);
    }

    private int GetRandomIdx(List<int> RandomIdx) //랜덤값 뽑기.
    {
        int i = Random.Range(0, RandomIdx.Count);
        int idx = RandomIdx[i];
        RandomIdx.RemoveAt(i);
        return idx;
    }
}
