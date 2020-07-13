﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Squibble_Controller : MonoBehaviour, ITextEvent, ISaveable
{
    Animator anime;
    [SerializeField] bool invisible = true, finished = false;
    public List<ITextEventContainer> textEvents = new List<ITextEventContainer>();
    void OnEnable()
    {
        anime = GetComponent<Animator>();
    }

    public void TextFinished()
    {
        if (invisible)
        {
            invisible = false;
            gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(TeleportOut());
            if (textEvents.Count > 0)
            {
                foreach (ITextEventContainer eve in textEvents)
                {
                    InterfaceInfo.interfaceText = eve;
                    InterfaceInfo.TextEvent.TextFinished();
                }
            }
            //Bad design but works for now
            if (Mathf.Abs(Vector2.Distance(transform.position, Info.mc.transform.position)) < 2)
            {
                finished = true;
            }
        }
    }
    IEnumerator TeleportOut()
    {
        print("Teleport away! >:3");
        anime.SetTrigger("Teleport");
        //Wait for animation to end
        while (!anime.IsInTransition(0))
        {
            gameObject.SetActive(true);
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void SaveData(Scene_Data data)
    {
        string saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
        data.Store<object>(saveKey, finished);
    }

    public void LoadData(Scene_Data data)
    {
        string saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
        finished = (bool)data.Take<object>(saveKey);
        if (finished)
        {
            gameObject.SetActive(false);
        }
    }
}
