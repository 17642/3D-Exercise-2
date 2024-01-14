using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;//플레이어 위치
    public Vector3 playerDirection;

    public List<string> invenItemName = new List<string>();//아이템 이름들
    public List<int> invenItemNum = new List<int>();//아이템 수
    public List<int> invenArrayNumber = new List<int>();//배열 요소 수
}
public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();//세이브 데이터 생성
    private string SAVE_DATA_DIRECTORY;//세이브 디렉토리
    private string SAVE_FILENAME="/Savefile.txt";//세이브 파일 이름


    private PlayerController playerController;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerController 
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";//세이브 저장 위치

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))//세이브 폴더가 없으면
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);//만든다.
        }
    }

    public void SaveData()
    {
        playerController = FindObjectOfType<PlayerController>();
        inventory = FindObjectOfType<Inventory>();//각 컴포넌트는 로드 시에 가져옴

        saveData.playerPos = playerController.transform.position;//플레이어 위치 저장
        saveData.playerDirection = playerController.transform.eulerAngles;//오일러 각으로 저장
        Slot[] slots = inventory.GetSlots();//슬롯을 가져와서

        for(int i=0; i<slots.Length; i++)//슬롯만큼 돌린다.
        {
            if (slots[i].item != null)//슬롯에 아이템이 존재하면
            {
                saveData.invenArrayNumber.Add(i);//슬롯에 있는 아이템을 추가
                saveData.invenItemName.Add(slots[i].item.name);//아이템 이름 지정
                saveData.invenItemNum.Add(slots[i].itemCount);//아이템 수 추가
            }
        }

        string json = JsonUtility.ToJson(saveData);//세이브 데이터를 Json으로 작성

        File.WriteAllText(SAVE_FILENAME, json);//작성된 JSon을 파일 이름으로 저장.

        Debug.Log("저장 완료.\n" + json);

    }

    public void LoadData()
    {
        if (File.Exists(SAVE_FILENAME))//세이브 파일이 존재하면
        {
            string loadJson = File.ReadAllText(SAVE_FILENAME);//세이브 파일에서 가져옴

            saveData = JsonUtility.FromJson<SaveData>(loadJson);//Json 파일을 SaveData로 변환

            playerController = FindObjectOfType<PlayerController>();
            inventory = FindObjectOfType< Inventory>();

            playerController.transform.position = saveData.playerPos;
            playerController.transform.eulerAngles = saveData.playerDirection;

            for(int i = 0; i < saveData.invenItemName.Count; i++)//아이템 만큼 루프를 돌림
            {
                inventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNum[i]);//인덱스 테이터 삽입
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다");
        }
    }
}
