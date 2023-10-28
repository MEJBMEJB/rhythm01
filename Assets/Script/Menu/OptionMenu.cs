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
    private int _gameMode; //0: ���� 1: �̵���
    public int gameMode
    {
        get => _gameMode;
        set => _gameMode = value;
    }
}



public class OptionMenu : MonoBehaviour
{
    //���ӿ��� Ȱ��ȭ ���ΰ��� �̹��� ����
    [SerializeField]
    private Sprite[] _toggleTapGameOver;
    [SerializeField]
    private GameObject _ObjtoggleTapGameOver;

    //��� ���ø��
    [SerializeField]
    private string[] _selectLanguage;
    [SerializeField]
    private TMP_Text _ObjselectLanguage = null;

    //���Ӹ�� ����
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
        //������ ���� ��� �⺻����
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

    public void BtnDefault() //�ʱ⼳������
    {
        SetDefault();
    }

    private void SetDefault()
    {
        // �ʱ⼳��: ���ӿ������(true)
        // ����: �ѱ���(korean) == 0
        // ���Ӹ��: ������ġ

        //Ŭ�������� �ݿ� 
        _optionValue.selectLanguage = 0;
        _optionValue.gameMode = 0;
        _optionValue.GameOverMode = true;

        //UI �����ϱ�
        _ObjtoggleTapGameOver.GetComponent<Image>().sprite = _toggleTapGameOver[0];
        _ObjselectLanguage.text = _selectLanguage[0];
        _ObjGameMode.text = _selectGameMode[0];

        // Json ���Ͽ� ����ϱ�

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
}
