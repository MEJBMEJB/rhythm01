using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _scoreMgrInstance;
    public static ScoreManager Instance
    {
        get => _scoreMgrInstance;
    }
    [SerializeField]
    private TMP_Text _txtScore = null;

    [SerializeField]
    private int _increaseScore = 10;

    private int _currentScore = 0;
    public int currentScore
    {
        get => _currentScore;
        set => _currentScore = value;
    }

    [SerializeField]
    private float[] _weight = null; //판정별 점수는 어떻게 둘것인가?
    [SerializeField]
    private int _comboWeight = 10; //각 콤보별 가중치는 어떻게 할 것인가?


    //ComboManager _classComboMgr;

    // Start is called before the first frame update
    void Start()
    {
        if(_scoreMgrInstance == null)
        {
            _scoreMgrInstance = this;
            Debug.Log("ScoreManager Instance Make!");
        }
       //_classComboMgr = FindObjectOfType<ComboManager>();
        _currentScore = 0;
        _txtScore.text = "0";
    }

    public void IncreaseScore(int p_judmentState) //p_judmentState = 어떤 판정이 내렸나?
    {
        //콤보처리
        if(p_judmentState == -1) //miss
        {
            ComboManager.Instance.ResetCombo();
            return;
        }
        ComboManager.Instance.IncreaseCombo();
        
        //콤보 가중치 계산
        int t_currentCombo = ComboManager.Instance.CurrentCombo;
        int t_bonusComboScore = (t_currentCombo / 10) * _comboWeight;

        //점수 가중치 계산 및 UI 반영
        int t_increaseScore = _increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * _weight[p_judmentState]);
        _currentScore += t_increaseScore;
        //Debug.Log($"score = {_currentScore}");
        _txtScore.text = string.Format("{0:#,##0}", _currentScore);
    }
}
