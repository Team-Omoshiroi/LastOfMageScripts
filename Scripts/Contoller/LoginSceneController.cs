using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneController : MonoBehaviour
{
    private bool IsLoginSucceed;

    // 추후에 게스트 로그인 기능 구현 후 삭제 예정
    [SerializeField] private Button buttonBae;
    [SerializeField] private Button buttonIm;
    [SerializeField] private Button buttonSong;
    [SerializeField] private Button buttonJo;
    [SerializeField] private Button buttonChae;

    [SerializeField] private Button BtnChangeScene;

    [SerializeField] private string BGM;
    private void Start()
    {
        SetTempGuestLogin();
        SoundManager.Instance.Play(BGM);
    }

    private void SetTempGuestLogin()
    {
        buttonBae.onClick.AddListener(() => { RequestPlayerInfo("bae_123", 2); });
        buttonIm.onClick.AddListener(() => { RequestPlayerInfo("im_123", 0); });
        buttonSong.onClick.AddListener(() => { RequestPlayerInfo("song_123", 0); });
        buttonJo.onClick.AddListener(() => { RequestPlayerInfo("jo_123", 0); });
        buttonChae.onClick.AddListener(() => { RequestPlayerInfo("chae_123", 0); });

        BtnChangeScene.onClick.AddListener(() => { LoadNextScene(); });
    }

    private void LoadNextScene()
    {
        LoadingScenController.LoadScene("LobbyScene");
    }

    private void RequestPlayerInfo(string playerName, int playerId)
    {
        PlayerLoginReq req = new() { PlayerName = playerName };
        PlayerLoginRes newRes = null;

        WebManager.Instance.SendPostRequest<PlayerLoginRes>("player/login", req, res =>
        {
            if (res != null)
            {
                newRes = res;

                DataManager.Instance.PlayerId = res.PlayerId;

                IsLoginSucceed = res.IsLoginSucceed;

                // SetIntialPlayerStats를 여기서 호출
                SetIntialPlayerStats(playerId);
            }
            else
            {
                Debug.LogError("PlayerLoginRes 객체가 null입니다.");
            }
        });
    }

    private void SetIntialPlayerStats(int playerId)
    {
        GetPlayerStatReq req = new() { PlayerId = playerId };
        PlayerStatRes newRes = null;

        WebManager.Instance.SendGetRequest<PlayerStatRes>("player/stat", req, res =>
        {
            if (res != null)
            {
                newRes = res;
                Debug.Log($"{res}");
            }
            else
            {
                Debug.LogError("PlayerStatRes 객체가 null입니다.");
            }
        });
    }
}