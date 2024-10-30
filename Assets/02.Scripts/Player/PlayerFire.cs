using Cinemachine;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Transform CameraTr;
    private CinemachineVirtualCamera PlayerCamera;
    private CinemachineBasicMultiChannelPerlin PlayerNoise;
    private PlayerMove playerMove;

    private Ray ray;
    private RaycastHit hit;

    private float prevTime;
    [SerializeField]private float verticalGunRebound; //���� �ݵ�
    public float VerticalGunRebound
    {
        get { return verticalGunRebound; }
        set { verticalGunRebound = value; }
    }
    [SerializeField] private float horizontalGunRebound; //���� �ݵ�
    public float HorizontalGunRebound
    {
        get { return horizontalGunRebound; }
        set { horizontalGunRebound = value; }
    }
    [SerializeField] private float delay = 0.2f; //�ѱ� ������
    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }

    [SerializeField] private int[] fireBranch;
    public int[] FireBranch
    {
        get { return fireBranch; }
        set { fireBranch = value; }
    }

    private int ShootCount = 0;
    private int fireState; //�߻� ��� 0 : ����, 1 : �ܹ�
    public int FireState
    {
        get { return fireState; }
        set { fireState = value; }
    }
    private int zoomState; //�� ��� 0 : ����, 1 : ������
    public int ZoomState
    {
        get { return zoomState; }
        set 
        {
            zoomState = value;
            ZoomAction();
        }
    }

    private bool isFire; //�߻� ����
    public bool IsFire
    {
        get { return isFire; }
        set 
        {
            isFire = value;
            if (!IsFire)
            {
                ShootCount = 0; //�� ��Ⱑ ������ �� ���� ī��Ʈ �ʱ�ȭ.
                playerMove.ExitGunRebound(); //�� ��Ⱑ ������ �� �¿� �ݵ����� ���� ī�޶� �¿� ȸ�� �ʱ�ȭ.
                playerMove.ApplyVerticalReBound(0f); //�� ��Ⱑ ������ �� �ݵ� �ʱ�ȭ
                playerMove.ApplyHorizontalReBound(0f);
            }
        }
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
        if (!IsFire) return;

        FireAction();
    }

    private void FireAction()
    {
        if (Physics.Raycast(ray, out hit, 110.0f))
        {
            switch(FireState)
            {
                case 0:
                    if (Time.time - prevTime > Delay)
                    {
                        FireSpray(FireBranch);
                        prevTime = Time.time;
                    }
                    else //���� ���� ���� �� �ݵ� ����
                    {
                        playerMove.ApplyVerticalReBound(0f);
                        playerMove.ApplyHorizontalReBound(0f);
                    }
                    break;

                case 1:
                    if (Time.time - prevTime > Delay * 1.5f)
                    {
                        playerMove.ApplyVerticalReBound(VerticalGunRebound);
                        prevTime = Time.time;
                        StartCoroutine(FireFalse());
                    }
                    else //���� ���� ���� �� �ݵ� ����
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
        switch (ZoomState)
        {
            case 0: //���� ��� ����
                PlayerCamera.m_Lens.FieldOfView = 60.0f;
                break;

            case 1: //������ ��� ����
                PlayerCamera.m_Lens.FieldOfView = 50.0f;
                break;
        }
    }

    private void FireSpray(int[] branch) //�ѱ� �������� ���� �Լ�.
    {
        ShootCount++;

        if(ShootCount <= branch[0]) //�������� ù��° �б�
        {
            playerMove.ApplyVerticalReBound(VerticalGunRebound);
            playerMove.ApplyHorizontalReBound(0f);
        }
        else if(ShootCount <= branch[1]) //�������� �ι�° �б�
        {
            playerMove.ApplyVerticalReBound(0f);
            playerMove.ApplyHorizontalReBound(HorizontalGunRebound);
        }
        else //�������� ����° �б� (���⼭ �ݺ�)
        {
            if(ShootCount == branch[2]) //ī��Ʈ �ʱ�ȭ
                ShootCount = branch[1] + 1;

            if (ShootCount == branch[1] + 1) //���� ��ȯ
                HorizontalGunRebound *= -1;

            playerMove.ApplyVerticalReBound(0f);
            playerMove.ApplyHorizontalReBound(HorizontalGunRebound);
        }
    }

    IEnumerator FireFalse()
    {
        yield return new WaitForSeconds(0.2f);
        IsFire = false;
    }
}
