using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaneSpace;

[System.Serializable]
public class TextBlock
{
    [TextArea] public string text;
    public bool dialogue, additive;
    [ConditionalHide("dialogue")]
    public bool newFace;
    [ConditionalHide("newFace")]
    public bool silhouette;
    [Range(.01f, 2f)]
    public float textSpeed = 1f;
    [ConditionalHide("newFace")]
    public RuntimeAnimatorController faceTalk;
    public ITextEventContainer[] textEvents = new ITextEventContainer[0];

}

public class Display_Text : MonoBehaviour, IInteractable
{
    GameObject descriptionTextBox, dialougeTextBox, selectedBox;
    public TextBlock[] textInfo;
    TextBlock textBlock;
    Text currentLine;
    int textLine;
    bool selected, currentlyTyping = false;
    string previousLine;

    void OnEnable()
    {
        StartCoroutine(Startup());
    }

    IEnumerator Startup()
    {
        yield return new WaitForEndOfFrame();
        Transform textParent = Info.gm.transform.GetChild(2);
        descriptionTextBox = textParent.GetChild(0).gameObject;
        dialougeTextBox = textParent.GetChild(1).gameObject;
    }
    public void Interact()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        //Stop the world
        Info.worldPause();
        //Show first line of dialouge
        textLine = 0;
        LoadTextBlock(textLine);
        //Cause only this object to effect the textbox
        selected = true;

    }

    void LoadTextBlock(int line)
    {
        textBlock = textInfo[line];
        //Check if text is dialouge
        if (textBlock.dialogue)
        {
            //If changing between description and dialouge, turn the other box off
            if (selectedBox = descriptionTextBox)
            {
                selectedBox.SetActive(false);
            }
            selectedBox = dialougeTextBox;
            //Check to see if the talking animation needs changed
            if (textBlock.newFace)
            {
                Transform face = dialougeTextBox.transform.GetChild(1).GetChild(0);
                face.GetComponent<Animator>().runtimeAnimatorController = textBlock.faceTalk;
                face.GetComponent<Image>().color = Color.white;
                if (textBlock.silhouette)
                {
                    face.GetComponent<Image>().color = Color.black;
                }
            }
        }
        //Must be a description then
        else
        {
            //If changing between description and dialouge, turn the other box off
            if (selectedBox = dialougeTextBox)
            {
                selectedBox.SetActive(false);
            }
            selectedBox = descriptionTextBox;
        }
        //Refrence to the text we are editing
        currentLine = selectedBox.transform.GetChild(0).GetComponent<Text>();
        //Display the next linec


        StartCoroutine(GenerateLetters(textBlock.text));

        //Turn on the Box to display it to the world
        selectedBox.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Primary") && selected)
        {
            //If currently typing, stop typing and set the line of text
            if (currentlyTyping)
            {
                StopAllCoroutines();
                //Additive needs to have a refrence to its previous line, so we call that here
                if (textBlock.additive)
                {
                    currentLine.text = previousLine + " " + textBlock.text;
                }
                else
                {
                    currentLine.text = textBlock.text;
                }
                //Once again store the line for later
                previousLine = currentLine.text;
                currentlyTyping = false;
            }
            //Otherwise move on to finishing the line
            else
            {
                //Check for text events and run them
                if (textBlock.textEvents.Length > 0)
                {
                    foreach (ITextEventContainer textEvent in textBlock.textEvents)
                    {

                        InterfaceInfo.interfaceText = textEvent;
                        InterfaceInfo.TextEvent.TextFinished();
                    }
                }
                //If at the end of the text, close it
                if (textLine >= textInfo.Length - 1)
                {
                    StartCoroutine(ExitText());
                }
                else
                {
                    //Otherwise display next line of text
                    textLine += 1;
                    LoadTextBlock(textLine);
                }
            }
        }
    }

    IEnumerator ExitText()
    {
        //We wait a frame here to prevent instantly interacting with the text again
        yield return new WaitForEndOfFrame();
        selected = false;
        selectedBox.SetActive(false);
        Info.worldPause();
    }

    //Use this function to take a string and convert it into singular characters to be displayed
    IEnumerator GenerateLetters(string textLine)
    {
        if (textBlock.textSpeed <= 0)
        {
            print("Please fix me! My text speed is invalid!");
        }
        //If we are adding text, give it a space
        if (textBlock.additive)
        {
            currentLine.text += " ";
        }
        //Otherwise clear the line
        else
        {
            currentLine.text = "";
        }
        currentlyTyping = true;
        foreach (char letter in textLine.ToCharArray())
        {
            currentLine.text += letter;
            //Wait at minimum a frame, can change textSpeed to be slower or faster
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(.05f * textBlock.textSpeed);
        }
        currentlyTyping = false;
        //Save the line for later if the player skips text
        previousLine = currentLine.text;
    }
}
