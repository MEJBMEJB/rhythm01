using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;


#region �� �������� json���� �б�/���⸦ ���� Ŭ����
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
    public string _bgmName; //��ũ��Ʈ���� �����ϱ� ���� ���ڿ�
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
#endregion


#region Option ���� ���� json <-> Application �ְ� �ޱ����� �ʿ��� class
//[System.Serializable]
public class OptionSettingValue // json�� ������ �޸𸮿� ���� ������
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
    private int _gameMode; //0: ���� 1: �̵���
    public int GameMode
    {
        get => _gameMode; 
        set => _gameMode = value;
    }
    private float _gameSoundVolume;
    public float GameSoundVolume
    {
        get => _gameSoundVolume;
        set => _gameSoundVolume = value;
    }

}
[System.Serializable]
public class OptionValueToJson //json�� ���� ���� �����͵�
{
    public OptionValueToJson(bool tmpGameOver , int tmpLanguage, int tmpGameMode, float tmpVol)
    {
        GameOverMode = tmpGameOver;
        language = tmpLanguage;
        gameMode= tmpGameMode;
        gameSoundVolume = tmpVol;

        _jsonFilePath = Application.dataPath + "/JsonFile/OptionData.json";
    }
    public OptionValueToJson()
    {
        _jsonFilePath = Application.dataPath + "/JsonFile/OptionData.json";
    }
    public bool GameOverMode;
    public int language;
    public int gameMode;
    public float gameSoundVolume;

    private string _jsonFilePath;
    public string jsonFilePath
    {
        get => _jsonFilePath;
        set => _jsonFilePath = value;
    }
}
#endregion

enum BUTTON_NAME //��ư �̸��� ���ڿ����� enum���� ����
{ 
    DEFAULT = 0,
    SAVE,
    RETURN

}


public class OptionMenu : MonoBehaviour
{
    //��� ���ø��
    [SerializeField]
    private string[] _strSelectLanguage;
    [SerializeField]
    private TMP_Text _ObjselectLanguage = null;

    //���Ӹ�� ����
    [SerializeField]
    private TMP_Text _ObjGameMode = null;
    [SerializeField]
    private int _gameModeSize;

    //���ӿ��� Ȱ��ȭ ���ΰ��� �̹��� ����
    [SerializeField]
    private Sprite[] _toggleTapGameOver;
    [SerializeField]
    private GameObject _ObjtoggleTapGameOver;

    //�Ҹ�����
    [SerializeField]
    private Slider _gameVolumeSlider;

    //�˾�
    [SerializeField]
    private GameObject _objPopUpMessage;
    [SerializeField]
    private TMP_Text _popUpText; //�˾��޽����� �����ϴ� �޽���
    private int _popUpIndex = -1; //� ��ư�� ���������? 

    private OptionSettingValue _optionValue;

    private void Awake()
    {
        _optionValue = new OptionSettingValue();
        _objPopUpMessage.SetActive(false);

    }

    private void Start()
    {
        //������ ���� ��� �⺻����
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
        SetPopUpMessage(BUTTON_NAME.SAVE);
    }

    public void BtnReturnMain() //����ȭ������ ���ư���
    {
        SetPopUpMessage(BUTTON_NAME.RETURN);
    }

    public void BtnDefault() //�ʱ⼳������
    {
        SetPopUpMessage(BUTTON_NAME.DEFAULT);
    }
    #endregion

    #region selectLanguage, ChangeGameMode, ApplyGameOverMode
    public void BtnSelectLanguage() // ���� ����
    {
        if(++_optionValue.SelectLanguage > _strSelectLanguage.Length - 1)
        {
            _optionValue.SelectLanguage = 0;
        }
        _ObjselectLanguage.text = _strSelectLanguage[_optionValue.SelectLanguage];
        LanguageManagerOption.Instance.setLocal(_optionValue.SelectLanguage);

        SetGameModeText();
    }

    public void BtnSelectGameMode() // ���Ӹ�� ����
    {
        if(--_optionValue.GameMode < 0)
        {
            _optionValue.GameMode = _gameModeSize - 1;
        }
        //_ObjGameMode.text = _strSelectGameMode[_optionValue.GameMode];
        Debug.Log($"GameMode = {_optionValue.GameMode}");

        SetGameModeText();
    }

    // ���ӿ���(hp < 0)�� ��� ���ӿ��� ���뿩��
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


    #region PopUp ��ư ��(Yes), �ƴϿ�(No) ���� 
    public void PopUpClickYes()
    {
        BUTTON_NAME btnVal = (BUTTON_NAME)_popUpIndex;
        switch(btnVal)
        {
            case BUTTON_NAME.DEFAULT:
                SetDefault();
                break;
            case BUTTON_NAME.SAVE:
                SaveToJson(_optionValue.jsonFilePath);
                break;
            case BUTTON_NAME.RETURN:
                SceneManager.LoadScene("MainMenu");
                break;
        }
        _objPopUpMessage.SetActive(false);
    }
    public void PopUpClickNo()
    {
        _objPopUpMessage.SetActive(false);
        return;
    }
    #endregion
    private void SetDefault()
    {
        // �ʱ⼳��: ���ӿ������(true)        
        // ���Ӹ��: ������ġ

        // Ŭ�������� �ݿ� 
        _optionValue.SelectLanguage = 0;
        _optionValue.GameMode = 0;
        _optionValue.GameOverMode = true;
        _gameVolumeSlider.value = 1.0f;

        // Json ���Ͽ� ����ϱ�
        SaveToJson(_optionValue.jsonFilePath);

        // UI �����ϱ�
        Debug.Log("SetDefault");
        _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        _ObjselectLanguage.text = _strSelectLanguage[0];
        LanguageManagerOption.Instance.setLocal(0);
        _gameVolumeSlider.value = 1.0f;     
        SetGameModeText();
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

        _optionValue.GameSoundVolume = _gameVolumeSlider.value;
        OptionValueToJson data = new OptionValueToJson(_optionValue.GameOverMode, _optionValue.SelectLanguage, _optionValue.GameMode, _optionValue.GameSoundVolume);
        string saveText = JsonUtility.ToJson(data, true);
        Debug.Log($"{saveText}, {path}");
        File.WriteAllText(path, saveText);
    }

    private void LoadOptionData()
    {
        //json ���Ͽ��� ���ڿ� ��������
        OptionValueToJson data = new OptionValueToJson();
        string strLoad = File.ReadAllText(data.jsonFilePath );
        data = JsonUtility.FromJson<OptionValueToJson>(strLoad);

        //������ ���ڿ� �����͸� �޸𸮿� ����
        _optionValue.GameOverMode = data.GameOverMode;
        _optionValue.SelectLanguage = data.language;
        _optionValue.GameMode = data.gameMode;
        _optionValue.GameSoundVolume = data.gameSoundVolume;

        //����� �޸� ���� ���� UI�� �ݿ��Ѵ�
        if(_optionValue.GameOverMode)
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        else
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[1];

        _ObjselectLanguage.text = _strSelectLanguage[_optionValue.SelectLanguage];
        Debug.Log($"LoadOptionData = {_optionValue.SelectLanguage}");
        SetGameModeText();
        _gameVolumeSlider.value = _optionValue.GameSoundVolume;

    }

    private void SetGameModeText()
    {
        if (_optionValue.GameMode == 0)
            _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_GamemodeFixed");
        else if (_optionValue.GameMode == 1)
            _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_GamemodeRandom");
    }

    private void SetPopUpMessage(BUTTON_NAME btnVal)
    {
        _objPopUpMessage.SetActive(true);

        _popUpIndex = (int)btnVal;
        switch (btnVal)
        {
            case BUTTON_NAME.DEFAULT:
                _popUpText.text = LanguageManagerOption.Instance.getText("StringTable", "Option_MessageDefault");
                break;
            case BUTTON_NAME.SAVE:
                _popUpText.text = LanguageManagerOption.Instance.getText("StringTable", "Option_MessageSave");
                break;
            case BUTTON_NAME.RETURN:
                _popUpText.text = LanguageManagerOption.Instance.getText("StringTable", "Option_MessageReturn");
                break;
            default:
                _popUpText.text = "";
                break;
        }
    }
}
