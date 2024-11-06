using UnityEngine;

public class GunChange : MonoBehaviour
{ //총기의 변화가 생기면 데이터를 업데이트해서 다른 스크립트로 데이터를 뿌려주는 역할.
    [SerializeField] private GunData playerGunData;
    public GunData PlayerGunData
    { //플레이어가 총을 줍거나 바꾸면 업데이트 시켜야한다.
        get { return playerGunData; }
        set
        {
            playerGunData = value;
            UpdateGunData();
        }
    }

    private PlayerFire playerFire; //총기 발사 딜레이, 반동 필요.
    private PlayerInputSys playerInputSys;

    private void Awake()
    {
        playerFire = GetComponent<PlayerFire>();
        playerInputSys = GetComponent<PlayerInputSys>();
    }

    private void UpdateGunData()
    {
        playerFire.Delay = PlayerGunData.Delay;
        playerFire.VerticalGunRebound = PlayerGunData.Vertical_ReBound;
        playerFire.HorizontalGunRebound = PlayerGunData.Horizontal_ReBound;
        playerFire.FireBranch = PlayerGunData.FireBranch;
        playerFire.Fov = PlayerGunData.Fov;
        playerInputSys.FireMode = PlayerGunData.FireMode;
    }
}
