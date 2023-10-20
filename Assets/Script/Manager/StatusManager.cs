using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    private static StatusManager _statusMgrinstance;
    public static StatusManager Instance
    {
        get => _statusMgrinstance;
    }

    private bool _isGameOver;
    public bool IsGameOver
    {
        get => _isGameOver; 
        set => _isGameOver = value; 
    }

    [SerializeField]
    private int _maxHp;
    private int _currentHp;

    //[SerializeField]
    //private Image _hpImage = null;
    [SerializeField]
    private Image _gaugeImage = null;

    [SerializeField]
    private float _blickSpeed = 0.1f;
    [SerializeField]
    private int _blickCount = 10;
    private int _currentBlickCount = 0;

    [SerializeField]
    private MeshRenderer _playerRender = null;

    private void Awake()
    {
        if(_statusMgrinstance == null)
        {
            _statusMgrinstance = this;
        }
        _isGameOver = false;
        _currentHp = _maxHp;
    }

    public void DecreaseHp(int p_num)
    {
        _currentHp -= p_num;
        SettingHpImage();
        StartCoroutine(BlickCo());

        if (_currentHp <= 0 )
        {
            SceneManager.LoadScene("GameOver");
        }
        
    }

    private void SettingHpImage()
    {
        _gaugeImage.fillAmount = (float)_currentHp / _maxHp;
        Debug.Log($"_gaugeImage.fillAmount = {_gaugeImage.fillAmount}");
    }

    IEnumerator BlickCo()
    {
        while(_currentBlickCount <= _blickCount)
        {
            _playerRender.enabled = !_playerRender.enabled;
            yield return new WaitForSeconds(_blickSpeed);
            _currentBlickCount++;
        }

        _playerRender.enabled = true;
        _currentBlickCount = 0;
    }
}
