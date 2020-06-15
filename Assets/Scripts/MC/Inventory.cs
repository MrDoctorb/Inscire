using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameObject inventorybook;
    Inventory inv;
    void Start()
    {
        inventorybook = ZaneSpace.Info.gm.transform.GetChild(3).gameObject;
        inv = ZaneSpace.Info.mc.GetComponent<Inventory>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Inventory") && !ZaneSpace.Info.worldPaused && !ZaneSpace.Info.gamePaused)
        {
            inventorybook.SetActive(true);
        }
    }
}
