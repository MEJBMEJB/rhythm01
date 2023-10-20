using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _timeMgrinstance;
    public static TimeManager timeMgrInstance
    {
        get => _timeMgrinstance;
    }
    private List<GameObject> _boxNoteList;
    public List<GameObject> boxNotList
    {
        get { return _boxNoteList; }
    }
    [SerializeField]
    private Transform _center = null;
    [SerializeField]
    private RectTransform[] _timingRect = null; //판정범위 설정
    
    private Vector2[] _timingBoxs = null; //판정 등급: Perfect > Cool > Good > Bad
    private int[] _judgementRecord;
    public int GetjudgementRecord(int idx)
    {
         return _judgementRecord[idx];        
    }
    public int[] GetjudgementRecord()
    {
        return _judgementRecord;
    }
    public void SetjudgementRecord(int idx, int value)
    {
        _judgementRecord[idx] = value;
    }
    public void SetJudgementRecordPlus(int idx)
    {
        _judgementRecord[idx]++;
    }

    private EffectManager _classeffectManager;
    //private ScoreManager _classScoreManger;
    //private ComboManager _classComboManager;
    //public ScoreManager GetScoreManger
    //{
    //    get => _classScoreManger;
    //}


    void Start()
    {
        if(_timeMgrinstance == null)
        {
            _timeMgrinstance = this;
        }
        //class 참조 설정
        _classeffectManager = FindObjectOfType<EffectManager>(); 
        //_classScoreManger = FindObjectOfType<ScoreManager>();
        //_classComboManager = FindObjectOfType<ComboManager>();

        _boxNoteList = new List<GameObject>();
        _timingBoxs = new Vector2[_timingRect.Length];
        _judgementRecord = new int[5];

        for (int i = 0; i < _timingRect.Length; i++)
        {
            _timingBoxs[i].Set(_center.localPosition.x - _timingRect[i].rect.width / 2,
                               _center.localPosition.x + _timingRect[i].rect.width / 2);
        }
    }

    //public void CheckTiming()
    public bool CheckTiming()
    {
        float t_notePosX; 
        for(int i = 0; i < _boxNoteList.Count; i++) 
        {
            t_notePosX = _boxNoteList[i].transform.localPosition.x;

            for(int j = 0; j < _timingBoxs.Length; j++)
            {
                if (_timingBoxs[j].x <= t_notePosX && t_notePosX <= _timingBoxs[j].y)
                {
                    switch (j)
                    {
                        case 0:
                            Debug.Log("Perfect!");                            
                            _classeffectManager.NoteHitEffect();
                            break;
                        case 1:
                            Debug.Log("Cool");
                            _classeffectManager.NoteHitEffect();
                            break;
                        case 2:
                            Debug.Log("Good");
                            break;
                        case 3:
                            Debug.Log("Bad");
                            break;
                        default:
                            break;
                    }
                    _judgementRecord[j]++;

                    //note 제거
                    _boxNoteList[i].GetComponent<Note>().HideNote();
                    _boxNoteList.RemoveAt(i);

                    //effect
                    _classeffectManager.JudgementEffect(j);

                    //score
                    ScoreManager.Instance.IncreaseScore(j);                    

                    //효과음 재생
                    AudioManager.instance.PlaySFX("Clap");
                    return true;
                }
            }
        }

        _judgementRecord[4]++;
        Debug.Log($"Miss! = {_judgementRecord[4]}");
        _classeffectManager.JudgementEffect(_timingBoxs.Length); //Miss연출
        ScoreManager.Instance.IncreaseScore(-1);
        StatusManager.Instance.DecreaseHp(1);
        return false;

    }

}
