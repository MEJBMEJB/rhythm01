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
        StartCoroutine(PlayAndEnd());        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
         //   AudioManager.instance.PlayBGM("BGM0");
         //   _musicStart = true;
        }
    }

    IEnumerator PlayAndEnd()
    {
        //���� ��� -> ���������� �ڷ�ƾ���� ��ٸ���
        //string playBGM = RememberDataBeforeStart.Instance.BGMName;        
        string playBGM = PlayerPrefs.GetString("BGMName");
        AudioManager.instance.PlayBGM(playBGM);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(AudioManager.instance.isFinishPlay())
            {
                AudioManager.instance.StopBGM();
                SceneManager.LoadScene("StageClear");
                break;
            }
        }
    }

}
