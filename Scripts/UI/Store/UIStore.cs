using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{
    [SerializeField] private UIStorePopup GhachaPopup;
    [SerializeField] private RectTransform PopupRoot;
  

    [SerializeField] private Button BtnWeapone1Ghacha;
    [SerializeField] private Button BtnWeapone10Ghacha;
    [SerializeField] private Button BtnMagic1Ghacha;
    [SerializeField] private Button BtnMagic10Ghacha;
    [SerializeField] private Button BtnRune1Ghacha;
    [SerializeField] private Button BtnRune10Ghacha;


    void Start()
    {
        GhachaPopup = UIManager.Instance.ShowUI<UIStorePopup>("Lobby/PopupConfirm", PopupRoot);
        GhachaPopup.gameObject.SetActive(false);
    

        BtnWeapone1Ghacha.onClick.AddListener(OpenPopup_Weapone1Gahcha);
    }

 
    void OpenPopup_Weapone1Gahcha()
    {
        GhachaPopup.gameObject.SetActive(true);
        GhachaPopup.SetUP( "100 골드 로 1회 소환하시겠습니다?", WeapneGahcha);
    }

    void  WeapneGahcha()
    {
       //todo
    }
}
