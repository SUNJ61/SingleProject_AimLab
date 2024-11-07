using Cinemachine;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Transform CameraPivot;
    private CinemachineVirtualCamera PlayerCamera;
    private PlayerMove playerMove;
    private GunData gunData;
    private GameObject gunObj;
    public GameObject GunObj
    {
        get { return gunObj; }
        set
        {
            gunObj = value;
            gunData = GunObj.GetComponent<Gun>().gundata;
            FireState = gunData.FireMode[0];
            HorizontalGunRebound = gunData.HorizontalReBound;
            MaxBullet = gunData.BulletMax;
        }
    }
    private RaycastHit hit;

    private float prevTime;
    private float HorizontalGunRebound = 0;

    private int ShootCount = 0;
    private int MaxBullet = 0;
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

    bool isFire; //발사 유무
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
    bool canFire = false;
    public bool CanFire
    {
        get { return canFire; }
        set { canFire = value; }
    }
    [SerializeField] bool isReload;
    public bool IsReload
    {
        get { return isReload; }
        set
        {
            isReload = value;
            if(IsReload)
                ReloadAction();
        }
    }

    private void Awake()
    {
        CameraPivot = transform.GetChild(0).transform;
        PlayerCamera = CameraPivot.GetChild(0).transform.GetComponent<CinemachineVirtualCamera>();
        playerMove = GetComponent<PlayerMove>();

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

        switch (FireState)
        {
            case 0: //연발
                if (Time.time - prevTime > gunData.Delay && gunData.BulletMax != 0)
                {
                    if (Physics.Raycast(CameraPivot.position, CameraPivot.forward, out hit, 110.0f, 1 << 5) && !GameManager.instance.isGameStart)
                    {
                        InGameUIManager.instance.RandomShootGameUI(hit);
                    }
                    if(Physics.Raycast(CameraPivot.position, CameraPivot.forward, out hit, 110.0f, 1 << 7)) //타겟에 맞았을 때
                    {
                        hit.transform.gameObject.SendMessage("HitDamage", 100); //나중에 적 npc제데로 만들면 수치 고치기.
                    }
                    FireSpray(gunData.FireBranch);
                    gunData.BulletMax -= 1;
                    prevTime = Time.time;
                }
                else //총을 쏘지 않을 때 반동 제거
                {
                    playerMove.ApplyVerticalReBound(0f);
                    playerMove.ApplyHorizontalReBound(0f);
                }
                break;

            case 1: //단발
                if (Time.time - prevTime > gunData.Delay * 1.5f && gunData.BulletMax != 0)
                {
                    if (Physics.Raycast(CameraPivot.position, CameraPivot.forward, out hit, 110.0f, 1 << 5))
                    {
                        InGameUIManager.instance.RandomShootGameUI(hit);
                    }
                    playerMove.ApplyVerticalReBound(gunData.VerticalReBound);
                    gunData.BulletMax -= 1;
                    prevTime = Time.time;
                    StartCoroutine(FireFalse(gunData.Delay * 1.2f));
                }
                else //총을 쏘지 않을 때 반동 제거
                {
                    playerMove.ApplyVerticalReBound(0f);
                    playerMove.ApplyHorizontalReBound(0f);
                }
                break;
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
                    PlayerCamera.m_Lens.FieldOfView = gunData.Fov;
                    break;
            }
        }
        
    }

    private void ReloadAction() //장전 함수
    {
        if(gunData.BulletMax != MaxBullet && gunData.SubBullet != 0)
        {
            int loadBullet = MaxBullet - gunData.BulletMax;
            if(loadBullet > gunData.SubBullet)
            {
                gunData.BulletMax += gunData.SubBullet;
                gunData.SubBullet = 0;
            }
            else
            {
                gunData.BulletMax += loadBullet;
                gunData.SubBullet -= loadBullet;
            }
        }
        IsReload = false;
    }

    private void FireSpray(int[] branch) //총기 스프레이 적용 함수.
    {
        ShootCount++;

        if(ShootCount <= branch[0]) //스프레이 첫번째 분기
        {
            playerMove.ApplyVerticalReBound(gunData.VerticalReBound);
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
