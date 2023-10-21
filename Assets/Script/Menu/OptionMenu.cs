using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public void BtnSave()
    {

    }

    public void BtnReturnMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnDefault() //초기설정으로
    {

    }
}
