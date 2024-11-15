using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BombState : MonoBehaviour
{
    public enum State
    {
        IDLE, A_PLANT, D_PLANT
    }
    public State state = State.IDLE;

    [SerializeField]private float BombCount = 0f;
    private float DefuseCount = 0f;

    private bool isPlant;
    public bool IsPlant
    {
        get { return isPlant; }
        set { isPlant = value; }
    }
    private bool canDefuse; //플랜트 장소에 있는가
    public bool CanDefuse
    {
        get { return canDefuse; }
        set { canDefuse = value; }
    }
    private bool isDefuse; //플레이어가 디퓨즈 버튼을 눌렀는가?
    public bool IsDefuse
    {
        get { return isDefuse; }
        set { isDefuse = value; }
    }

    private void OnEnable()
    {
        StartCoroutine(BombStateUpdate());
    }

    private void OnDisable()
    {
        state = State.IDLE;
        BombCount = 0f;
        DefuseCount = 0f;
    }

    private IEnumerator BombStateUpdate()
    {
        yield return new WaitForSeconds(0.1f);

        while (GameManager.instance.isGameStart)
        {
            if (GameManager.instance.AIGunMatchLevelIdx == 0)
                state = State.A_PLANT;
            else if (!IsPlant)
                state = State.IDLE;
            else if (IsPlant)
                state = State.D_PLANT;
        }
    }

    private IEnumerator StateAction()
    {
        yield return new WaitForSeconds(0.1f);

        while (GameManager.instance.isGameStart)
        {
            switch (state)
            {
                case State.IDLE:

                    break;

                case State.A_PLANT:
                    AddBombCount();
                    AddDefuseCount();
                    break;

                case State.D_PLANT:

                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void AddDefuseCount()
    {
        if ((state == State.A_PLANT || state == State.D_PLANT) && IsDefuse && CanDefuse)
        {
            DefuseCount += 0.1f;
            Debug.Log(DefuseCount);
            if (DefuseCount > 8.0f)
            {
                GameManager.instance.AIGunMatchSucess();
            }
        }
        else
        {
            if (DefuseCount < 4.0f)
                DefuseCount = 0;
            else if (DefuseCount < 8.0f)
                DefuseCount = 4.01f;
        }
    }

    private void AddBombCount()
    {
        BombCount += 0.1f;
        if (BombCount >= 240.0f)
            GameManager.instance.AIGunMatchFalse();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            CanDefuse = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            CanDefuse = false;
        }
    }
}
