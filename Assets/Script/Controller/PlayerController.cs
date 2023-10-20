using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //이동
    [SerializeField]
    private float _moveSpeed = 3.0f;
    private Vector3 _dir;
    private Vector3 _destPos;

    //회전
    [SerializeField]
    private float _spinSpeed = 270.0f;
    private Vector3 _rotDir;
    private Quaternion _destRot;

    //반동
    [SerializeField]
    private float _recoilPos = 0.25f; //player 반동 크기
    [SerializeField]
    private float _recoilSpeed = 1.5f; //player 반동 속력

    [SerializeField]
    private Transform _fakeCube = null; //임의 큐브를 회전시킨(눈에는 안보임) 다음 실제 player에 적용한다
    [SerializeField]
    private Transform _realCube = null;

    private bool _canMove;

    private TimeManager _classtimeManager;
    private CameraController _classCameraController;

    // Start is called before the first frame update
    void Start()
    {
        _classtimeManager = FindObjectOfType<TimeManager>();
        _classCameraController = FindObjectOfType<CameraController>();

        _dir = new Vector3();
        _destPos = new Vector3();
        _rotDir = new Vector3();
        _destRot = new Quaternion();
        _canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    //판정 체크
        //    _classtimeManager.CheckTiming();
        //}

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
        {
            if(_canMove)
            {
                if (_classtimeManager.CheckTiming()) //판정체크
                {
                    StartAction();
                }
            }
        }
    }

    private void StartAction()
    {
        // 방향계산
        _dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        // 이동 목표값 계산
        _destPos = transform.position + new Vector3(-_dir.x, 0, _dir.z);

        // 회전 목표값 계산
        _rotDir = new Vector3(-_dir.z, 0.0f, -_dir.x);
        _fakeCube.RotateAround(transform.position, _rotDir, _spinSpeed);
        _destRot = _fakeCube.rotation;

        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(_classCameraController.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        _canMove = false;
        while(Vector3.SqrMagnitude(transform.position - _destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = _destPos;  
        _canMove = true;
    }

    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(_realCube.rotation, _destRot) > 0.5f)
        {
            _realCube.rotation = Quaternion.RotateTowards(_realCube.rotation, _destRot, _spinSpeed * Time.deltaTime);
            yield return null;
        }

        _realCube.rotation = _destRot;
    }

    IEnumerator RecoilCo()
    {
        while(_realCube.position.y < _recoilPos)
        {
            _realCube.position += new Vector3(0, _recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while(_realCube.position.y > 0)
        {
            _realCube.position -= new Vector3(0, _recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        _realCube.localPosition = Vector3.zero;
    }
}
