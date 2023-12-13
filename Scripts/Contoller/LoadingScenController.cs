using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScenController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image ProgressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 비동기적으로 씬 로드
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                ProgressBar.fillAmount = op.progress; // 로딩 진행률에 따라 프로그레스 바 업데이트
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5F;
                ProgressBar.fillAmount = Mathf.Lerp(0.7f, 1f, timer);
                if (ProgressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;  // 프로그레스 바가 완전히 채워지면 씬 활성화
                    yield break;
                }
            }
        }
    }
}
