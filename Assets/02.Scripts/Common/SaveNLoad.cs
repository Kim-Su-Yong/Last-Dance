using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //직렬화된걸 2진 파일로 만드는 변환기
using UnityEngine.SceneManagement;
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

        public int added_atk;
        public int added_def;

        public List<int> playerItemInventroy;
        public List<int> playerITemInventoryCount;
        public List<int> playerEquipItem;

        public string mapName;
        public string sceneName;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;
    }
    private PlayerAction thePlayer;
    private PlayerStat thePlayerStat;
    private PlayerDamage playerDamage;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;

    public Data data;

    private Vector3 vector;

    public void CallSave()
    {
        theDatabase = FindObjectOfType<DatabaseManager>();
        playerDamage = FindObjectOfType<PlayerDamage>();
        thePlayer = FindObjectOfType<PlayerAction>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();

        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.initHP;
        data.playerCurrentHP = playerDamage.curHp;
        data.playerCurrentEXP = thePlayerStat.currentEXP;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("기초 데이터 성공");

        data.playerItemInventroy.Clear();
        data.playerITemInventoryCount.Clear();
        data.playerEquipItem.Clear();

        //for (int i = 0; i < theDatabase.var_name.Length; i++)
        //{
        //    data.varNameList.Add(theDatabase.var_name[i]);
        //    data.varNumberList.Add(theDatabase.var[i]);
        //}
        //for (int i = 0; i < theDatabase.switch_name.Length; i++)
        //{
        //    data.swNameList.Add(theDatabase.switch_name[i]);
        //    data.swList.Add(theDatabase.switches[i]);
        //}
        List<ItemInfo> itemList = theInven.SaveItem();
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("인벤토리의 아이템 저장 완료 : " + itemList[i].itemID);
            data.playerItemInventroy.Add(itemList[i].itemID);
            data.playerITemInventoryCount.Add(itemList[i].itemCount);
        }
        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("장착된 아이템 저장 완료 : " + theEquip.equipItemList[i].itemID);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");

        bf.Serialize(file, data);
        file.Close();
        Debug.Log(Application.dataPath + "이 위치에 저장했습니다.");
    }
    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file);

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerAction>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            playerDamage = FindObjectOfType<PlayerDamage>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.initHP = data.playerHP;
            playerDamage.curHp = data.playerCurrentHP;
            thePlayerStat.currentEXP = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;

            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;

            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            for (int i = 0; i < theEquip.equipItemList.Length; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerEquipItem[i] == theDatabase.itemList[x].itemID)
                    {
                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("장착된 아이템을 로드했습니다 : " + theEquip.equipItemList[i].itemID);
                        break;
                    }
                }
            }

            List<ItemInfo> itemList = new List<ItemInfo>();

            for (int i = 0; i < data.playerItemInventroy.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventroy[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }

            for(int i = 0; i < data.playerITemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerITemInventoryCount[i];
            }
            theInven.LoadItem(itemList);
            //theEquip.ShowTxT();          

            GameManager theGM = FindObjectOfType<GameManager>();
            theGM.LoadStart();

            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }
        file.Close();
    }
}
