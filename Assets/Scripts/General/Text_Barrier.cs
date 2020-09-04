using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Text_Barrier : MonoBehaviour, IInteractable, ITextEvent
{
    [SerializeField] bool invisible = true;
    //string saveKey;
    bool throwback;
    Transform mc;
    [SerializeField] GameObject startCondition;
    [SerializeField] bool startConditionOn = false;

    void OnEnable()
    {
        if (invisible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        StartCoroutine(StartupWait());
    }

    IEnumerator StartupWait()
    {
        yield return new WaitForSeconds(.25f);
        if (startCondition.activeSelf != startConditionOn)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == Info.mc.GetComponent<CapsuleCollider2D>())
        {
            throwback = true;
            mc = other.transform;
            GetComponent<Display_Text>().enabled = true;
            GetComponent<Display_Text>().Interact();
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
        if (throwback)
        {
            StartCoroutine(Throwback());
        }
        else
        {
            //Flip on or off
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    IEnumerator Throwback()
    {
        mc.GetComponent<Move>().enabled = false;
        float dist = 0;
        while (dist < 1)
        {
            dist += 2 * Time.deltaTime;
            mc.position = Vector3.Lerp(mc.position, mc.position + ((mc.position - transform.position) / (mc.position - transform.position).magnitude), 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        mc.GetComponent<Move>().enabled = true;
        throwback = false;
    }

    /*public void SaveData(Scene_Data data)
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
    }*/
}
