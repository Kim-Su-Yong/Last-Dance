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

        public int playerMoney; //ĳ������ ������

        public List<int> playerItemInventory; //�÷��̾� �κ��丮
        public List<int> playerItemInventoryCount; //�÷��̾� �κ��丮 ������ ��
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
        data.playerMoney = thePlayerStat.money; //���� ĳ������ ������ ����

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
        BinaryFormatter bf = new BinaryFormatter(); //����ȭ ���� ����
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat"); //SaveFile.dat�̶�� ������ ����
        bf.Serialize(file, data); //��ü�� �����ͷ� ��ȯ
        file.Close(); //���� �ݱ�

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
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open); //SaveFile.dat�̶�� ���� ����

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file); //�����͸� ��ü�� ��ȯ

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerAction>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            playerDamage = FindObjectOfType<PlayerDamage>();
            theInven = FindObjectOfType<Inventory>();

            vector.Set(data.playerX, data.playerY, data.playerZ); //�����Ϳ� ����� �÷��̾��� x,y,z ������ ����
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv; //�����Ϳ� �ִ� �������� �÷��̾� ������ �־���
            thePlayerStat.maxHP = data.playerHP; //�����Ϳ� �ִ� �ִ� HP�� �÷��̾� �ִ� HP�� �־���
            playerDamage.curHp = data.playerCurrentHP; //�����Ϳ� �ִ� ���� HP�� �÷��̾� ���� HP�� �־���
            thePlayerStat.currentEXP = data.playerCurrentEXP; //�����Ϳ� �ִ� ����ġ�� �÷��̾� ����ġ�� �־���
            thePlayerStat.atk = data.playerATK; //�����Ϳ� �ִ� ���ݷ��� �÷��̾� ���ݷ¿� �־���
            thePlayerStat.def = data.playerDEF; //�����Ϳ� �ִ� ������ �÷��̾� ���¿� �־���
            FindObjectOfType<PlayerDamage>().hpUpdate(); //�����Ϳ� �ִ� HP�� ����Ǿ����� �׿� ���� UI�� ������Ʈ
            thePlayerStat.money = data.playerMoney; //�����Ϳ� �ִ� �������� �÷��̾� �����ݿ� �־���
            thePlayerStat.moneyText.text = thePlayerStat.money.ToString();

            Debug.Log("�ε� �Ϸ�!");

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
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                Debug.Log("�ε�");
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }
            //getanitem ���⿡ �־��
            //theInven.LoadItem(itemList);
        }
        else
        {
            Debug.Log("����� ���̺� ������ �����ϴ�.");
        }
        file.Close();
    }
}