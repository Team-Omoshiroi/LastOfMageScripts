using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController: MonoBehaviour
{
    public Slider HpBar;

    LeaveGame leaveGame;
    public  Button BtnLeavGame;
    public GameObject GameOver;

    public RectTransform UIRoot;
    public Button BtnInventory;
    public Button BtnCancel;


    public static UIController Instance { get; private set; }


    public GameObject InventoryUI;

   

    private void Awake()
    {
        Instance = this;
    } 


    private void Start()
    {
        init();

        leaveGame = GetComponent<LeaveGame>();

        BtnLeavGame.onClick.AddListener(() =>
        {

            SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-confirm-1", eSoundType.PlayerEffect);
            LoadingScenController.LoadScene("LobbyScene");
            //if (leaveGame)
            //{
            //    leaveGame.LeaveGameRoom();
            //}
            //else
            //{
            //    Debug.LogError("Component null");
            //}
        });
       
    }
    void init()
    {
        if (GameOver != null) // GameOver가 할당되었는지 확인
        {
            GameOver.SetActive(false);
        }
        else
        {
            Debug.LogError("NullGameOver");
        }
    }
    public void  UIDie()
    {
        GameOver.SetActive(true);
    }

    public  void HandlerHp(float MaxHp ,float CurHp)
    {
        Debug.Log(MaxHp);
        HpBar.value = Mathf.Lerp(HpBar.value, (float)CurHp / (float)MaxHp, Time.deltaTime * 10);
        Debug.Log(CurHp);
    }

    public void InitHpbar(float MaxHp, float CurHp)
    {

        HpBar.value = CurHp / MaxHp;
    }
    private void OnDestroy()
    {
        
        Instance = null;
    }
}
