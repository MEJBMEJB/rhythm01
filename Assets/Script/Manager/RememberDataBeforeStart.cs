using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 곡 선택하는 모드에서 실제 게임으로 넘어가기 전에 가지고 있는 데이터들 모음...
 곡선택 Scene에서 본게임 Scene로 넘어갈때 없어지면 안되는 Gameobject

*2023-10-16
필요한 값: bpm, bgm이름
*2023-10-17
필요한 값: 최고점수
변환개선: bgm이름을 각 곡마다 index로 하면 어떨까?
*2023-11-01
Json파일과 PlayerPrefs를 이용하여 현재 사용하지 않는 클래스

 
 */

public class RememberDataBeforeStart : MonoBehaviour
{
    private static RememberDataBeforeStart _instance;
    public static RememberDataBeforeStart Instance
    {
        get => _instance;
    }

    [SerializeField]
    private GameObject _objStageMenu;
    public GameObject objStageMenu
    {
        get => _objStageMenu;
    }

    //bpm
    private int _playbpmValue;
    public int PlaybpmValue
    {
        get => _playbpmValue;
        set => _playbpmValue = value;
    }

    //곡이름 및 곡 index
    private string _BGMName;
    public string BGMName
    {
        get => _BGMName;
        set => _BGMName = value; 
    }

    private int _idxBGM;
    public int idxBGM
    {
        get => _idxBGM;
        set => _idxBGM = value;
    }

    //최고점수
    private int _topScore;
    public int TopScore
    {
        get => _topScore; 
        set => _topScore = value;
    }

    private void Awake()
    {
        if(_instance == null) 
        {
            _instance = this;
        }
    }
}
