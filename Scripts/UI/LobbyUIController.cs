using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public GameObject lobbyLoading;
    public GameObject[] UIPage; // UI 페이지 배열
    delegate void UIAction();



    private void Start()
    {
        Init();
    }


    private void Init()
    {
        SetActiveWithCheck(UIPage[0], true);
        for (int i = 1; i < UIPage.Length; i++)
        {
            SetActiveWithCheck(UIPage[i], false);
           
        }
    }

    public void ChangePage(int numBtn)
    {
        if (numBtn == 0)
        {
            SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-cancel-1",eSoundType.PlayerEffect);
        }
        else
        {
            SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-confirm-1", eSoundType.PlayerEffect);
        }
        for (int i = 0; i < UIPage.Length; i++)
        {         
            MoveUIPage(numBtn, i);           
        }

    }

    // 특정 페이지로 이동하는 메소드
    private void MoveUIPage(int numOpenPage,int numClosePage)
    {
        ToggleUI(() => ChangeUIState(UIPage[numOpenPage], UIPage[numClosePage])); 
     
      
    }

    // 로딩 화면을 토글하고 UI 액션 수행
    private void ToggleUI(UIAction action)
    {
        SetActiveWithCheck(lobbyLoading, true);
        StartCoroutine(PerformUIAction(action));
    }

    // 지정된 UI 작업 수행
    private IEnumerator PerformUIAction(UIAction action)
    {
        yield return new WaitForSeconds(1.5f);
        action?.Invoke();
        yield return new WaitForSeconds(1.5f);
        SetActiveWithCheck(lobbyLoading, false);
    }

    // 지정된 UI를 보여주고 숨기는 메소드
    private void ChangeUIState(GameObject uiToShow, GameObject uiToHide)
    {
        SetActiveWithCheck(uiToHide, false);
        SetActiveWithCheck(uiToShow, true);
    
    }

    // 안전하게 GameObject의 활성화/비활성화 상태 설정
    private void SetActiveWithCheck(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }


    public void LoadBTG()
    {
        LoadingScenController.LoadScene("BattleGroundScene");
        SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-confirm-1", eSoundType.PlayerEffect);
    }
}





    //void Start()
    //{
    //    btnStore.onClick.AddListener(() =>
    //    {
    //        StartUILoading(uiStore, uiMainLobby); 
    //    });
    //}

    //public void OpenOption()
    //{
    //    var ui = UIManager.Instance.ShowUI<UIOption>();
    //}

    //public void StartUILoading(GameObject openUI, GameObject closeUI) 
    //{
    //    if (lobbyLoading != null)
    //    {
    //        lobbyLoading.SetActive(true);
    //        StartCoroutine(OpenUI(openUI, closeUI));
    //    }
    //}

    //public IEnumerator OpenUI(GameObject openUI, GameObject closeUI)
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    if (openUI != null) openUI.SetActive(true);
    //    if (closeUI != null) closeUI.SetActive(false);
    //    yield return new WaitForSeconds(1);
    //    if (lobbyLoading != null) lobbyLoading.SetActive(false);
    //}
