using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteManager : MonoBehaviour
{
    private static NoteManager _noteMgrInstance;
    public static NoteManager Instance
    {
        get => _noteMgrInstance;
    }
    //게임선택에서 설정한 bpm 값을 담는다.(instance를 너무 부르는게 부담....)
    private int _bpm; 
    double _currentTime = 0d;

    [SerializeField]
    Transform _tfNoteAppear = null;

    private TimeManager _classtimeManager;
    private EffectManager _classeffectManager;

    private void Awake()
    {
        if (_noteMgrInstance == null)
        {
            _noteMgrInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _classeffectManager = FindObjectOfType<EffectManager>();
        _classtimeManager = GetComponent<TimeManager>();
        //_bpm = RememberDataBeforeStart.Instance.PlaybpmValue;
        _bpm = PlayerPrefs.GetInt("PlayBPM");
        Debug.Log($"BPM = {_bpm}");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainGame")
        {
            return;
        }

        _currentTime += Time.deltaTime;

        if(_currentTime >= 60d / _bpm) //note 생성시간
        {
            //GameObject t_note = Instantiate(_goNote, _tfNoteAppear.position, Quaternion.identity);
            //t_note.transform.SetParent(this.transform);

            GameObject t_note = ObjectPool.Instance.NoteQueue.Dequeue();
            t_note.transform.position = _tfNoteAppear.position;
            t_note.SetActive(true);
            _classtimeManager.boxNotList.Add(t_note);   
            _currentTime -= 60d / _bpm;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                //파라미터도 ENUM화 시킨다
                _classeffectManager.JudgementEffect(4);
                ScoreManager.Instance.IncreaseScore(-1);
                //_classtimeManager.GetScoreManger.IncreaseScore(-1);
                Debug.Log("Miss!!!-> NoteTag OnTriggerExit2D");
                int val = TimeManager.timeMgrInstance.GetjudgementRecord(4);
                TimeManager.timeMgrInstance.SetJudgementRecordPlus(4);
                StatusManager.Instance.DecreaseHp(1);
                //Debug.Log($"before = {val}, After = {TimeManager.timeMgrInstance.GetjudgementRecord(4)}");
            }

            _classtimeManager.boxNotList.Remove(collision.gameObject); //현재 처리해야될(게임상에 남아있다) 노트에서는 사라진다
            ObjectPool.Instance.NoteQueue.Enqueue(collision.gameObject); //노트가 생성된 큐에 다시 쌓는다.
            collision.gameObject.SetActive(false); //게임상에 남아있는 노트는 안보이게(비활성화)시킨다
        }
    }
}
