using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController Player_Controller;
    private Transform Camera_Pivot; //카메라 x축 회전
    private Transform FirePos_Pivot; //FireFos x축 회전
    private Transform Player_Transform;
    private AudioSource PlayerSource;
    private AudioClip Walk;
    private AudioClip Run;

    private Vector3 Velocity_y = Vector3.zero; //중력

    private float moveSpeed = 6.0f;
    private float currentXRot = 0f;
    private float currentYRot = 0f;
    private float VerticalReboundAmount;
    private float HorizontalReboundAmount;

    private readonly float rotSpeed = 10.0f; //감도
    private readonly float Gravity = -25f;
    private readonly float JumpHeight = 0.4f;

    private Vector3 playerDir = Vector3.zero; //이동 방향
    public Vector3 PlayerDir
    { 
        get { return playerDir; } 
        set
        {
            playerDir = value;
        } 
    }

    private Vector2 playerRot = Vector2.zero; //회전 방향
    public Vector2 PlayerRot
    {
        get { return playerRot; }
        set
        {
            playerRot = value;
        }
    }

    private bool player_MoveState; //느리게 걷기 실행 체크
    public bool Player_MoveState
    {
        get { return player_MoveState; }
        set
        {
            player_MoveState = value;
        }
    }

    private bool Player_isJump;
    public bool Player_IsJump
    {
        get { return Player_isJump; }
        set
        {
            Player_isJump = value;
        }
    }

    private bool isGround;
    public bool IsGround
    {
        get { return isGround; }
        set { isGround = value; }
    }

    void Awake()
    {
        Player_Transform = transform;
        Player_Controller = GetComponent<CharacterController>();
        Camera_Pivot = transform.GetChild(0).GetComponent<Transform>();

        Walk = Resources.Load<AudioClip>("Sound/Player/Walk");
    }

    private void Start()
    {
        //InGameSoundManager.instance.ActiveSound(gameObject, Walk, 2.0f, false, true, true, 1);
        //PlayerSource = InGameSoundManager.instance.Data["Walk"].SoundBox_AudioSource;
    }

    private void Update()
    {
        CheckJumpState();
        CheckMoveState();
        Player_Moving();
        Camera_Moving();
    }

    private void Player_Moving()
    {
        if (!GameManager.instance.isTeleport)
        {
            Vector3 move = transform.TransformDirection(PlayerDir);
            Player_Controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }

    private void CheckMoveState()
    {
        if (PlayerDir == Vector3.zero || !IsGround) //멈춰있을 때와 점프시 소리 멈춤
        {
            //PlayerSource.Stop();
        }

        if (Player_MoveState) //느리게 걷기
        {
            moveSpeed = 3.0f;
            //if (PlayerSource.clip != Walk || !PlayerSource.isPlaying)
            //{
            //    PlayerSource.Stop();
            //}
        }
        else if (!Player_MoveState) // 걷기
        {
            moveSpeed = 6.0f;
            //if (PlayerSource.clip != Run || !PlayerSource.isPlaying)
            //{
            //    PlayerSource.Play();
            //}
        }
    }

    private void CheckJumpState()
    {
        if (IsGround)
        {
            Velocity_y.y = 0;
            if (Player_IsJump)
            {
                Velocity_y.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        else
            Velocity_y.y += Gravity * Time.deltaTime;

        Player_Controller.Move(Velocity_y * Time.deltaTime);
    }
    private void Camera_Moving()
    {
        Player_Transform.Rotate(Vector3.up * PlayerRot.x * rotSpeed * Time.deltaTime);

        currentXRot -= playerRot.y * rotSpeed * Time.deltaTime;

        currentXRot = Mathf.Clamp(currentXRot - VerticalReboundAmount, -45f, 45f);
        currentYRot = Mathf.Clamp(currentYRot - HorizontalReboundAmount, -10.0f, 10.0f);

        Camera_Pivot.localRotation = Quaternion.Euler(currentXRot, currentYRot, 0f);
    }

    public void ApplyVerticalReBound(float amount)
    {
        VerticalReboundAmount = amount;
    }

    public void ApplyHorizontalReBound(float amount)
    {
        HorizontalReboundAmount = amount;
    }

    public void ExitGunRebound()
    {
        currentYRot = 0f;
    }
}
