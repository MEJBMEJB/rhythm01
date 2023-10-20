using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField]
    private float _noteSpeed;

    private Image _noteImage;

    private void OnEnable()
    {
        if(_noteImage == null)
            _noteImage = GetComponent<Image>();
        _noteImage.enabled = true; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.right * _noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        _noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return _noteImage.enabled;
    }
}
