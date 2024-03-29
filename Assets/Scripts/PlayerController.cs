using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //속도 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float swimSpeed;
    [SerializeField]
    private float swimFastSpeed;
    [SerializeField]
    private float upSwimSpeed;

    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //이동 확인 변수
    private Vector3 lastPos;//이전 프레임 플레이어 위치

    //앉았을 때 높이
    [SerializeField]
    private float crouchPosY;//앉았을 때 위치
    private float originPosY;//원래 위치
    private float applyCrouchPosY;

    //카메라 민감도
    [SerializeField]
    private float lookSensitivity;
    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //컴포넌트

    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private Camera theCamera;//Camera를 직접 지정.
    //[SerializeField]//만약 이 필드를 사용하면 직접 Rigidbody를 지정할 수 있다.
    private Rigidbody myRigid;
    private GunController gunController;
    private Crosshair crosshair;
    private StatusController statusController;
    // Start is called before the first frame update
    void Start()
    {
        //theCamera = FindObjectOfType<Camera>();//카메라 타입을 갖는 오브젝트를 찾아 지정한다.(카메라가 하나만 있을 때에만 유효)
        myRigid=GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // 카메라의 로컬 위치를 이용.(플레이어가 땅에 박히면 안되므로 카메라의 위치를 바꿔 앉기를 구현한다.
        applyCrouchPosY = originPosY;
        gunController = FindObjectOfType<GunController>();//GunController 탐색
        crosshair= FindObjectOfType<Crosshair>();
        statusController = FindObjectOfType<StatusController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();
            TryJump();
            if (!GameManager.isWater)
            {
                TryRun();
            }
            TryCrouch();
            Move();
            MoveCheck2();
            if (!Inventory.inventoryActivated)//인벤토리 비활성화 시 카메라 조작 비활성화
            {
                CameraRotation();
                CharacterRotation();
            }
        }
    }

    private void WaterCheck()
    {

        if (GameManager.isWater)
        {
            applySpeed = swimSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applySpeed = swimFastSpeed;
            }

        }
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

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // normalized -> 벡터의 정규벡터화. 크기를 1로 바꾼다.
        //Vector3 _movement = new Vector3(_moveDirX,0 ,_moveDirZ);

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck2()//이동 시 크로스헤어가 흔들리는 현상을 방지(입력을 통해 예방)
    {
        if (!isRun && !isCrouch && isGround)
        {
            if((Input.GetAxisRaw("Horizontal")!=0) || (Input.GetAxisRaw("Vertical") != 0))
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            crosshair.WalkingAnimation(isWalk);
        }
    }
    private void MoveCheck()
    {
        if (!isRun&& !isCrouch && isGround)//달리고,앉고 있지 않을 때 확인
        {
            if (Vector3.Distance(lastPos,transform.position)>=0.0001f)//이전 위치와 현재 위치가 다르면(거리가 0.01보다 크면)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            crosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;//위치 업데이트
        }
        
    }
    private void CharacterRotation()
    {
        //캐릭터는 좌우로 움직인다. (Mouse X에 의존)

        float _yRotation = Input.GetAxisRaw("Mouse X"); // Y축을 중심으로 회전한다.

        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0)*lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));//유니티는 회전을 사원수 기반으로 계산한다. Vector3을 이용하기 위해서는 Euler를 쿼터니언으로 변환할 필요가 있다.

        //Debug.Log(myRigid.rotation);//사원수
       // Debug.Log(myRigid.rotation.eulerAngles);//오일러 각
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)&&statusController.GetCurrentSP()>0)//Shift가 눌려 있을 때
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)||statusController.GetCurrentSP()<=0)//Shift를 떼었을 때
        {
            RunningCancle();
        }
    }
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();//달릴 때 앉아있으면 자동으로 앉은 상태를 해제한다.
        }

        gunController.CancelFineSight();
        isRun = true;
        applySpeed = runSpeed;
        crosshair.RunningAnimation(isRun);
        statusController.DecreaseStamina(10);
    }

    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
        crosshair.RunningAnimation(isRun);
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&isGround&&statusController.GetCurrentSP()>0&&!GameManager.isWater)
        {
            Jump();
        }else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
        {
            UpSwim();
        }
    }

    private void UpSwim()
    {
        myRigid.velocity = transform.up * upSwimSpeed;
    }
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();//점프할 때 앉아있으면 자동으로 앉은 상태를 해제한다.
        }
        myRigid.velocity = transform.up * jumpForce;
        statusController.DecreaseStamina(100);
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);//현재 위치에서 Vector down(무조건 아래) 방향으로 캡슐콜라이더의 y 크기만큼 레이저를 발사
        //무언가에 닿으면(바닥에 닿아 있으면) true -> 그런데 경사면에 있으면 어떡하죠? 약간의 여유를 준다.(0.1f)
        crosshair.JumpAnimation(!isGround);
    }
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;//isCrouch를 crouch 실행 시마다 반전시킨다.
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        crosshair.CrouchingAniation(isCrouch);
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);//카메라 위치를 수정
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()//앉기 코루틴 IEnumerator
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0; // 보간 카운트

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY += Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);// 자연스러운 이동을 위해 보간을 사용(시작점, 목적지, 변동치)
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);//카메라 위치 변경
            if (count > 15)//보간을 15번 정도 반복했으면
            {
                break; //보간을 중단한다
            }
            yield return null; // 1프레임 대기
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);//카메라 위치 설정
        //yield return new WaitForSeconds(1f);//1초 대기
    }
}
