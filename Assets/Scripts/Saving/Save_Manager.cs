using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ZaneSpace;

public static class Save_Manager
{
    static string savePath = Application.persistentDataPath + "/Saves";
    /*
    public static void SaveSceneData(string sceneName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves/" + sceneName + ".sav";
        FileStream fs = new FileStream(path, FileMode.Create);
        Scene_Data data = new Scene_Data();
        bf.Serialize(fs, data);
        fs.Close();
    }*/

    public static void LoadSceneData(string sceneName)
    {
        string path = "";
        if (File.Exists(savePath + "/Temp/" + sceneName + ".sav"))
        {
            path = savePath + "/Temp/" + sceneName + ".sav";
        }
        else if (File.Exists(savePath + "/" + sceneName + ".sav"))
        {
            path = savePath + "/" + sceneName + ".sav";
        }
        else
        {
            Debug.Log("No file found");
        }
        if (path != "")
        {
            Debug.Log(path);    
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
    }
    /*
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
            //SaveSceneData(SceneManager.GetActiveScene().name);
        }*/

    public static void LoadPlayerData()
    {
        string path = "";
        if (File.Exists(savePath + "/Temp/MC.sav"))
        {
            path = savePath + "/Temp/MC.sav";
        }
        else if (File.Exists(savePath + "/MC.sav"))
        {
            path = savePath + "/MC.sav";
        }

        else
        {
            Debug.Log("No file found");
        }

        if (path != "")
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
            foreach (string partName in data.equippedParts)
            {
                equippedPartIndexes.Add(data.unlockedParts[enumerator].IndexOf(partName));
                enumerator++;
            }
            Info.mc.GetComponent<MC_Controller>().LoadSavedParts(equippedPartIndexes);
            Info.mc.GetComponent<Attack>().UpdateArms();
            Debug.Log("Player Loaded");
        }
    }

    public static void ClearTempData()
    {
        DirectoryInfo path = new DirectoryInfo(savePath + "/Temp");
        foreach(FileInfo file in path.GetFiles())
        {
            file.Delete();
        }
    }

    public static void ClearAllData()
    {
        ClearTempData();
        DirectoryInfo path = new DirectoryInfo(savePath);
        foreach (FileInfo file in path.GetFiles())
        {
            file.Delete();
        }
    }

    public static void SoftSave()
    {
        string path = savePath + "/Temp/" + SceneManager.GetActiveScene().name + ".sav";
        SaveSceneCore(path);
        SavePlayerCore();
        Info.roomStartPosition = Info.mc.transform.position;
    }

    public static void HardSave()
    {   //Turn on the Save Icon
        Info.gm.transform.GetChild(4).GetComponent<Animator>().SetTrigger("Save");
        //Normal Save
        SoftSave();
        //Move Files
        System.IO.DirectoryInfo path = new DirectoryInfo(savePath + "/Temp");
        foreach (FileInfo file in path.GetFiles())
        {
            string newPath = savePath + "/" + file.Name;
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            file.MoveTo(newPath);
        }
    }

    static void SavePlayerCore()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + "/Temp/MC.sav", FileMode.Create);
        MC_Data data = new MC_Data();
        bf.Serialize(fs, data);
        fs.Close();
        Debug.Log("Player Saved");
    }

    static void SaveSceneCore(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        Scene_Data data = new Scene_Data();
        bf.Serialize(fs, data);
        fs.Close();
    }
}
