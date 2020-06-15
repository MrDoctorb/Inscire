using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scene_Data
{
    public Dictionary<string, object> dict = new Dictionary<string, object>();

    public Scene_Data()
    {
        //var allObjs = InterfaceHelper.FindObjects<ISaveable>();
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            ISaveable[] saves = obj.GetComponents<ISaveable>();
            foreach (ISaveable sav in saves)
            {
                sav.SaveData(this);
            }
        }
    }

    public void Store<T>(string key, T value) where T : class
    {
        if (key == null || key == "")
        {
            Debug.Log("A null key was passed");
        }
        else
        {
            dict.Add(key, value);
        }
    }

    public T Take<T>(string key) where T : class
    {
        if (key != null)
        {
            return dict[key] as T;
        }
        else
        {
            Debug.Log("Oh no! You gave a null key!" + key + " <== That's what your key was");
            return null;
        }
    }
}
