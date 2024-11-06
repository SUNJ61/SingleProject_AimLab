using Cinemachine;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Transform CameraTr;
    private CinemachineVirtualCamera PlayerCamera;
    private PlayerMove playerMove;

    private Ray ray;
    private RaycastHit hit;

    private float prevTime;
    [SerializeField]private float verticalGunRebound; //수직 반동
    public float VerticalGunRebound
    {
        get { return verticalGunRebound; }
        set { verticalGunRebound = value; }
    }
    [SerializeField] private float horizontalGunRebound; //수평 반동
    public float HorizontalGunRebound
    {
        get { return horizontalGunRebound; }
        set { horizontalGunRebound = value; }
    }
    [SerializeField] private float delay = 0.2f; //총기 딜레이
    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }
    [SerializeField] private float fov; //확대 배율
    public float Fov
    {
        get { return fov; }
        set { fov = value; }
    }

    [SerializeField] private int[] fireBranch;
    public int[] FireBranch
    {
        get { return fireBranch; }
        set { fireBranch = value; }
    }

    private int ShootCount = 0;
    private int fireState; //발사 모드 0 : 연사, 1 : 단발
    public int FireState
    {
        get { return fireState; }
        set { fireState = value; }
    }
    private int zoomState; //줌 모드 0 : 견착, 1 : 스코프
    public int ZoomState
    {
        get { return zoomState; }
        set 
        {
            zoomState = value;
            ZoomAction();
        }
    }

    [SerializeField] bool isFire; //발사 유무
    public bool IsFire
    {
        get { return isFire; }
        set 
        {
            isFire = value;
            if (!IsFire)
            {
                ShootCount = 0; //총 쏘기가 끝났을 때 연사 카운트 초기화.
                playerMove.ExitGunRebound(); //총 쏘기가 끝났을 때 좌우 반동으로 생긴 카메라 좌우 회전 초기화.
                playerMove.ApplyVerticalReBound(0f); //총 쏘기가 끝났을 때 반동 초기화
                playerMove.ApplyHorizontalReBound(0f);
            }
        }
    }
    [SerializeField] bool canFire = false;
    public bool CanFire
    {
        get { return canFire; }
        set { canFire = value; }
    }

    private void Awake()
    {
        CameraTr = transform.GetChild(0).transform;
        PlayerCamera = CameraTr.GetChild(0).transform.GetComponent<CinemachineVirtualCamera>();
        playerMove = GetComponent<PlayerMove>();

        ray = new Ray(CameraTr.position, Vector3.forward);
        prevTime = Time.time;
    }

    private void Update()
    {
        if(!isFire) return;

        if (CanFire)
        {
            FireAction();
        }
    }

    private void FireAction()
    {
        if (Physics.Raycast(ray, out hit, 110.0f))
        {
            switch(FireState)
            {
                case 0: //연발
                    if (Time.time - prevTime > Delay)
                    {
                        FireSpray(FireBranch);
                        prevTime = Time.time;
                    }
                    else //총을 쏘지 않을 때 반동 제거
                    {
                        playerMove.ApplyVerticalReBound(0f);
                        playerMove.ApplyHorizontalReBound(0f);
                    }
                    break;

                case 1: //단발
                    if (Time.time - prevTime > Delay * 1.5f)
                    {
                        playerMove.ApplyVerticalReBound(VerticalGunRebound);
                        prevTime = Time.time;
                        StartCoroutine(FireFalse(Delay * 1.2f));
                    }
                    else //총을 쏘지 않을 때 반동 제거
                    {
                        playerMove.ApplyVerticalReBound(0f);
                        playerMove.ApplyHorizontalReBound(0f);
                    }
                    break;
            }
        }
    }

    private void ZoomAction()
    {
        if(CanFire)
        {
            switch (ZoomState)
            {
                case 0: //견착 모드 구현
                    PlayerCamera.m_Lens.FieldOfView = 60.0f;
                    break;

                case 1: //스코프 모드 구현
                    PlayerCamera.m_Lens.FieldOfView = Fov;
                    break;
            }
        }
        
    }

    private void FireSpray(int[] branch) //총기 스프레이 적용 함수.
    {
        ShootCount++;

        if(ShootCount <= branch[0]) //스프레이 첫번째 분기
        {
            playerMove.ApplyVerticalReBound(VerticalGunRebound);
            playerMove.ApplyHorizontalReBound(0f);
        }
        else if(ShootCount <= branch[1]) //스프레이 두번째 분기
        {
            playerMove.ApplyVerticalReBound(0f);
            playerMove.ApplyHorizontalReBound(HorizontalGunRebound);
        }
        else //스프레이 세번째 분기 (여기서 반복)
        {
            if(ShootCount == branch[2]) //카운트 초기화
                ShootCount = branch[1] + 1;

            if (ShootCount == branch[1] + 1) //방향 전환
                HorizontalGunRebound *= -1;

            playerMove.ApplyVerticalReBound(0f);
            playerMove.ApplyHorizontalReBound(HorizontalGunRebound);
        }
    }

    IEnumerator FireFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsFire = false;
    }
}
