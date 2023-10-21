using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void BtnPlay()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void BtnTerminate()
    {
        Debug.Log("Program terminate");
        Application.Quit();
    }

    public void BtnOption()
    {
        SceneManager.LoadScene("OptionScene");
    }
}
