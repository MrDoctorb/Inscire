using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Door : MonoBehaviour, ITextEvent, IInteractable
{
    Display_Text text;
    ITextEventContainer tempEvent;
    bool playerInteracting = false, _locked;
    bool locked
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
                text.textInfo = new TextBlock[1];
                text.textInfo[0] = new TextBlock();
                text.textInfo[0].text = "Outside looks dismal and dark. Best to stay inside for now.";
                text.textInfo[0].textEvents = new ITextEventContainer[0];
            }
            else
            {
                text.textInfo = new TextBlock[2];
                text.textInfo[0] = new TextBlock();
                text.textInfo[0].text = "The doors swing open with ease.";                
                text.textInfo[0].textEvents = new ITextEventContainer[1];
                text.textInfo[0].textEvents[0] = tempEvent;
                text.textInfo[1] = new TextBlock();
                text.textInfo[1].text = "Time to explore.";
                text.textInfo[1].additive = true;
            }
        }
    }
    void Start()
    {
        text = GetComponent<Display_Text>();
        tempEvent = text.textInfo[0].textEvents[0];
        locked = true;
    }

    public void TextFinished()
    {
        print("Recived");
        if (!locked && playerInteracting)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Open");
            playerInteracting = false;
        }
        else if(locked && !playerInteracting)
        {
            locked = false;
        }
        else if(playerInteracting)
        {
            playerInteracting = false;
        }
    }

    public void Interact()
    {
        playerInteracting = true;
        GetComponent<Display_Text>().Interact();
    }
}
