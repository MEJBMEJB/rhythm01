using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void BtnClickRetry()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void BtnClickSelectSong()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
