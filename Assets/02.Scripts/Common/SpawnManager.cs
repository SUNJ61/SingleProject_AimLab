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

    public List<Transform> GetPoint(string GetPosName) //Pos����Ʈ ��������.
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

    public List<int> MakeIdxList<T>(List<T> list) //����Ʈ ���� ��ŭ�� ���ڸ� ���� ����Ʈ �ۼ�.
    {
        List<int> idxList = new List<int>();

        for (int i = 0; i < list.Count; i++)
            idxList.Add(i);

        return idxList;
    }  

    public void SetActivePos(GameObject spawnPos, int key) //Ư�� ������Ʈ ��ġ�� Ǯ���� ������Ʈ ��ȯ�ϱ�
    {// Ư�� ������Ʈ, Ǯ���Ŵ������� ��ȯ �ϰ� ���� ������Ʈ�� key�� 
        if (spawnPos != null)
        {
            GameObject ex = PoolingManager.instance.GetObject(key);
            Transform pos = spawnPos.transform;
            ex.transform.position = pos.position;
            ex.SetActive(true);
        }
    }

    public void SetActiveFar(List<Transform> SpawnPoint, Transform standard, int key, float Delay) // ù ��ȯ�� ȣ��
    {// ��������Ʈ ����Ʈ, ��������Ʈ�� �Ÿ��� ����� ��ġ, ��ȯ�ϰ���� ������Ʈ�� Ű��, ��ȯ ������
        GameObject ex = PoolingManager.instance.GetObject(key);
        if (ex != null)
            StartCoroutine(RespawnWait(SpawnPoint, standard, ex, Delay));
    }

    public void RespawnFar(List<Transform> SpawnPoint, Transform standard, GameObject RespawnObj, float Delay) // ������ ��ȯ�� ������Ʈ�� ���ȯ �� ��� ȣ��
    {// ��������Ʈ ����Ʈ, ��������Ʈ�� �Ÿ��� ����� ��ġ, ������ ��ȯ�� ������Ʈ, ��ȯ ������
        StartCoroutine(RespawnWait(SpawnPoint, standard, RespawnObj, Delay));
    }

    IEnumerator RespawnWait(List<Transform> SpawnPoint, Transform Standard, GameObject RespawnObj, float Delay) // �����ð� �� �÷��̾�� ���� �� ������ ������Ʈ ��ȯ
    {
        yield return new WaitForSeconds(Delay);
        FarRespawnSetup(SpawnPoint, RespawnObj, Standard);
        RespawnObj.SetActive(true);
    }

    public void SetActiveRandomPos(List<Transform> SpawnPoint, int key) // ���� ��ġ ��ȯ
    {// ��������Ʈ ����Ʈ, ��ȯ�ϰ���� ������Ʈ�� Ű��
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

    private void FarRespawnSetup(List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard) //�� ������ ��ġ ã�� ����
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

    private int GetRandomIdx(List<int> RandomIdx) //������ �̱�.
    {
        int i = Random.Range(0, RandomIdx.Count);
        int idx = RandomIdx[i];
        RandomIdx.RemoveAt(i);
        return idx;
    }
}
