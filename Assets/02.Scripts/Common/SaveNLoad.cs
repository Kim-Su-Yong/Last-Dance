using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //����ȭ�Ȱ� 2�� ���Ϸ� ����� ��ȯ��

public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public float playerX;
        public float playerY;
        public float playerZ;

        public int playerLv;
        public int playerHP;

        public int playerCurrentHP;
        public int playerCurrentEXP;

        public int playerATK;
        public int playerDEF;
        public float playerSPD;

        public List<int> playerItemInventroy;
        public List<int> playerItemInventoryCount;
    }
    private PlayerAction thePlayer;
    private PlayerStat thePlayerStat;
    private PlayerDamage playerDamage;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private SoundManager theSound;

    Data data = new Data();

    private Vector3 vector;

    public string click_sound;
    void Awake()
    {
        thePlayer = GetComponent<PlayerAction>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        playerDamage = FindObjectOfType<PlayerDamage>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theInven = FindObjectOfType<Inventory>();
        theSound = FindObjectOfType<SoundManager>();
    }
    public void CallSave()
    {
        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.maxHP;
        data.playerCurrentHP = playerDamage.curHp;
        data.playerCurrentEXP = thePlayerStat.currentEXP;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.playerSPD = thePlayerStat.speed;

        Debug.Log("���� ������ ����");

        //data.playerItemInventroy.Clear();
        //data.playerItemInventoryCount.Clear();
        //data.playerEquipItem.Clear();

        List<ItemInfo> itemList = theInven.SaveItem();
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("�κ��丮�� ������ ���� �Ϸ� : " + itemList[i].itemID);
            data.playerItemInventroy.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");
        //bf.Serialize(file, data);
        //file.Close();
        string fileName = "Save";
        string path = Application.persistentDataPath + "/" + fileName + ".dat";

        FileStream filestream = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(filestream, data);
        filestream.Close();
        Debug.Log(Application.persistentDataPath + "�� ��ġ�� �����߽��ϴ�.");
    }
    public void CallLoad()
    {
        string fileName = "Save";
        string path = Application.persistentDataPath + "/" + fileName + ".dat";

        FileStream file = new FileStream(path, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        //Data data = bf.Deserialize(file) as Data;
        //FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

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
            GetComponent<PlayerDamage>().hpUpdate();

            List<ItemInfo> itemList = new List<ItemInfo>();

            for (int i = 0; i < data.playerItemInventroy.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventroy[i] == theDatabase.itemList[x].itemID)
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

            theInven.LoadItem(itemList);
            //Debug.Log("�ε�1");
            //theEquip.ShowTxT();          

            //GameManager theGM = FindObjectOfType<GameManager>();
            //theGM.LoadStart();
            theSound.Play(click_sound);
            SceneLoader.Instance.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("����� ���̺� ������ �����ϴ�.");
        }
        file.Close();
    }
}