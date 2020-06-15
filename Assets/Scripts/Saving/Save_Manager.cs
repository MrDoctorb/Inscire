using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ZaneSpace;

public static class Save_Manager
{
    public static void SaveSceneData(string sceneName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves/" + sceneName + ".sav";
        FileStream fs = new FileStream(path, FileMode.Create);
        Scene_Data data = new Scene_Data();
        bf.Serialize(fs, data);
        fs.Close();
    }

    public static void LoadSceneData(string sceneName)
    {
        string path = Application.persistentDataPath + "/Saves/" + sceneName + ".sav";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            Scene_Data data = (Scene_Data)bf.Deserialize(fs);
            fs.Close();

            var allObjs = InterfaceHelper.FindObjects<ISaveable>();
            foreach (ISaveable obj in allObjs)
            {
                obj.LoadData(data);
            }
        }
        else
        {
            Debug.Log("No file found");
        }
    }

    public static void SavePlayerData()
    {
        //Turn on the Save Icon
        Info.gm.transform.GetChild(4).GetComponent<Animator>().SetTrigger("Save");
        //Complicated Saving stuff
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves/MC.sav";
        FileStream fs = new FileStream(path, FileMode.Create);
        MC_Data data = new MC_Data();
        bf.Serialize(fs, data);
        fs.Close();
        Debug.Log("Player Saved");
    }

    public static void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/Saves/MC.sav";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            MC_Data data = (MC_Data)bf.Deserialize(fs);
            fs.Close();
            MC_Controller.maxHealth = data.maxHealth;
            Info.mc.transform.position = new Vector2(data.pos[0], data.pos[1]);
            int i = 0;
            foreach (List<string> list in data.unlockedParts)
            {
                Info.mc.GetComponent<MC_Controller>().unlockedParts[i].Clear();
                foreach (string obj in list)
                {
                    Info.gm.GetComponent<Game_Manager>().UnlockPart(i, obj);
                }
                i++;
            }

            List<int> equippedPartIndexes = new List<int>();
            int enumerator = 0;
            foreach(string partName in data.equippedParts)
            {
                equippedPartIndexes.Add(data.unlockedParts[enumerator].IndexOf(partName));
                enumerator++;
            }
            Info.mc.GetComponent<MC_Controller>().LoadSavedParts(equippedPartIndexes);
            Info.mc.GetComponent<Attack>().UpdateArms();
            Debug.Log("Player Loaded");
        }
        else
        {
            Debug.Log("No file found");
        }
    }

    public static void ClearAllData()
    {
        System.IO.DirectoryInfo path = new DirectoryInfo(Application.persistentDataPath + "/Saves");

        foreach (FileInfo file in path.GetFiles())
        {
            Debug.Log(file + "'s data has been cleared!");
            file.Delete();
        }
    }
}
