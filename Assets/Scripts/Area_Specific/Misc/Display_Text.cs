using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaneSpace;

public class Display_Text : MonoBehaviour, IInteractable
{
    GameObject textBox;
    public bool dialogue = false, silhouette = false;
#pragma warning disable 0649
    [SerializeField] RuntimeAnimatorController faceTalk;
#pragma warning restore 0649
    [TextArea] public string[] text;
    int textLine;
    bool selected;

    void LateUpdate()
    {
        if (Info.worldPaused && Input.GetButtonDown("Primary") && selected)
        {
            ProgressText();
        }
    }
    public void Interact()
    {
        if (enabled)
        {
            textLine = -1;
            selected = true;
            textBox = Info.gm.transform.GetChild(2).gameObject;
            textBox.SetActive(true);
            if (dialogue)
            {
                textBox = textBox.transform.GetChild(1).gameObject;
                Animator anime = textBox.transform.GetChild(2).GetComponent<Animator>();
                anime.runtimeAnimatorController = faceTalk;
                if (silhouette)
                {
                    anime.gameObject.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    anime.gameObject.GetComponent<Image>().color = Color.white;
                }
            }
            else
            {
                textBox = textBox.transform.GetChild(0).gameObject;
            }
            textBox.SetActive(true);
            textBox = textBox.transform.GetChild(0).gameObject;
            Info.worldPause();
        }
        else
        {
            print("I hope you didn't want it to work this way. Display Text is disabled");
        }
    }

    public void ProgressText()
    {
        textLine += 1;
        if (textLine < text.Length)
        {
            textBox.GetComponent<Text>().text = text[textLine];
        }
        else
        {
            selected = false;
            textBox.transform.parent.gameObject.SetActive(false);
            textBox = Info.gm.transform.GetChild(2).gameObject;
            textBox.SetActive(false);
            ITextEvent[] localTextEvents = GetComponents<ITextEvent>();
            if (localTextEvents.Length > 0)
            {
                foreach (ITextEvent eve in localTextEvents)
                {
                    eve.TextFinished();
                }
            }
            Info.worldPause();
        }
    }
}
