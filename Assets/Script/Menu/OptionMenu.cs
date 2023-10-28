using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

#region 곡 정보들을 json으로 읽기/쓰기를 위한 클래스
//[System.Serializable]
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
    private Sprite _songSprite;
    public Sprite songSprite
    {
        get => _songSprite;
        set => _songSprite = value;
    }
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
#endregion

#region Option 설정 값을 json <-> Application 주고 받기위해 필요한 class
//[System.Serializable]
public class OptionSettingValue // json에 쓰기전 메모리에 담기는 데이터
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
    private int _selectLanguage;
    public int SelectLanguage
    {
        get => _selectLanguage; 
        set => _selectLanguage = value;
    }
    private int _gameMode; //0: 고정 1: 이동식
    public int GameMode
    {
        get => _gameMode; 
        set => _gameMode = value;
    }
}
[System.Serializable]
public class OptionValueToJson //json에 쓰기 위한 데이터들
{
    public OptionValueToJson(bool tmpGameOver , int tmpLanguage, int tmpGameMode)
    {
        GameOverMode = tmpGameOver;
        language = tmpLanguage;
        gameMode= tmpGameMode;
        _jsonFilePath = Application.dataPath + "/JsonFile/OptionData.json";
    }
    public OptionValueToJson()
    {
        _jsonFilePath = Application.dataPath + "/JsonFile/OptionData.json";
    }
    public bool GameOverMode;
    public int language;
    public int gameMode;

    private string _jsonFilePath;
    public string jsonFilePath
    {
        get => _jsonFilePath;
        set => _jsonFilePath = value;
    }
}
#endregion



public class OptionMenu : MonoBehaviour
{
    //언어 선택모드
    [SerializeField]
    private string[] _strSelectLanguage;
    [SerializeField]
    private TMP_Text _ObjselectLanguage = null;

    //게임모드 선택
    [SerializeField]
    private TMP_Text _ObjGameMode = null;
    [SerializeField]
    private int _gameModeSize;

    //게임오버 활성화 여부관련 이미지 설정
    [SerializeField]
    private Sprite[] _toggleTapGameOver;
    [SerializeField]
    private GameObject _ObjtoggleTapGameOver;

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
        else
        {
            LoadOptionData();
        }
    }

    #region Save, Return, Default
    public void BtnSave()
    {
        SaveToJson(_optionValue.jsonFilePath);
    }

    public void BtnReturnMain() //메인화면으로 돌아가기
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnDefault() //초기설정으로
    {
        SetDefault();
    }
    #endregion

    #region selectLanguage, ChangeGameMode, ApplyGameOverMode
    public void BtnSelectLanguage() // 언어선택 변경
    {
        if(++_optionValue.SelectLanguage > _strSelectLanguage.Length - 1)
        {
            _optionValue.SelectLanguage = 0;
        }
        _ObjselectLanguage.text = _strSelectLanguage[_optionValue.SelectLanguage];
        LanguageManagerOption.Instance.setLocal(_optionValue.SelectLanguage);
    }

    public void BtnSelectGameMode() // 게임모드 변경
    {
        if(--_optionValue.GameMode < 0)
        {
            _optionValue.GameMode = _gameModeSize - 1;
        }
        //_ObjGameMode.text = _strSelectGameMode[_optionValue.GameMode];
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");
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
    #endregion

    private void SetDefault()
    {
        // 초기설정: 게임오버모드(true)        
        // 게임모드: 고정위치

        // 클래스값에 반영 
        _optionValue.SelectLanguage = 0;
        _optionValue.GameMode = 0;
        _optionValue.GameOverMode = true;

        // UI 설정하기
        _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        _ObjselectLanguage.text = _strSelectLanguage[0];
        LanguageManagerOption.Instance.setLocal(0);
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");

        // Json 파일에 기록하기
        SaveToJson(_optionValue.jsonFilePath);

    }

    private void SaveToJson(string path)
    {
        if (!File.Exists(path))
        {
            using(File.Create(path))
            {
                Debug.Log($"savePath = {path}");
            }
        }

        OptionValueToJson data = new OptionValueToJson(_optionValue.GameOverMode, _optionValue.SelectLanguage, _optionValue.GameMode);
        string saveText = JsonUtility.ToJson(data, true);
        Debug.Log($"{saveText}, {path}");
        File.WriteAllText(path, saveText);
    }

    private void LoadOptionData()
    {
        //json 파일에서 문자열 가져오기
        OptionValueToJson data = new OptionValueToJson();
        string strLoad = File.ReadAllText(data.jsonFilePath );
        data = JsonUtility.FromJson<OptionValueToJson>(strLoad);

        //가져온 문자열 데이터를 메모리에 저장
        _optionValue.GameOverMode = data.GameOverMode;
        _optionValue.SelectLanguage = data.language;
        _optionValue.GameMode = data.gameMode;

        //저장된 메모리 값에 따라 UI에 반영한다
        if(_optionValue.GameOverMode)
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        else
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[1];

        _ObjselectLanguage.text = _strSelectLanguage[_optionValue.SelectLanguage];
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");
    }
}
