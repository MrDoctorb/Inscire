using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_Controls : MonoBehaviour
{
    Text text;
    void Start()
    {
        if (ZaneSpace.Info.mc.transform.position.y < 5)
        {
            text = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            text.gameObject.SetActive(true);
            //fade in the WASD move text
            text.CrossFadeAlpha(1, 5, true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other == ZaneSpace.Info.mc.GetComponent<CapsuleCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //Fade in the text J K arm text            
            text.CrossFadeAlpha(0, 1f, true);
            yield return new WaitForSeconds(1f);
            text.text = "Use J to attack or confirm";
            text.CrossFadeAlpha(1, 5, true);
            yield return new WaitForSeconds(5f);
            text.CrossFadeAlpha(0, 1, true);
            yield return new WaitForSeconds(1f);
            text.text = "Use K to attack or deselect";
            text.CrossFadeAlpha(1, 5, true);
            yield return new WaitForSeconds(5f);
            text.CrossFadeAlpha(0, 1, true);
            gameObject.SetActive(false);

        }
    }
}
