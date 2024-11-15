using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TargetState : MonoBehaviour
{
    public enum State
    {
        IDLE, DETECTION,FIRE
    }
    public State state = State.IDLE;
    private Transform TargetTr;
    private Transform PlayerTr;
    private TargetFire targetFire;

    private float Angle;
    private readonly float MaxDist = 20.0f;
    private readonly float MaxAngle = 22.5f;
    private readonly float MinAngle = -22.5f;
    private void Awake()
    {
        TargetTr = transform;
        PlayerTr = GameObject.Find("Player").transform;
        targetFire = transform.GetComponent<TargetFire>();
    }
    private void OnEnable()
    {
        StartCoroutine(UpdateState());
        StartCoroutine(StateAction());
    }

    private IEnumerator UpdateState()
    {
        yield return new WaitForSeconds(0.1f);

        while(GameManager.instance.isGameStart)
        {
            float dist = Vector3.Distance(PlayerTr.position,TargetTr.position);
            if(dist < MaxDist)
            {
                Vector3 ToPlayerVector = new Vector3(PlayerTr.position.x, 0f, PlayerTr.position.z) - new Vector3(TargetTr.position.x, 0f, TargetTr.position.z);
                Angle = Vector3.Angle(transform.forward, ToPlayerVector);

                if(MinAngle <= Angle && Angle <= MaxAngle)
                    state = State.FIRE;
                else
                    state = State.DETECTION;
            }
            else
                state = State.IDLE;

            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
    }

    private IEnumerator StateAction()
    {
        yield return new WaitForSeconds(0.1f);

        while(GameManager.instance.isGameStart)
        {
            switch(state)
            {
                case State.IDLE:
                targetFire.IsDetection = false;
                targetFire.IsFire = false;
                break;

                case State.DETECTION:
                targetFire.IsDetection = true;
                targetFire.IsFire = false;
                break;

                case State.FIRE:
                targetFire.IsDetection = true;
                targetFire.IsFire = true;
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
