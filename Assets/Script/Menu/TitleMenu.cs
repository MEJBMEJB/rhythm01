using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void BtnPlay()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
