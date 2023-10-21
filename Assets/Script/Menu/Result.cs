using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class Result : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] _txtCount = null;
    [SerializeField]
    private TMP_Text _txtScore = null;
    [SerializeField]
    private TMP_Text _txtMaxCombo = null;

    private static Result _resultInstance;
    public static Result ResultInstance
    {
        get => _resultInstance;
    }

    //instance 구조로 바꿔서 다시해보기
    //ScoreManager _scoreManager;
    //ComboManager _comboManager;

    private void Awake()
    {
        if (_resultInstance == null)
        {
            _resultInstance = this;
            Debug.Log("_resultInstance make");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //_scoreManager = FindObjectOfType<ScoreManager>();
        //_comboManager = FindObjectOfType<ComboManager>();
        ShowResult();
    }

    private void ShowResult()
    {
        int i = 0;

        int[] tmpArr = TimeManager.timeMgrInstance.GetjudgementRecord();
        
        //각 판정 갯수 표시하기
        for(i = 0; i <tmpArr.Length; i++) 
        {
            _txtCount[i].text = string.Format("{0:#,##0}", tmpArr[i]);
        }
        _txtScore.text = string.Format("{0:#,##0}", ScoreManager.Instance.currentScore);
        _txtMaxCombo.text = string.Format("{0:#,##0}", ComboManager.Instance.MaxCombo);

        //최고기록여부 확인 - PlayerPrefs으로 비교하자 -> json으로 적자
        int scoreNow = ScoreManager.Instance.currentScore;
        int topScore = PlayerPrefs.GetInt("TopScore");
        int songIdx = PlayerPrefs.GetInt("PlayIndex");

        if(scoreNow > topScore)
        {
            SongInfoToJson tmp = new SongInfoToJson();
            string strTmp = File.ReadAllText(tmp.jsonFilePath);
            tmp = JsonUtility.FromJson<SongInfoToJson>(strTmp);
            Debug.Log($"current = {scoreNow} topscore = {topScore}, idx = {songIdx}");
            tmp.listSong[songIdx]._topScore = scoreNow;
            Debug.Log($"현재 최고점수 = {tmp.listSong[songIdx]._topScore}");

            strTmp = JsonUtility.ToJson(tmp, true);
            File.WriteAllText(tmp.jsonFilePath, strTmp);
        }
        else
        {
            Debug.Log($"갱신 실패... 현재 = {scoreNow}, 최고 = {topScore}");
        }
    }

    public void BtnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BtnClickRetry()
    {
        SceneManager.LoadScene("MainGame");
    }
    
    public void BtnClickSelectSong()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

}
