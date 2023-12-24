using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float lookSensitivity;
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;


    [SerializeField]
    private Camera theCamera;//Camera를 직접 지정.

    //[SerializeField]//만약 이 필드를 사용하면 직접 Rigidbody를 지정할 수 있다.
    private Rigidbody myRigid;
    // Start is called before the first frame update
    void Start()
    {
        //theCamera = FindObjectOfType<Camera>();//카메라 타입을 갖는 오브젝트를 찾아 지정한다.(카메라가 하나만 있을 때에만 유효)
        myRigid=GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void CameraRotation()
    {
        //카메라는 상하로 움직인다(Mouse Y)
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // 위아래 확인
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX; // 현재 수치에 더한다.(부호를 조정해 방향을 반전할 수 있다.)   
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit,cameraRotationLimit); // 카메라 이동범위 제한 설정 (-45~+45)

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX,0,0); // 카메라의 오일러 각을 설정
    }

    private void Move()
    {
        
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; // (1,0,0)에 무언가를 곱한다.
        Vector3 _moveVertical = transform.forward * _moveDirZ; // (0,0,1)에 무언가를 곱한다.

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed; // normalized -> 벡터의 정규벡터화. 크기를 1로 바꾼다.
        //Vector3 _movement = new Vector3(_moveDirX,0 ,_moveDirZ);

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CharacterRotation()
    {
        //캐릭터는 좌우로 움직인다. (Mouse X에 의존)

        float _yRotation = Input.GetAxisRaw("Mouse X"); // Y축을 중심으로 회전한다.

        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0)*lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));//유니티는 회전을 사원수 기반으로 계산한다. Vector3을 이용하기 위해서는 Euler를 쿼터니언으로 변환할 필요가 있다.

        Debug.Log(myRigid.rotation);//사원수
        Debug.Log(myRigid.rotation.eulerAngles);//오일러 각
    }
}
