using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

[System.Serializable]
public class MC_Data
{
    public int maxHealth;
    public float[] pos = new float[2];
    public List<List<string>> unlockedParts = new List<List<string>>();
    public List<string> equippedParts = new List<string>();
    public MC_Data()
    {
        maxHealth = MC_Controller.maxHealth;
        pos[0] = Info.mc.transform.position.x;
        pos[1] = Info.mc.transform.position.y;

        //Save the equipped parts
        for (int i = 1; i <= 5; i++)
        {
            equippedParts.Add(Info.mc.transform.GetChild(i).name);
        }

        int enumerator = 0;
        foreach (List<GameObject> list in Info.mc.GetComponent<MC_Controller>().unlockedParts)
        {
            unlockedParts.Add(new List<string>());
            foreach (GameObject obj in list)
            {
                unlockedParts[enumerator].Add(obj.name);
            }
            enumerator++;
        }
    }
}
