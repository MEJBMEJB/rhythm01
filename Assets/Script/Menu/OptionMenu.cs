using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

#region �� �������� json���� �б�/���⸦ ���� Ŭ����
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
    public string _bgmName; //��ũ��Ʈ���� �����ϱ� ���� ���ڿ�
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
}
[System.Serializable]
public class OptionValueToJson //json�� ���� ���� �����͵�
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

    private OptionSettingValue _optionValue;

    private void Awake()
    {
        _optionValue = new OptionSettingValue();


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
        SaveToJson(_optionValue.jsonFilePath);
    }

    public void BtnReturnMain() //����ȭ������ ���ư���
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnDefault() //�ʱ⼳������
    {
        SetDefault();
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
    }

    public void BtnSelectGameMode() // ���Ӹ�� ����
    {
        if(--_optionValue.GameMode < 0)
        {
            _optionValue.GameMode = _gameModeSize - 1;
        }
        //_ObjGameMode.text = _strSelectGameMode[_optionValue.GameMode];
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");
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

    private void SetDefault()
    {
        // �ʱ⼳��: ���ӿ������(true)        
        // ���Ӹ��: ������ġ

        // Ŭ�������� �ݿ� 
        _optionValue.SelectLanguage = 0;
        _optionValue.GameMode = 0;
        _optionValue.GameOverMode = true;

        // UI �����ϱ�
        _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        _ObjselectLanguage.text = _strSelectLanguage[0];
        LanguageManagerOption.Instance.setLocal(0);
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");

        // Json ���Ͽ� ����ϱ�
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
        //json ���Ͽ��� ���ڿ� ��������
        OptionValueToJson data = new OptionValueToJson();
        string strLoad = File.ReadAllText(data.jsonFilePath );
        data = JsonUtility.FromJson<OptionValueToJson>(strLoad);

        //������ ���ڿ� �����͸� �޸𸮿� ����
        _optionValue.GameOverMode = data.GameOverMode;
        _optionValue.SelectLanguage = data.language;
        _optionValue.GameMode = data.gameMode;

        //����� �޸� ���� ���� UI�� �ݿ��Ѵ�
        if(_optionValue.GameOverMode)
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        else
            _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[1];

        _ObjselectLanguage.text = _strSelectLanguage[_optionValue.SelectLanguage];
        _ObjGameMode.text = LanguageManagerOption.Instance.getText("StringTable", "Option_SelectGamemode");
    }
}
