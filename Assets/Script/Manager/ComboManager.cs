using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ComboManager : MonoBehaviour
{
    private static ComboManager _comboMgrInstance;
    public static ComboManager Instance
    {
        get => _comboMgrInstance;
    }

    [SerializeField]
    private GameObject _goComboImage = null;
    [SerializeField]
    private TMP_Text _txtCombo;

    private int _currentCombo = 0;
    public int CurrentCombo
    {
        get => _currentCombo;
        set => _currentCombo = value;
    }
    private int _maxCombo = 0;
    public int MaxCombo
    {
        get => _maxCombo;
        set => _maxCombo = value;
    }

    private void Start()
    {
        if(_comboMgrInstance == null)
        {
            _comboMgrInstance = this;
        }
        _txtCombo.gameObject.SetActive(false);
        _goComboImage.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        _currentCombo += p_num;
        if(_maxCombo < _currentCombo)
            _maxCombo = _currentCombo;
        _txtCombo.text = string.Format("{0:#,##0}", _currentCombo);

        if(_currentCombo > 2)
        {
            _txtCombo.gameObject.SetActive(true);
            _goComboImage.SetActive(true);
        }
    }

    public void ResetCombo()
    {
        _txtCombo.text = "0";
        _currentCombo = 0;
        _goComboImage.SetActive(false);
        _txtCombo.gameObject.SetActive(false);
    }
}
