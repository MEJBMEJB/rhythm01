using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 �� �����ϴ� ��忡�� ���� �������� �Ѿ�� ���� ������ �ִ� �����͵� ����...
 ��� Scene���� ������ Scene�� �Ѿ�� �������� �ȵǴ� Gameobject

*2023-10-16
�ʿ��� ��: bpm, bgm�̸�
*2023-10-17
�ʿ��� ��: �ְ�����
��ȯ����: bgm�̸��� �� ��� index�� �ϸ� ���?
*2023-11-01
Json���ϰ� PlayerPrefs�� �̿��Ͽ� ���� ������� �ʴ� Ŭ����

 
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

    //���̸� �� �� index
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

    //�ְ�����
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
