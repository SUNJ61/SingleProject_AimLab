using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Transform> RandomShootGamespawnPoint;
    private GameObject currentTarget;

    private float[] RandomShootGameLevelDelay = new float[2] {1.0f , 0.6f};

    private readonly string RandomShootGamespawnPointName = "RandomShootGameSpawnPoint";

    private int RandomShootGameIdx = 0;
    private int RandomShootGameLevel = 0;
    public int Score = 0;

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
        Score = 0;
        StartCoroutine(RandomShootGameStart(RandomShootGameLevelDelay[RandomShootGameLevel])); //난이도에 따라 입력 값 조절   
    }

    public void RandomShootGameStop()
    {
        StopCoroutine(RandomShootGameStart(RandomShootGameLevelDelay[RandomShootGameLevel]));
        currentTarget.SetActive(false);
    }

    public void RandomShootGameLevelChange()
    {
        RandomShootGameLevel++;
        if(RandomShootGameLevel > 1)
            RandomShootGameLevel = 0;

        InGameUIManager.instance.RandomShootGameLevelText(RandomShootGameLevel);
    }

    IEnumerator RandomShootGameStart(float delay)
    {
        while(RandomShootGameIdx < 20)
        {
            yield return new WaitForSeconds(0.4f);
            RandomShootGameIdx++;
            GameObject RandomShootGameObj = SpawnManager.instance.SetActivePos(RandomShootGamespawnPoint[Random.Range(0, RandomShootGamespawnPoint.Count)], 1);
            currentTarget = RandomShootGameObj;
            yield return new WaitForSeconds(delay);
            RandomShootGameObj.SetActive(false);
        }

        if(RandomShootGameIdx == 20)
            isGameStart = false;
        
        InGameUIManager.instance.RandomShootGameScore(Score);
    }
}
