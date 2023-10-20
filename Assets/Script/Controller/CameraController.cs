using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerPos = null;
    [SerializeField]
    private float _followSpeed = 15.0f;

    private Vector3 _playerDistance;

    //¡‹ => √‡º“, »Æ¥Î
    private float _hitDistance = 0.0f;
    [SerializeField]
    private float _zoomDistance = -1.25f;

    // Start is called before the first frame update
    void Start()
    {
        _playerDistance = new Vector3();
        _playerDistance = transform.position - _playerPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t_destPos = _playerPos.position + _playerDistance + (transform.forward * _hitDistance);
        transform.position = Vector3.Lerp(transform.position, t_destPos, _followSpeed * Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        _hitDistance = _zoomDistance;

        yield return new WaitForSeconds(0.15f);

        _hitDistance = 0;
    }
}
