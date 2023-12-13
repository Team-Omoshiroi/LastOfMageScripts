using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSceneController : MonoBehaviour
{
    [SerializeField] private Button BtnLobby;
    [SerializeField] private Button BtnOption;
    [SerializeField] private GameObject UIStoragePage;
    [SerializeField] private string sceneName;

    private void Awake()
    {
        init();
    }
    void Start()
    {
        BtnLobby.onClick.AddListener(() =>
        {
            SoundManager.Instance.Clear();
            LoadingScenController.LoadScene(sceneName);
        });
        BtnOption.onClick.AddListener(() =>
        {
        });

    }
    void init()
    {
        UIStoragePage.SetActive(true);    
    }

}
