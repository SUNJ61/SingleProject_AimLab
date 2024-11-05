using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSys : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerFire playerFire;
    private Inventory playerInventory;
    private PlayerGetItem playerGetItem;

    private PlayerInput playerInput;
    private InputActionMap playerActionMap;

    private int[] fireMode;
    public int[] FireMode
    {
        get { return fireMode; }
        set
        {
            fireMode = value;
            FireState = FireMode[idx]; //총을 처음 주웠을 때 기본 세팅
        }
    }

    private int fireState;
    public int FireState
    {
        get { return fireState; }
        set
        {
            fireState = value;
            playerFire.FireState = FireState;
        }
    }
    private int idx = 0;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerFire = GetComponent<PlayerFire>();
        playerInput = GetComponent<PlayerInput>();
        playerInventory = GetComponent<Inventory>();
        playerGetItem = GetComponent<PlayerGetItem>();
    }

    private void OnEnable()
    {
        playerActionMap = playerInput.actions.FindActionMap("Player");

        playerActionMap.FindAction("Move").performed += OnMovePerformed; //performed는 누를 때 값 전달. (누르는 값의 변화가 생기면 계속 업데이트 -> 여러개의 키가 동시에 입력되는 걸 관리에 적합?)
        playerActionMap.FindAction("Move").canceled += OnMoveCanceled; //canceled는 누르는 것이 종료되면 값을 전달한다.
        playerActionMap.FindAction("Look").performed += OnLookPerformed;
        playerActionMap.FindAction("Look").canceled += OnLookCanceled;
        playerActionMap.FindAction("SlowWalk").started += OnSlowWalkStarted; //started는 누를 때 값 전달. (누르는 값이 변화 하더라도 업데이트 x)
        playerActionMap.FindAction("SlowWalk").canceled += OnSlowWalkCanceled;
        playerActionMap.FindAction("Jump").started += OnJumpStarted;
        playerActionMap.FindAction("Jump").canceled += OnJumpCanceled;
        playerActionMap.FindAction("Fire").started += OnFireStarted;
        playerActionMap.FindAction("Fire").canceled += OnFireCanceled;
        playerActionMap.FindAction("Zoom").started += OnZoomStarted;
        playerActionMap.FindAction("Zoom").canceled += OnZoomCanceled;
        playerActionMap.FindAction("Reload").started += OnReloadStarted;
        playerActionMap.FindAction("Mode").started += OnModeStarted;
        playerActionMap.FindAction("Action").started += OnActionStarted;
        playerActionMap.FindAction("Action").canceled += OnActionCanceled;
        playerActionMap.FindAction("Drop").started += OnDropStarted;
        playerActionMap.FindAction("Inventory").started += OnInventoryStarted;
        playerActionMap.FindAction("Shop").started += OnShopStarted;
        playerActionMap.FindAction("Escape").started += OnEscapeStarted;
    }

    private void SendInventoryIdx(int idx)
    {
        playerInventory.SlotIdx = idx; //슬롯 인덱스 전달.
    }

    private void OnMovePerformed(InputAction.CallbackContext context) //플레이어 이동
    {
        Vector2 dir = context.ReadValue<Vector2>();
        playerMove.PlayerDir = new Vector3(dir.x, 0f, dir.y).normalized;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) //플레이어 이동 멈춤
    {
        playerMove.PlayerDir = Vector3.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext context) //마우스 이동
    {
        playerMove.PlayerRot = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context) //마우스 이동 멈춤
    {
        playerMove.PlayerRot = Vector2.zero;
    }

    private void OnSlowWalkStarted(InputAction.CallbackContext context) //왼쪽 쉬프트 누름
    {
        playerMove.Player_MoveState = true;
    }

    private void OnSlowWalkCanceled(InputAction.CallbackContext context) //왼쪽 쉬프트 땜
    {
        playerMove.Player_MoveState = false;
    }

    private void OnJumpStarted(InputAction.CallbackContext context) //점프 누름
    {
        playerMove.Player_IsJump = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context) //점프 키 땜
    {
        playerMove.Player_IsJump = false;
    }

    private void OnFireStarted(InputAction.CallbackContext context) //발사 키 누름
    {
        playerFire.IsFire = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context) //발사 키 땜
    {
        playerFire.IsFire = false;
    }

    private void OnZoomStarted(InputAction.CallbackContext context) //줌 키 누름
    {
        playerFire.ZoomState = 1;
    }

    private void OnZoomCanceled(InputAction.CallbackContext context) //줌 키 땜
    {
        playerFire.ZoomState = 0;
    }

    private void OnReloadStarted(InputAction.CallbackContext context) // 장전 키 누름 (잠깐 동안만 활성화 되면됨. 코루틴 사용)
    {

    }

    private void OnModeStarted(InputAction.CallbackContext context) // 총 모드 변경 키 누름
    {
        idx = (idx +1) % FireMode.Length;
        FireState = FireMode[idx];
    }

    private void OnActionStarted(InputAction.CallbackContext context) // 상호작용 키 누름
    {
        playerGetItem.IsActive = true;
    }

    private void OnActionCanceled(InputAction.CallbackContext context) // 상호작용 키 땜
    {
        playerGetItem.IsActive = false;
    }

    private void OnDropStarted(InputAction.CallbackContext context) // 버리기 키 누름 (잠깐 동안만 활성화 되면됨.)
    {

    }

    private void OnInventoryStarted(InputAction.CallbackContext context) // 퀵 슬롯 키 누름 -> 이거 started하나면 될듯?
    {
        var key = Keyboard.current;

        if (key.digit1Key.wasPressedThisFrame)
            SendInventoryIdx(0);
        else if (key.digit2Key.wasPressedThisFrame)
            SendInventoryIdx(1);
        else if (key.digit3Key.wasPressedThisFrame)
            SendInventoryIdx(2);
        else if (key.digit4Key.wasPressedThisFrame)
            SendInventoryIdx(3);
    }

    private void OnShopStarted(InputAction.CallbackContext context) // 상점 열기 키 누름 -> 토글로 만들어야해서 started하나면 될듯?
    {

    }

    private void OnEscapeStarted(InputAction.CallbackContext context) // 닫기 키 누름 -> 잠깐 동안만 활성화 되면됨.
    {

    }
}
