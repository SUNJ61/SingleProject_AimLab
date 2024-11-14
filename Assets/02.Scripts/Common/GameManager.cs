using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]private List<Transform> RandomShootGamespawnPoint;
    [SerializeField]private List<Transform> AIGunMatchPlayerSpawnPoint;
    [SerializeField]private List<Transform> AIGunMatchAttackerSpawnPoint;
    [SerializeField]private List<Transform> AIGunMatchDependerSpawnPoint;
    private GameObject currentTarget;
    private Coroutine RandomShootGameCorutine;
    private GameObject Player;

    private float[] RandomShootGameLevelDelay = new float[2] {1.8f , 1.0f};

    private string[] RandomShootGameLevel = {"Normal","Hard"};
    private string[] AIGunMatchLevel = {"Attacker","Defender"};

    private readonly string RandomShootGamespawnPointName = "RandomShootGameSpawnPoint";
    private readonly string AIGunMatchPlayerSpawnPointName = "AIGunMatchPlayerSpawnPoint";
    private readonly string AIGunMatchAttackerSpawnPointName = "AIGunMatchAttackerSpawnPoint";
    private readonly string AIGunMatchDependerSpawnPointName = "AIGunMatchDefenderSpawnPoint"; 

    public float ClearTime = 0f;

    private int RandomShootGameIdx = 0;
    private int RandomShootGameLevelIdx = 0;
    [SerializeField]private int AIGunMatchLevelIdx = 0;
    public int Score = 0;

    public bool isGameover = false;
    public bool isGameStart = false;
    public bool isTeleport = false;
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
        AIGunMatchPlayerSpawnPoint = SpawnManager.instance.GetPoint(AIGunMatchPlayerSpawnPointName);
        AIGunMatchAttackerSpawnPoint = SpawnManager.instance.GetPoint(AIGunMatchAttackerSpawnPointName);
        AIGunMatchDependerSpawnPoint = SpawnManager.instance.GetPoint(AIGunMatchDependerSpawnPointName);
        Player = GameObject.Find("Player");

        CursorCtrl(InGameUIManager.instance.activeShopUI);
    }

    public void CursorCtrl(bool active)
    {
        switch(active)
        {
            case true:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            break;

            case false:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            break;
        }
    }

    public void RandomShootGame() //필드 UI를 통해 상호작용하면 게임 스타트
    {
        if(!isGameStart)
        {
            isGameStart = true;
            Score = 0;
            RandomShootGameCorutine = StartCoroutine(RandomShootGameStart(RandomShootGameLevelDelay[RandomShootGameLevelIdx])); //난이도에 따라 입력 값 조절   
        }
    }

    public void RandomShootGameStop()
    {
        if(RandomShootGameCorutine != null)
        {
            StopCoroutine(RandomShootGameCorutine);
            RandomShootGameIdx = 0;
        }

        if(currentTarget != null)
            currentTarget.SetActive(false);

        isGameStart = false;
    }

    public void RandomShootGameLevelChange()
    {
        if(!isGameStart)
        {
            RandomShootGameLevelIdx++;
            if(RandomShootGameLevelIdx > 1)
                RandomShootGameLevelIdx = 0;

            InGameUIManager.instance.RandomShootGameLevelText(RandomShootGameLevel[RandomShootGameLevelIdx]);
        }

    }

    IEnumerator RandomShootGameStart(float delay)
    {
        while(RandomShootGameIdx < 20)
        {
            yield return new WaitForSeconds(0.5f);
            RandomShootGameIdx++;
            GameObject RandomShootGameObj = SpawnManager.instance.SetActivePos(RandomShootGamespawnPoint[Random.Range(0, RandomShootGamespawnPoint.Count)], 1);
            currentTarget = RandomShootGameObj;
            yield return new WaitForSeconds(delay);
            RandomShootGameObj.SetActive(false);
        }

        if(RandomShootGameIdx == 20)
        {
            isGameStart = false;
            RandomShootGameIdx = 0;
        }
        
        InGameUIManager.instance.RandomShootGameScore(Score);
    }

    public void AIGunMatchStart()
    {
        if(!isGameStart)
        {
            isGameStart = true;
            isTeleport =true;
            ClearTime = 0f;
            AIGunMatchSetting(AIGunMatchLevelIdx); //게임 세팅
            StartCoroutine(TeleportPlayer(AIGunMatchPlayerSpawnPoint[AIGunMatchLevelIdx]));
            StartCoroutine(AIGunMatchTimer());
        }
    }

    public void AIGunMatchLevelChange()
    {
        if(!isGameStart)
        {
            AIGunMatchLevelIdx++;
            if(AIGunMatchLevelIdx > 1)
                AIGunMatchLevelIdx = 0;

            InGameUIManager.instance.AIGunMatchLevelText(AIGunMatchLevel[AIGunMatchLevelIdx]);
        }
    }

    public void AIGunMatchFalse() //플레이어가 사망시 호출
    {
        isGameStart = false;
        isTeleport = true;
        StartCoroutine(TeleportPlayer(AIGunMatchPlayerSpawnPoint[2]));
    }

    private IEnumerator TeleportPlayer(Transform pos)
    {
        yield return new WaitForSeconds(0.1f);
        Player.transform.position = pos.position;
        yield return new WaitForSeconds(0.1f);
        isTeleport = false;
    }

    private void AIGunMatchSetting(int idx)
    {
        switch(AIGunMatchLevel[idx])
        {
            case "Attacker": //공격몹 6개 소환
            Player.tag = AIGunMatchLevel[idx];
            SpawnManager.instance.SetActiveRandomPos(AIGunMatchAttackerSpawnPoint, 6);
                break;

            case "Defender":
            Player.tag = AIGunMatchLevel[idx];
                break; 
        }
    }

    private IEnumerator AIGunMatchTimer()
    {
        while(isGameStart)
        {
            yield return new WaitForSeconds(0.1f);
            ClearTime += 0.1f;
        }
    }
}
