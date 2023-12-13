using UnityEngine;

public class LoginManager : CustomSingleton<DataManager>
{
    [SerializeField] private string sceneName;

    bool isLoginSucced;

    private void Start()
    {
        SoundManager.Instance.Play("BGMStartScene", eSoundType.Bgm);

        TestLogin();
    }

    private void Update()
    {
        if (isLoginSucced == true) LoadingScenController.LoadScene(sceneName);
    }

    private void TestLogin()
    {

    }
}
