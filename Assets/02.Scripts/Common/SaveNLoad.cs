using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //����ȭ�Ȱ� 2�� ���Ϸ� ����� ��ȯ��

public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data //Data Ŭ������ ����
    {
        public float playerX; //ĳ������ X��ǥ��
        public float playerY; //ĳ������ Y��ǥ��
        public float playerZ; //ĳ������ Z��ǥ��

        public int playerLv; //ĳ���� ����
        public int playerHP; //ĳ���� �ִ� hp

        public int playerCurrentHP; //ĳ������ ���� ü��
        public int playerCurrentEXP; //ĳ������ ����ġ

        public int playerATK; //ĳ������ ���ݷ�
        public int playerDEF; //ĳ������ ����
        public float playerSPD; //ĳ������ �̵��ӵ�

        public List<int> playerItemInventory; //�÷��̾� �κ��丮
        public List<int> playerItemInventoryCount; //�÷��̾� �κ��丮 ������ ��
    }
    private PlayerAction thePlayer;
    private PlayerStat thePlayerStat;
    private PlayerDamage playerDamage;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private SoundManager theSound;

    public Data data;

    private Vector3 vector;

    public string click_sound;
    public void CallSave()
    {
        thePlayer = GetComponent<PlayerAction>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        playerDamage = FindObjectOfType<PlayerDamage>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theInven = FindObjectOfType<Inventory>();
        theSound = FindObjectOfType<SoundManager>();

        data.playerX = thePlayer.transform.position.x; //���� ĳ������ X��ǥ�� ����
        data.playerY = thePlayer.transform.position.y; //���� ĳ������ Y��ǥ�� ����
        data.playerZ = thePlayer.transform.position.z; //���� ĳ������ Z��ǥ�� ����

        data.playerLv = thePlayerStat.character_Lv; //���� ĳ������ ���� ����
        data.playerHP = thePlayerStat.maxHP; //���� ĳ������ ĳ���� �ִ� hp ����
        data.playerCurrentHP = playerDamage.curHp; //���� ĳ������ ���� hp ����
        data.playerCurrentEXP = thePlayerStat.currentEXP; //���� ĳ������ ����ġ ����
        data.playerATK = thePlayerStat.atk; //���� ĳ������ ���ݷ� ����
        data.playerDEF = thePlayerStat.def; //���� ĳ������ ���� ����
        data.playerSPD = thePlayerStat.speed; //���� ĳ������ �̵��ӵ� ����

        Debug.Log("���� ������ ����");

        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();

        List<ItemInfo> itemList = theInven.SaveItem(); //inventoryItemList
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("�κ��丮�� ������ ���� �Ϸ� : " + itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");
        bf.Serialize(file, data);
        file.Close();

        //string fileName = "Save";
        //string path = Application.persistentDataPath + "/" + fileName + ".dat";
        //FileStream filestream = new FileStream(path, FileMode.Create);
        //BinaryFormatter formatter = new BinaryFormatter();
        //formatter.Serialize(filestream, data);
        //filestream.Close();

        Debug.Log(Application.persistentDataPath + "�� ��ġ�� �����߽��ϴ�.");
    }
    public void CallLoad()
    {
        //string fileName = "Save";
        //string path = Application.persistentDataPath + "/" + fileName + ".dat";

        //FileStream file = new FileStream(path, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
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

            List<ItemInfo> itemList = new List<ItemInfo>();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("�κ��丮 �������� �ε��߽��ϴ� : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }
            for(int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                Debug.Log("�ε�");
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }

            theInven.LoadItem(itemList);      
            //GameManager theGM = FindObjectOfType<GameManager>();
            //theGM.LoadStart();
            theSound.Play(click_sound);
            //SceneLoader.Instance.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("����� ���̺� ������ �����ϴ�.");
        }
        file.Close();
    }
}