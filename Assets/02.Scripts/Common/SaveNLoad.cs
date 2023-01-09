using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //직렬화된걸 2진 파일로 만드는 변환기

public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data //Data 클래스로 정의
    {
        public float playerX; //캐릭터의 X좌표값
        public float playerY; //캐릭터의 Y좌표값
        public float playerZ; //캐릭터의 Z좌표값

        public int playerLv; //캐릭터 레벨
        public int playerHP; //캐릭터 최대 hp

        public int playerCurrentHP; //캐릭터의 현재 체력
        public int playerCurrentEXP; //캐릭터의 경험치

        public int playerATK; //캐릭터의 공격력
        public int playerDEF; //캐릭터의 방어력
        public float playerSPD; //캐릭터의 이동속도

        public int playerMoney; //캐릭터의 소지금

        public List<int> playerItemInventory; //플레이어 인벤토리
        public List<int> playerItemInventoryCount; //플레이어 인벤토리 아이템 수
    }
    private PlayerAction thePlayer;
    private PlayerStat thePlayerStat;
    private PlayerDamage playerDamage;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private SoundManager theSound;
    public static SaveNLoad Instance;
    public SaveNLoad savenload;

    public Data data;

    private Vector3 vector;

    public string click_sound;
    private void Awake()
    {
        if (savenload == null)
            savenload = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void CallSave()
    {
        thePlayer = FindObjectOfType<PlayerAction>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        playerDamage = FindObjectOfType<PlayerDamage>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theInven = FindObjectOfType<Inventory>();
        theSound = FindObjectOfType<SoundManager>();

        data.playerX = thePlayer.transform.position.x; //현재 캐릭터의 X좌표값 저장
        data.playerY = thePlayer.transform.position.y; //현재 캐릭터의 Y좌표값 저장
        data.playerZ = thePlayer.transform.position.z; //현재 캐릭터의 Z좌표값 저장

        data.playerLv = thePlayerStat.character_Lv; //현재 캐릭터의 레벨 저장
        data.playerHP = thePlayerStat.maxHP; //현재 캐릭터의 캐릭터 최대 hp 저장
        data.playerCurrentHP = playerDamage.curHp; //현재 캐릭터의 현재 hp 저장
        data.playerCurrentEXP = thePlayerStat.currentEXP; //현재 캐릭터의 경험치 저장
        data.playerATK = thePlayerStat.atk; //현재 캐릭터의 공격력 저장
        data.playerDEF = thePlayerStat.def; //현재 캐릭터의 방어력 저장
        data.playerSPD = thePlayerStat.speed; //현재 캐릭터의 이동속도 저장
        data.playerMoney = thePlayerStat.money; //현재 캐릭터의 소지금 저장

        Debug.Log("기초 데이터 성공");

        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();

        List<ItemInfo> itemList = theInven.SaveItem(); //inventoryItemList
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("인벤토리의 아이템 저장 완료 : " + itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }
        BinaryFormatter bf = new BinaryFormatter(); //직렬화 변수 생성
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat"); //SaveFile.dat이라는 파일을 생성
        bf.Serialize(file, data); //객체를 데이터로 변환
        file.Close(); //파일 닫기

        //string fileName = "Save";
        //string path = Application.persistentDataPath + "/" + fileName + ".dat";
        //FileStream filestream = new FileStream(path, FileMode.Create);
        //BinaryFormatter formatter = new BinaryFormatter();
        //formatter.Serialize(filestream, data);
        //filestream.Close();
    }
    public void CallLoad()
    {
        //string fileName = "Save";
        //string path = Application.persistentDataPath + "/" + fileName + ".dat";
        //FileStream file = new FileStream(path, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        //Data data = bf.Deserialize(file) as Data;
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file);

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerAction>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            playerDamage = FindObjectOfType<PlayerDamage>();
            theInven = FindObjectOfType<Inventory>();

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.maxHP = data.playerHP;
            playerDamage.curHp = data.playerCurrentHP;
            thePlayerStat.currentEXP = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;
            FindObjectOfType<PlayerDamage>().hpUpdate();
            thePlayerStat.money = data.playerMoney;

            Debug.Log("로드 완료!");

            List<ItemInfo> itemList = new List<ItemInfo>();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                Debug.Log("로드");
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }
            theInven.LoadItem(itemList);

            //GameManager theGM = FindObjectOfType<GameManager>();
            //theGM.LoadStart();
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }
        file.Close();
    }
}