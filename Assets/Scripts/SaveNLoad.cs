using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;//�÷��̾� ��ġ
    public Vector3 playerDirection;

    public List<string> invenItemName = new List<string>();//������ �̸���
    public List<int> invenItemNum = new List<int>();//������ ��
    public List<int> invenArrayNumber = new List<int>();//�迭 ��� ��
}
public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();//���̺� ������ ����
    private string SAVE_DATA_DIRECTORY;//���̺� ���丮
    private string SAVE_FILENAME="/Savefile.txt";//���̺� ���� �̸�


    private PlayerController playerController;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerController 
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";//���̺� ���� ��ġ

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))//���̺� ������ ������
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);//�����.
        }
    }

    public void SaveData()
    {
        playerController = FindObjectOfType<PlayerController>();
        inventory = FindObjectOfType<Inventory>();//�� ������Ʈ�� �ε� �ÿ� ������

        saveData.playerPos = playerController.transform.position;//�÷��̾� ��ġ ����
        saveData.playerDirection = playerController.transform.eulerAngles;//���Ϸ� ������ ����
        Slot[] slots = inventory.GetSlots();//������ �����ͼ�

        for(int i=0; i<slots.Length; i++)//���Ը�ŭ ������.
        {
            if (slots[i].item != null)//���Կ� �������� �����ϸ�
            {
                saveData.invenArrayNumber.Add(i);//���Կ� �ִ� �������� �߰�
                saveData.invenItemName.Add(slots[i].item.name);//������ �̸� ����
                saveData.invenItemNum.Add(slots[i].itemCount);//������ �� �߰�
            }
        }

        string json = JsonUtility.ToJson(saveData);//���̺� �����͸� Json���� �ۼ�

        File.WriteAllText(SAVE_FILENAME, json);//�ۼ��� JSon�� ���� �̸����� ����.

        Debug.Log("���� �Ϸ�.\n" + json);

    }

    public void LoadData()
    {
        if (File.Exists(SAVE_FILENAME))//���̺� ������ �����ϸ�
        {
            string loadJson = File.ReadAllText(SAVE_FILENAME);//���̺� ���Ͽ��� ������

            saveData = JsonUtility.FromJson<SaveData>(loadJson);//Json ������ SaveData�� ��ȯ

            playerController = FindObjectOfType<PlayerController>();
            inventory = FindObjectOfType< Inventory>();

            playerController.transform.position = saveData.playerPos;
            playerController.transform.eulerAngles = saveData.playerDirection;

            for(int i = 0; i < saveData.invenItemName.Count; i++)//������ ��ŭ ������ ����
            {
                inventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNum[i]);//�ε��� ������ ����
            }

            Debug.Log("�ε� �Ϸ�");
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�");
        }
    }
}
