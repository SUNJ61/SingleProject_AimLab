using UnityEngine;

public class GunChange : MonoBehaviour
{ //�ѱ��� ��ȭ�� ����� �����͸� ������Ʈ�ؼ� �ٸ� ��ũ��Ʈ�� �����͸� �ѷ��ִ� ����.
    [SerializeField] private GunData playerGunData;
    public GunData PlayerGunData
    { //�÷��̾ ���� �ݰų� �ٲٸ� ������Ʈ ���Ѿ��Ѵ�.
        get { return playerGunData; }
        set
        {
            playerGunData = value;
            UpdateGunData();
        }
    }

    private PlayerFire playerFire; //�ѱ� �߻� ������, �ݵ� �ʿ�.
    private PlayerInputSys playerInputSys;

    private void Awake()
    {
        playerFire = GetComponent<PlayerFire>();
        playerInputSys = GetComponent<PlayerInputSys>();
    }
    private void Start()
    {
        PlayerGunData = Resources.Load<GunData>("ARGunData");
    }

    private void UpdateGunData()
    {
        playerFire.Delay = PlayerGunData.Delay;
        playerFire.VerticalGunRebound = PlayerGunData.Vertical_ReBound;
        playerFire.HorizontalGunRebound = PlayerGunData.Horizontal_ReBound;
        playerFire.FireBranch = PlayerGunData.FireBranch;
        playerInputSys.FireMode = PlayerGunData.FireMode;
    }
}
