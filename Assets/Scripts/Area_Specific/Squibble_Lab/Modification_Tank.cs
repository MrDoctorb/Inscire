﻿using UnityEngine;
using ZaneSpace;

public class Modification_Tank : MonoBehaviour, ITextEvent, ISaveable, IInteractable
{
    [SerializeField]bool _locked;
    bool playerInteracting = false;
    string saveKey;
    Display_Text text;

    public void TextFinished()
    {
        if (!locked)
        {
            GetComponent<Animator>().SetTrigger("Open");
            transform.GetComponent<BoxCollider2D>().enabled = false;
            text.enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder += 100;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if(!playerInteracting)
        {
            locked = false;
        }
        playerInteracting = false;
    }

    public void Interact()
    {
        playerInteracting = true;
        GetComponent<Display_Text>().Interact();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Info.mc)
        {
            Save_Manager.SavePlayerData();
        }
    }

    public bool locked
    {
        get
        {
            return _locked;
        }
        set
        {
            _locked = value;
            if(_locked)
            {
                text.text = new string[] {"The tank looks like it could be opened. But the glass doesn't budge."};
            }
            else
            {
                text.text = new string[] {"Wrrrrr"};
            }
        }
    }

    void OnEnable()
    {
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
        text = GetComponent<Display_Text>();
        locked = _locked;
    }

    public void SaveData(Scene_Data data)
    {
        data.Store<object>(saveKey, locked);
    }

    public void LoadData(Scene_Data data)
    {
        locked = (bool)data.Take<object>(saveKey);
        if (locked)
        {
            //Stuff
        }
    }

}
