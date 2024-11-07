using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Transform> RandomShootGamespawnPoint;

    private readonly string RandomShootGamespawnPointName = "RandomShootGameSpawnPoint";

    private int RandomShootGameIdx = 0;

    public bool isGameover;
    public bool isGameStart;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        RandomShootGamespawnPoint = SpawnManager.instance.GetPoint(RandomShootGamespawnPointName);
    }

    public void RandomShootGame() //필드 UI를 통해 상호작용하면 게임 스타트
    {
        isGameStart = true;
        StartCoroutine(RandomShootGameStart(1.0f)); //난이도에 따라 입력 값 조절   
    }

    IEnumerator RandomShootGameStart(float delay)
    {
        while(RandomShootGameIdx < 20)
        {
            yield return new WaitForSeconds(0.3f);
            RandomShootGameIdx++;
            GameObject RandomShootGameObj = SpawnManager.instance.SetActivePos(RandomShootGamespawnPoint[Random.Range(0, RandomShootGamespawnPoint.Count)], 1);
            yield return new WaitForSeconds(delay);
            RandomShootGameObj.SetActive(false);
            Debug.Log(RandomShootGameIdx);
        }
        if(RandomShootGameIdx == 20)
            isGameStart = false;
    }
}
