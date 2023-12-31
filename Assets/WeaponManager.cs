using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(GunController))]//�䱸�ϴ� ������Ʈ(�� ������Ʈ ������ ���� �����
public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;//Ŭ���� ��ü�� ������. -> Ŭ�������� ���� ����

    [SerializeField]
    private float changeWeaponDelayTime;//���� ��ü ������
    [SerializeField]
    private float changeWeaponEndDelayTime;//���� ��ü�� ������ ����

    //������ �����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    //���� ��ųʸ�
    private Dictionary<string,Gun> gunDictionary = new Dictionary<string,Gun>();
    private Dictionary<string,Hand> handDictionary = new Dictionary<string,Hand>();

    [SerializeField]
    private string currentWeaponType;//���� ���� Ÿ��

    public static Transform currentWeapon;//���� ����(����)
    public static Animator currentWeaponAnimator;//���� ���� �ִϸ��̼�

    [SerializeField] // �� ��Ʈ�ѷ�
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    // Start is called before the first frame update
    //[SerializeField]
    private void Start()
    {
        //��ųʸ� ����
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for(int i=0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))//���� 1�� ������ ��
            {
                //���� ��ü ����(��)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }else if (Input.GetKeyDown(KeyCode.Alpha2))
            {//��
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }
    public IEnumerator ChangeWeaponCoroutine(string type, string name)//���� Ÿ�� -> �ٲ� ���� �̸�
    {
            isChangeWeapon = true;
            currentWeaponAnimator.SetTrigger("WeaponOut");

            yield return new WaitForSeconds(changeWeaponDelayTime);

            CancelPreWeaponAction();
        WeaponChange(type,name);
        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        currentWeaponType = type;
        isChangeWeapon = false;
        
    }

    void CancelPreWeaponAction()
        {
            switch (currentWeaponType)
            {
                case "GUN":
                    gunController.CancelFineSight();
                    gunController.CancelReload();
                GunController.isActivate = false;//���� ��ü �� ���� ��Ȱ��ȭ
                    break;
                case "HAND":
                HandController.isActivate = false;
                    break;
            }
        }
    void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            gunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND") 
        {
            handController.HandChange(handDictionary[_name]);
        }
    }
}
