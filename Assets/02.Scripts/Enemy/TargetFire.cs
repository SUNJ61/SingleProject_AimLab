using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFire : MonoBehaviour
{
    private Transform PlayerTr;
    private Transform FirePos;
    private Animator animator;
    private RaycastHit hit;

    private float prevTime;
    private readonly float Delay = 0.2f;

    private int layer;

    private bool isDetection;
    public bool IsDetection
    {
        get { return isDetection; }
        set { isDetection = value; }
    }
    [SerializeField]private bool isFire;
    public bool IsFire
    {
        get { return isFire; }
        set { isFire = value; }
    }

    private void Awake()
    {
        PlayerTr = GameObject.Find("Player").transform;
        FirePos = transform.GetChild(4).transform;
        animator = GetComponent<Animator>();
        layer = 1 << 3 | 1 << 12;

        animator.SetBool("IsFire",false);
        prevTime = Time.time;
    }

    private void Update()
    {
        Debug.DrawRay(FirePos.position, FirePos.forward * 30f, Color.red);
        if(IsDetection)
        {
            Vector3 ToPlayerVector = new Vector3(PlayerTr.position.x, 0f, PlayerTr.position.z) - new Vector3(transform.position.x, 0f, transform.position.z);
            Quaternion ToPlayerAngle = Quaternion.LookRotation(ToPlayerVector);

            transform.rotation = Quaternion.Slerp(transform.rotation, ToPlayerAngle, Time.deltaTime);
        }
        if(IsFire && Time.time - prevTime > Delay)
        {
            if(Physics.Raycast(FirePos.position, FirePos.forward, out hit, 30.0f, layer))
            {
                if(hit.collider.gameObject.layer != 3)
                    animator.SetBool("IsFire", false);
                else
                {
                    animator.SetBool("IsFire", true);
                    hit.collider.gameObject.SendMessage("HitDamage", 22);
                }
            }
            prevTime = Time.time;
        }
    }
}
