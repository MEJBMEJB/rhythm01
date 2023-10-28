using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

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
    public SongInfoToJson()
    {
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
//[System.Serializable]
public class OptionSettingValue
{
    public OptionSettingValue() 
    {
        _jsonFilePath = Application.dataPath + "/JsonFile/OptionData.json";
    }
    private string _jsonFilePath;
    public string jsonFilePath
    {
        get => _jsonFilePath;
        set => _jsonFilePath = value;
    }
    private bool _isGameOverMode;
    public bool GameOverMode
    {
        get => _isGameOverMode;
        set => _isGameOverMode = value;
    }

    private int _selectLanguage; //0:korean 1:English
    public int selectLanguage
    {
        get => _selectLanguage;
        set => _selectLanguage = value;
    }
    private int _gameMode; //0: 고정 1: 이동식
    public int gameMode
    {
        get => _gameMode;
        set => _gameMode = value;
    }
}



public class OptionMenu : MonoBehaviour
{
    //게임오버 활성화 여부관련 이미지 설정
    [SerializeField]
    private Sprite[] _toggleTapGameOver;
    [SerializeField]
    private GameObject _ObjtoggleTapGameOver;

    //언어 선택모드
    [SerializeField]
    private string[] _selectLanguage;
    [SerializeField]
    private TMP_Text _ObjselectLanguage = null;

    //게임모드 선택
    [SerializeField]
    private string[] _selectGameMode;
    [SerializeField]
    private TMP_Text _ObjGameMode = null;

    private OptionSettingValue _optionValue;

    private void Awake()
    {        
        _optionValue = new OptionSettingValue();


    }

    private void Start()
    {
        //파일이 없는 경우 기본설정
        if (!File.Exists(_optionValue.jsonFilePath))
        {
            SetDefault();

        }
    }

    public void BtnSave()
    {

    }

    public void BtnReturnMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnDefault() //초기설정으로
    {
        SetDefault();
    }

    private void SetDefault()
    {
        // 초기설정: 게임오버모드(true)
        // 언어설정: 한국어(korean) == 0
        // 게임모드: 고정위치

        //클레스값에 반영 
        _optionValue.selectLanguage = 0;
        _optionValue.gameMode = 0;
        _optionValue.GameOverMode = true;

        //UI 설정하기
        _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        _ObjselectLanguage.text = _selectLanguage[0];
        _ObjGameMode.text = _selectGameMode[0];

        // Json 파일에 기록하기

    }

    // 게임오버(hp < 0)인 경우 게임오버 적용여부
    public void BtnSetGameOverMode()
    {
        bool mode = _optionValue.GameOverMode;

        if(mode) 
        {
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[1];
        }
        else
        {
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        }
        _optionValue.GameOverMode = !mode;

    }
}
