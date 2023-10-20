using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenterFlame : MonoBehaviour
{
    //private AudioSource _myAudio;
    //private bool _musicStart;

    // Start is called before the first frame update
    void Start()
    {
        //   _myAudio = GetComponent<AudioSource>();
        //_musicStart = false;
        StartCoroutine(PlayAndEnd());
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!_musicStart)
        //{
        //    if (collision.CompareTag("Note"))
        //    {
        //    //    _myAudio.Play();
        //        _musicStart = true;
        //    }
        //}
        if (collision.CompareTag("Note"))
        {
         //   AudioManager.instance.PlayBGM("BGM0");
         //   _musicStart = true;
        }
    }

    IEnumerator PlayAndEnd()
    {
        //string playBGM = RememberDataBeforeStart.Instance.BGMName;        
        string playBGM = PlayerPrefs.GetString("BGMName");
        AudioManager.instance.PlayBGM(playBGM);
        //_musicStart = true;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(AudioManager.instance.isFinishPlay())
            {
                AudioManager.instance.StopBGM();
                //DontDestroyOnLoad(RememberDataBeforeStart.Instance.objStageMenu);
                SceneManager.LoadScene("StageClear");
                //Result.ResultInstance.ShowResult();
                break;
            }
        }
    }

}
