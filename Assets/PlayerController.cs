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
    private Camera theCamera;//Camera�� ���� ����.

    //[SerializeField]//���� �� �ʵ带 ����ϸ� ���� Rigidbody�� ������ �� �ִ�.
    private Rigidbody myRigid;
    // Start is called before the first frame update
    void Start()
    {
        //theCamera = FindObjectOfType<Camera>();//ī�޶� Ÿ���� ���� ������Ʈ�� ã�� �����Ѵ�.(ī�޶� �ϳ��� ���� ������ ��ȿ)
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

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed; // normalized -> ������ ���Ժ���ȭ. ũ�⸦ 1�� �ٲ۴�.
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
}
