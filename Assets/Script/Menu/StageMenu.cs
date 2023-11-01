using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class StageMenu : MonoBehaviour
{
    [SerializeField]
    private SongInfo[] _listSong;
    [SerializeField]
    private TMP_Text _txtSongName = null;
    [SerializeField]
    private TMP_Text _txtSongArtist = null;
    [SerializeField]
    private TMP_Text _txtTopScore = null;
    [SerializeField]
    private Image _imgDisk = null;

    // ���� �̸���� �ð� �����ϱ�
    private float _currentTime = 0.0f;
    [SerializeField]
    private float _allowPreListen = 20.0f;

    // ���� ������ ���� index
    private int _currentSelectIndex = 0;

    private string _jsonFilePath;

    private void Start()
    {
        _jsonFilePath = Application.dataPath + "/JsonFile/SaveData.json";
        LoadSongInJson();
        SettingSong();
    }

    public void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime > _allowPreListen )
        {            
            AudioManager.instance.StopBGM();
            _currentTime = 0.0f;
        }
    }

    public void BtnNext()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (++_currentSelectIndex > _listSong.Length - 1)
        {
            _currentSelectIndex = 0;
        }
        Debug.Log($"_currentSelectIndex = {_currentSelectIndex}");
        SettingSong();
    }
    public void BtnPrev()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (--_currentSelectIndex < 0)
        {
            _currentSelectIndex = _listSong.Length - 1;
        }
        SettingSong();
    }

    private void SettingSong()
    {
        _txtSongName.text = _listSong[_currentSelectIndex]._songName;
        _txtSongArtist.text = _listSong[_currentSelectIndex]._artistName;
        _imgDisk.sprite = _listSong[_currentSelectIndex]._songSprite;
        _txtTopScore.text = _listSong[_currentSelectIndex]._topScore.ToString();

        AudioManager.instance.PlayBGM("BGM" + _currentSelectIndex.ToString());
    }
    
    private void LoadSongInJson()
    {
        if (!File.Exists(_jsonFilePath))
        {
            SaveSongInJson();
            return;
        }
        SongInfoToJson listSongtmp = new SongInfoToJson();
        string textData = File.ReadAllText(_jsonFilePath);                

        listSongtmp = JsonUtility.FromJson<SongInfoToJson>(textData);
        Debug.Log($"{listSongtmp.listSong.Count}");

        int idx = 0;
        for(idx = 0; idx < listSongtmp.listSong.Count; idx++)
        {
            _listSong[idx]._songName = listSongtmp.listSong[idx]._songName;
            _listSong[idx]._artistName = listSongtmp.listSong[idx]._artistName;
            _listSong[idx]._bgmName = listSongtmp.listSong[idx]._bgmName;
            _listSong[idx]._topScore = listSongtmp.listSong[idx]._topScore;
        }
        
        //foreach (SongInfo value in listSongtmp.listSong)
        //{
        //
        //    Debug.Log($"Index = {idx}");
        //    Debug.Log($"artist = {value._artistName}");
        //    Debug.Log($"song = {value._songName}");
        //    Debug.Log($"bgmName = {value._bgmName}");
        //    Debug.Log($"bpmValue = {value._bpmValue}");
        //    Debug.Log($"topscore = {value._topScore}");
        //    idx++;
        //}
    }

    private void SaveSongInJson()
    {
        if (!File.Exists(_jsonFilePath))
        {
            using (File.Create(_jsonFilePath))
            {
                Debug.Log($"_jsonFilePath = {_jsonFilePath} ��� ���� ����");
            }
        }
        else
        {
            Debug.Log($"_jsonFilePath = {_jsonFilePath} ��ο� ������ �̹� �����մϴ�");
        }

        SongInfoToJson tmp = new SongInfoToJson();
        for(int i = 0; i< _listSong.Length; i++) 
        {
            tmp.listSong.Add(_listSong[i]);
        }
        string eachline = JsonUtility.ToJson(tmp, true);
        //Debug.Log($"{eachline}");
        File.WriteAllText(_jsonFilePath, eachline);
    }
    

    public void BtnReturnTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnPlayGame()
    {        
        // ������Ʈ���� ���� �����ϴ� ���� ������ �����Ѵ� - for PlayerPrefs
        PlayerPrefs.SetInt("PlayBPM", _listSong[_currentSelectIndex]._bpmValue);
        PlayerPrefs.SetString("BGMName", _listSong[_currentSelectIndex]._bgmName);
        PlayerPrefs.SetInt("PlayIndex", _currentSelectIndex);
        PlayerPrefs.SetInt("TopScore", _listSong[_currentSelectIndex]._topScore);

        // �̸����� �����ϴ� ���� ����� �����
        AudioManager.instance.StopBGM();

        // �� �̵��߿� �Ķ���ͷ� ������ GameObject�� �ı����� �ʴ´�
        //DontDestroyOnLoad(RememberDataBeforeStart.Instance.objStageMenu);        
        SceneManager.LoadScene("MainGame");
    }
}
