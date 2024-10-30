using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSys : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerFire playerFire;

    private PlayerInput playerInput;
    private InputActionMap playerActionMap;

    private int[] fireMode;
    public int[] FireMode
    {
        get { return fireMode; }
        set
        {
            fireMode = value;
            FireState = FireMode[0]; //���� ó�� �ֿ��� �� �⺻ ����
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
    private int idx;
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerFire = GetComponent<PlayerFire>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerActionMap = playerInput.actions.FindActionMap("Player");

        playerActionMap.FindAction("Move").performed += OnMovePerformed; //performed�� ���� �� �� ����. (������ ���� ��ȭ�� ����� ��� ������Ʈ -> �������� Ű�� ���ÿ� �ԷµǴ� �� ������ ����?)
        playerActionMap.FindAction("Move").canceled += OnMoveCanceled; //canceled�� ������ ���� ����Ǹ� ���� �����Ѵ�.
        playerActionMap.FindAction("Look").performed += OnLookPerformed;
        playerActionMap.FindAction("Look").canceled += OnLookCanceled;
        playerActionMap.FindAction("SlowWalk").started += OnSlowWalkStarted; //started�� ���� �� �� ����. (������ ���� ��ȭ �ϴ��� ������Ʈ x)
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
        playerActionMap.FindAction("Drop").started += OnDropStarted;
        playerActionMap.FindAction("Inventory").started += OnInventoryStarted;
        playerActionMap.FindAction("Shop").started += OnShopStarted;
        playerActionMap.FindAction("Escape").started += OnEscapeStarted;
    }

    private void SendInventoryIdx(int idx)
    {
        //���� Ű ���� ���� �� ü������ ���� 
    }

    private void OnMovePerformed(InputAction.CallbackContext context) //�÷��̾� �̵�
    {
        Vector2 dir = context.ReadValue<Vector2>();
        playerMove.PlayerDir = new Vector3(dir.x, 0f, dir.y).normalized;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) //�÷��̾� �̵� ����
    {
        playerMove.PlayerDir = Vector3.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext context) //���콺 �̵�
    {
        playerMove.PlayerRot = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context) //���콺 �̵� ����
    {
        playerMove.PlayerRot = Vector2.zero;
    }

    private void OnSlowWalkStarted(InputAction.CallbackContext context) //���� ����Ʈ ����
    {
        playerMove.Player_MoveState = true;
    }

    private void OnSlowWalkCanceled(InputAction.CallbackContext context) //���� ����Ʈ ��
    {
        playerMove.Player_MoveState = false;
    }

    private void OnJumpStarted(InputAction.CallbackContext context) //���� ����
    {
        playerMove.Player_IsJump = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context) //���� Ű ��
    {
        playerMove.Player_IsJump = false;
    }

    private void OnFireStarted(InputAction.CallbackContext context) //�߻� Ű ����
    {
        playerFire.IsFire = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context) //�߻� Ű ��
    {
        playerFire.IsFire = false;
    }

    private void OnZoomStarted(InputAction.CallbackContext context) //�� Ű ����
    {
        playerFire.ZoomState = 1;
    }

    private void OnZoomCanceled(InputAction.CallbackContext context) //�� Ű ��
    {
        playerFire.ZoomState = 0;
    }

    private void OnReloadStarted(InputAction.CallbackContext context) // ���� Ű ���� (��� ���ȸ� Ȱ��ȭ �Ǹ��. �ڷ�ƾ ���)
    {

    }

    private void OnModeStarted(InputAction.CallbackContext context) // �� ��� ���� Ű ����
    {
        FireState = (FireMode[0] +1) % FireMode.Length;
        
    }

    private void OnActionStarted(InputAction.CallbackContext context) // ��ȣ�ۿ� Ű ���� (��� ���ȸ� Ȱ��ȭ �Ǹ��.)
    {

    }

    private void OnDropStarted(InputAction.CallbackContext context) // ������ Ű ���� (��� ���ȸ� Ȱ��ȭ �Ǹ��.)
    {

    }

    private void OnInventoryStarted(InputAction.CallbackContext context) // �� ���� Ű ���� -> �̰� started�ϳ��� �ɵ�?
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

    private void OnShopStarted(InputAction.CallbackContext context) // ���� ���� Ű ���� -> ��۷� �������ؼ� started�ϳ��� �ɵ�?
    {

    }

    private void OnEscapeStarted(InputAction.CallbackContext context) // �ݱ� Ű ���� -> ��� ���ȸ� Ȱ��ȭ �Ǹ��.
    {

    }
}
