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
    //���Ӽ��ÿ��� ������ bpm ���� ��´�.(instance�� �ʹ� �θ��°� �δ�....)
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

        if(_currentTime >= 60d / _bpm) //note �����ð�
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
                //�Ķ���͵� ENUMȭ ��Ų��
                _classeffectManager.JudgementEffect(4);
                ScoreManager.Instance.IncreaseScore(-1);
                //_classtimeManager.GetScoreManger.IncreaseScore(-1);
                Debug.Log("Miss!!!-> NoteTag OnTriggerExit2D");
                int val = TimeManager.timeMgrInstance.GetjudgementRecord(4);
                TimeManager.timeMgrInstance.SetJudgementRecordPlus(4);
                StatusManager.Instance.DecreaseHp(1);
                //Debug.Log($"before = {val}, After = {TimeManager.timeMgrInstance.GetjudgementRecord(4)}");
            }

            _classtimeManager.boxNotList.Remove(collision.gameObject); //���� ó���ؾߵ�(���ӻ� �����ִ�) ��Ʈ������ �������
            ObjectPool.Instance.NoteQueue.Enqueue(collision.gameObject); //��Ʈ�� ������ ť�� �ٽ� �״´�.
            collision.gameObject.SetActive(false); //���ӻ� �����ִ� ��Ʈ�� �Ⱥ��̰�(��Ȱ��ȭ)��Ų��
        }
    }
}
