using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Text_Event : MonoBehaviour, IInteractable, ITextEvent, ISaveable
{
    public List<ITextEventContainer> textEvents = new List<ITextEventContainer>();
    [SerializeField] bool invisible = true;
    string saveKey;
    bool finished;

    void OnEnable()
    {
        if (invisible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == Info.mc.GetComponent<CapsuleCollider2D>())
        {
            GetComponent<Display_Text>().enabled = true;
            GetComponent<Display_Text>().Interact();
            //GetComponent<Display_Text>().ProgressText();
            GetComponent<Text_Event>().enabled = false;
        }
    }

    public void Interact()
    {
        print("I intercepted your interaction!");
        //This intercepts interactions and prevents the player from clicking to trigger 
        //the event if it is above Display_Text in the hierarchy
    }

    public void TextFinished()
    {
        if (textEvents.Count > 0)
        {
            foreach (ITextEventContainer eve in textEvents)
            {
                InterfaceInfo.interfaceText = eve;
                InterfaceInfo.TextEvent.TextFinished();
            }
        }
        finished = true;
        StartCoroutine(TurnOff());
    }

    //Wait two Frame so that the text actually dissapears
    IEnumerator TurnOff()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }

    public void SaveData(Scene_Data data)
    {
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
        data.Store<object>(saveKey, finished);
    }

    public void LoadData(Scene_Data data)
    {
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
        finished = (bool)data.Take<object>(saveKey);
        if (finished)
        {
            gameObject.SetActive(false);
        }
    }
}
