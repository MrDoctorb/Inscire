using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Door : MonoBehaviour, ITextEvent, IInteractable
{
    Display_Text text;
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
                text.text = new string[] {"Outside looks dismal and dark. Best to stay inside for now."};
            }
            else
            {
                text.text = new string[] {"The doors swing open with ease.", "The doors swing open with ease. Time to explore." };
            }
        }
    }
    void Start()
    {
        text = GetComponent<Display_Text>();
        locked = true;
    }

    public void TextFinished()
    {
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
