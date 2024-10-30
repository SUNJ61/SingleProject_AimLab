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

    public void SetActivePos(GameObject spawnPos, int key) //특정 오브젝트 위치에 풀링한 오브젝트 소환하기
    {// 특정 오브젝트, 풀링매니저에서 소환 하고 싶은 오브젝트의 key값 
        if (spawnPos != null)
        {
            GameObject ex = PoolingManager.instance.GetObject(key);
            Transform pos = spawnPos.transform;
            ex.transform.position = pos.position;
            ex.SetActive(true);
        }
    }

    public void SetActiveFar(List<Transform> SpawnPoint, Transform standard, int key, float Delay) // 첫 소환에 호출
    {// 스폰포인트 리스트, 스폰포인트와 거리를 계산할 위치, 소환하고싶은 오브젝트의 키값, 소환 딜레이
        GameObject ex = PoolingManager.instance.GetObject(key);
        if (ex != null)
            StartCoroutine(RespawnWait(SpawnPoint, standard, ex, Delay));
    }

    public void RespawnFar(List<Transform> SpawnPoint, Transform standard, GameObject RespawnObj, float Delay) // 기존에 소환한 오브젝트를 재소환 할 경우 호출
    {// 스폰포인트 리스트, 스폰포인트와 거리를 계산할 위치, 기존에 소환한 오브젝트, 소환 딜레이
        StartCoroutine(RespawnWait(SpawnPoint, standard, RespawnObj, Delay));
    }

    IEnumerator RespawnWait(List<Transform> SpawnPoint, Transform Standard, GameObject RespawnObj, float Delay) // 일정시간 후 플레이어와 가장 먼 곳에서 오브젝트 소환
    {
        yield return new WaitForSeconds(Delay);
        FarRespawnSetup(SpawnPoint, RespawnObj, Standard);
        RespawnObj.SetActive(true);
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
            ex.SetActive(true);
        }

        GameObject exGroup = GameObject.Find(PoolingManager.instance.Data[key].GroupName);
        Destroy(exGroup);
    }

    private void FarRespawnSetup(List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard) //먼 리스폰 위치 찾는 로직
    {
        float Respawn_Dist = (SpawnPoint[0].position - Standard.position).magnitude;
        Vector3 Respawn_Pos = SpawnPoint[0].position;
        foreach (Transform point in SpawnPoint)
        {
            float Dist = (point.position - Standard.position).magnitude;
            if (Dist > Respawn_Dist)
                Respawn_Pos = point.position;
        }

        RespawnObj.transform.position = Respawn_Pos;
    }

    private int GetRandomIdx(List<int> RandomIdx) //랜덤값 뽑기.
    {
        int i = Random.Range(0, RandomIdx.Count);
        int idx = RandomIdx[i];
        RandomIdx.RemoveAt(i);
        return idx;
    }
}
