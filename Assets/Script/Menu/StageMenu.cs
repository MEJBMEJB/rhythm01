using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Data;

/*
 * 선택음악을 정하는 역할을 한다
 *  SonInfo class -> 음악제목, 이름, bpm 정보를 담고있다

 
 
 */

/*
[System.Serializable]
public class SongInfo
{
    public SongInfo(string name, string artist, string bgm, int bpm, int topscore)
    {
        _songName = name;
        _artistName = artist;
        _bgmName = bgm;
        _bpmValue = bpm;
        _topScore = topscore;
    }
    public SongInfo()
    {

    }
    public string _songName;
    public string _artistName;
    public string _bgmName; //스크립트에서 실행하기 위한 문자열
    public int _bpmValue;
    public Sprite _songSprite;
    public int _topScore;
}
[System.Serializable]
public class SongInfoToJson
{
    public SongInfoToJson() {
        listSong = new List<SongInfo>();
        _jsonFilePath = Application.dataPath + "/JsonFile/SaveData.json";
    }
    public List<SongInfo> listSong;
    private string _jsonFilePath;
    public string jsonFilePath
    {
        get => _jsonFilePath;
        set => _jsonFilePath = value;
    }
}
*/


public class StageMenu : MonoBehaviour
{
    [SerializeField]
    private SongInfo[] _listSong = null;
    [SerializeField]
    private TMP_Text _txtSongName = null;
    [SerializeField]
    private TMP_Text _txtSongArtist = null;
    [SerializeField]
    private TMP_Text _txtTopScore = null;
    [SerializeField]
    private Image _imgDisk = null;

    // 음악 미리듣기 시간 설정하기
    private float _currentTime = 0.0f;
    [SerializeField]
    private float _allowPreListen = 20.0f;

    // 현재 선택한 음악 index
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
        //Debug.Log($"{textData}");

        listSongtmp = JsonUtility.FromJson<SongInfoToJson>(textData);
        
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
                Debug.Log($"_jsonFilePath = {_jsonFilePath} 경로 파일 생성");
            }
        }
        else
        {
            Debug.Log($"_jsonFilePath = {_jsonFilePath} 경로에 파일이 이미 존재합니다");
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
        //선택해!! 

        // 실제 게임에 들어가기 전에 필요한 데이터들은 담는다(씬이동하기 전에) - for DontDestroyOnLoad
        //RememberDataBeforeStart.Instance.PlaybpmValue = _listSong[_currentSelectIndex]._bpmValue;
        //RememberDataBeforeStart.Instance.BGMName = _listSong[_currentSelectIndex]._bgmName;
        //RememberDataBeforeStart.Instance.idxBGM = _currentSelectIndex;
        // 레지스트리에 현재 실행하는 음악 정보를 저장한다 - for PlayerPrefs
        PlayerPrefs.SetInt("PlayBPM", _listSong[_currentSelectIndex]._bpmValue);
        PlayerPrefs.SetString("BGMName", _listSong[_currentSelectIndex]._bgmName);
        PlayerPrefs.SetInt("PlayIndex", _currentSelectIndex);
        PlayerPrefs.SetInt("TopScore", _listSong[_currentSelectIndex]._topScore);

        // 미리듣기로 실행하던 음악 재생을 멈춘다
        AudioManager.instance.StopBGM();

        // 씬 이동중에 파라미터로 지정한 GameObject는 파괴하지 않는다
        //DontDestroyOnLoad(RememberDataBeforeStart.Instance.objStageMenu);        
        SceneManager.LoadScene("MainGame");
    }
}
