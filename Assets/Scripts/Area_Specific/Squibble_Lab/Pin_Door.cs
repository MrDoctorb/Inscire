using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Pin_Door : MonoBehaviour, IInteractable, ISaveable
{
    int rowSelected, columnSelected;
    string saveKey;
    bool opened;
    GameObject pinPad, selectedButton;
    List<int> currentCode;
    [SerializeField] List<int> correctCode = new List<int>(4);
    [SerializeField] Sprite[] pinPadImages = new Sprite[5];

    void OnEnable()
    {
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
    }
    void Start()
    {
        pinPad = transform.GetChild(0).GetChild(0).gameObject;
        newButton();
        pinPad.GetComponent<Image>().sprite = pinPadImages[0];
        currentCode = new List<int>();
    }

    void Update()
    {
        if (pinPad.transform.localScale.x >= 1)
        {
            if (Input.GetKeyDown("w"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().idle;
                rowSelected += 2; //3 rows - 1
                newButton();
            }
            else if (Input.GetKeyDown("s"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().idle;
                rowSelected += 1;
                newButton();
            }
            else if (Input.GetKeyDown("a"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().idle;
                columnSelected += 2; //3 columns - 1
                newButton();
            }
            else if (Input.GetKeyDown("d"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().idle;
                columnSelected += 1;
                newButton();
            }
            else if (Input.GetButtonDown("Primary"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().pressed;
                PushButton();
            }
            else if (Input.GetButtonUp("Primary"))
            {
                selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().highlight;
            }
            else if (Input.GetButtonUp("Secondary"))
            {
                StartCoroutine(Grow(-1, false));
            }
        }
    }

    void PushButton()
    {
        int buttonPushed = (rowSelected * 3) + (columnSelected + 1);
        currentCode.Add(buttonPushed);
        if (currentCode.Count >= 4)
        {
            if (currentCode.SequenceEqual(correctCode))
            {
                StartCoroutine(Grow(-1, true));
            }
            else
            {
                currentCode.Clear();
            }
        }
        pinPad.GetComponent<Image>().sprite = pinPadImages[currentCode.Count];

    }
    void newButton()
    {
        rowSelected %= 3;
        columnSelected %= 3;
        selectedButton = pinPad.transform.GetChild(rowSelected).GetChild(columnSelected).gameObject;
        selectedButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Pin_Button_Images>().highlight;
    }

    public void Interact()
    {
        ZaneSpace.Info.worldPause();
        StartCoroutine(Grow(1, false));
        currentCode.Clear();
        pinPad.GetComponent<Image>().sprite = pinPadImages[currentCode.Count];
    }

    IEnumerator Grow(int speed, bool finished)
    {
        float growSpeed = speed * Time.deltaTime;
        if (speed > 0)
        {
            while (pinPad.transform.localScale.x <= (speed + 1) / 2)
            {
                pinPad.transform.localScale += new Vector3(growSpeed, growSpeed, 0);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (pinPad.transform.localScale.x >= (speed + 1) / 2)
            {
                pinPad.transform.localScale += new Vector3(growSpeed, growSpeed, 0);
                yield return new WaitForEndOfFrame();
            }
            pinPad.transform.localScale = Vector3.zero;
            ZaneSpace.Info.worldPause();
        }
        if (finished)
        {
            opened = true;
            GetComponent<Animator>().SetTrigger("Open");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Pin_Door>().enabled = false;
        }
    }

    public void SaveData(Scene_Data data)
    {
        data.Store<object>(saveKey, opened);
    }

    public void LoadData(Scene_Data data)
    {
        opened = (bool)data.Take<object>(saveKey);
        if (opened)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Open");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Pin_Door>().enabled = false;
        }
    }
}
