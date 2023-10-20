using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private Animator _noteHitAnimator = null;
    private string _strHit = "Hit";

    [SerializeField]
    private Animator _judgementAnimator = null;
    [SerializeField]
    private Image _judgementImage = null;
    [SerializeField]
    private Sprite[] _judgementSprite = null;

    public void JudgementEffect(int p_num)
    {
        //Debug.Log($"{p_num}");
        _judgementImage.GetComponent<Image>().sprite = _judgementSprite[p_num];
        _judgementAnimator.SetTrigger(_strHit);
    }

    public void NoteHitEffect()
    {
        _noteHitAnimator.SetTrigger(_strHit);
    }
}
