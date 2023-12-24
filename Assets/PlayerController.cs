using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�ӵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //�ɾ��� �� ����
    [SerializeField]
    private float crouchPosY;//�ɾ��� �� ��ġ
    private float originPosY;//���� ��ġ
    private float applyCrouchPosY;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;
    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //������Ʈ

    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private Camera theCamera;//Camera�� ���� ����.
    //[SerializeField]//���� �� �ʵ带 ����ϸ� ���� Rigidbody�� ������ �� �ִ�.
    private Rigidbody myRigid;
    // Start is called before the first frame update
    void Start()
    {
        //theCamera = FindObjectOfType<Camera>();//ī�޶� Ÿ���� ���� ������Ʈ�� ã�� �����Ѵ�.(ī�޶� �ϳ��� ���� ������ ��ȿ)
        myRigid=GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // ī�޶��� ���� ��ġ�� �̿�.(�÷��̾ ���� ������ �ȵǹǷ� ī�޶��� ��ġ�� �ٲ� �ɱ⸦ �����Ѵ�.
        applyCrouchPosY = originPosY;
        
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void CameraRotation()
    {
        //ī�޶�� ���Ϸ� �����δ�(Mouse Y)
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // ���Ʒ� Ȯ��
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX; // ���� ��ġ�� ���Ѵ�.(��ȣ�� ������ ������ ������ �� �ִ�.)   
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit,cameraRotationLimit); // ī�޶� �̵����� ���� ���� (-45~+45)

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX,0,0); // ī�޶��� ���Ϸ� ���� ����
    }

    private void Move()
    {
        
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; // (1,0,0)�� ���𰡸� ���Ѵ�.
        Vector3 _moveVertical = transform.forward * _moveDirZ; // (0,0,1)�� ���𰡸� ���Ѵ�.

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // normalized -> ������ ���Ժ���ȭ. ũ�⸦ 1�� �ٲ۴�.
        //Vector3 _movement = new Vector3(_moveDirX,0 ,_moveDirZ);

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CharacterRotation()
    {
        //ĳ���ʹ� �¿�� �����δ�. (Mouse X�� ����)

        float _yRotation = Input.GetAxisRaw("Mouse X"); // Y���� �߽����� ȸ���Ѵ�.

        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0)*lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));//����Ƽ�� ȸ���� ����� ������� ����Ѵ�. Vector3�� �̿��ϱ� ���ؼ��� Euler�� ���ʹϾ����� ��ȯ�� �ʿ䰡 �ִ�.

        Debug.Log(myRigid.rotation);//�����
        Debug.Log(myRigid.rotation.eulerAngles);//���Ϸ� ��
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))//Shift�� ���� ���� ��
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//Shift�� ������ ��
        {
            RunningCancle();
        }
    }
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();//�޸� �� �ɾ������� �ڵ����� ���� ���¸� �����Ѵ�.
        }
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();//������ �� �ɾ������� �ڵ����� ���� ���¸� �����Ѵ�.
        }
        myRigid.velocity = transform.up * jumpForce;
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);//���� ��ġ���� Vector down(������ �Ʒ�) �������� ĸ���ݶ��̴��� y ũ�⸸ŭ �������� �߻�
        //���𰡿� ������(�ٴڿ� ��� ������) true -> �׷��� ���鿡 ������ �����? �ణ�� ������ �ش�.(0.1f)
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
        isCrouch = !isCrouch;//isCrouch�� crouch ���� �ø��� ������Ų��.
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

        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);//ī�޶� ��ġ�� ����
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()//�ɱ� �ڷ�ƾ IEnumerator
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0; // ���� ī��Ʈ

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY += Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);// �ڿ������� �̵��� ���� ������ ���(������, ������, ����ġ)
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);//ī�޶� ��ġ ����
            if (count > 15)//������ 15�� ���� �ݺ�������
            {
                break; //������ �ߴ��Ѵ�
            }
            yield return null; // 1������ ���
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);//ī�޶� ��ġ ����
        //yield return new WaitForSeconds(1f);//1�� ���
    }
}
