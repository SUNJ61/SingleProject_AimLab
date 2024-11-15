using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFire : MonoBehaviour
{
    private Transform PlayerTr;
    private Coroutine FireCoroutine;
    private RaycastHit hit;

    private bool isDetection;
    public bool IsDetection
    {
        get { return isDetection; }
        set { isDetection = value; }
    }
    private bool isFire;
    public bool IsFire
    {
        get { return isFire; }
        set
        {
            isFire = value;
            if (isFire)
                FireCoroutine = StartCoroutine(EnemyFire());
            else if(!isFire && FireCoroutine != null)
                StopCoroutine(FireCoroutine);
        }
    }

    private void Awake()
    {
        PlayerTr = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if(IsDetection)
        {
            Vector3 ToPlayerVector = new Vector3(PlayerTr.position.x, 0f, PlayerTr.position.z) - new Vector3(transform.position.x, 0f, transform.position.z);
            Quaternion ToPlayerAngle = Quaternion.LookRotation(ToPlayerVector);

            transform.rotation = Quaternion.Slerp(transform.rotation, ToPlayerAngle, Time.deltaTime);
        }
    }

   private IEnumerator EnemyFire()
   {
        yield return new WaitForSeconds(0.4f);
        while(IsFire)
        {
            //총쏘기 구현
            yield return new WaitForSeconds(0.2f); //0.2초 마다 발사
        }
   }
}
