using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundInfo
{
    public string _name;
    public AudioClip _clip;
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioMgrInstance;
    public static AudioManager instance
    {
        get => _audioMgrInstance;
    }

    [SerializeField]
    private SoundInfo[] _sfx = null;
    [SerializeField]
    private SoundInfo[] _bgm = null;

    [SerializeField]
    private AudioSource _bgmPlayer = null;
    [SerializeField]
    private AudioSource[] _sfxPlayer = null;


    private void Start()
    {
        if (_audioMgrInstance == null)
        {
            _audioMgrInstance = this;
        //    Debug.Log($"_audioMgrInstance make!!");
        }
    }
    public void PlayBGM(string p_bgmName)
    {
        for(int i = 0; i < _bgm.Length; i++) 
        {
            if (_bgm[i]._name != p_bgmName)
            {
                continue;
            }
            _bgmPlayer.clip = _bgm[i]._clip;
            _bgmPlayer.Play(); 
            break;        
        }
    }

    public void StopBGM()
    {
        if(_bgmPlayer.isPlaying)
            _bgmPlayer.Stop();
    }

    public bool isFinishPlay()
    {
        if(!_bgmPlayer.isPlaying)
        {
            return true;
        }
        return false;
    }


    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < _bgm.Length; i++)
        {
            if (_sfx[i]._name == p_sfxName)
            {
                for(int x = 0; x < _sfxPlayer.Length; x++) 
                {
                    if (!_sfxPlayer[x].isPlaying)
                    {
                        _sfxPlayer[x].clip = _sfx[i]._clip;
                        _sfxPlayer[x].Play();
                        return;
                    }
                }
                //모든 오디오 플레이어 재생중....
                Debug.Log("모든 오디오 플레이어 재생중....");
                return;
            }
        }

        //찾는 효과음이 없습니다
        Debug.Log("찾는 효과음이 없습니다");
    }
}
