using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Slider_Minigame : MonoBehaviour
{
    public float speed, recoil;
    public int maxBounces;
    float startSpeed;
    int bounces, dmg;
    GameObject bar;

    void OnEnable() //Reset all the things
    {
        bar = transform.GetChild(0).gameObject;
        startSpeed = speed;
        Info.worldPause();
        bounces = 0;
        dmg = 0;
        transform.position = GameObject.Find("Camera").transform.position + new Vector3(0, 0, 5);
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector2.zero;
        bar.transform.localPosition = Vector2.zero;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        while (transform.localScale.x <= 2)
        {
            transform.localScale += new Vector3(2, 2, 0) * Time.deltaTime;
            bar.transform.localPosition = Vector2.zero;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        bar.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * ((Random.Range(0, 2) * 2) - 1), 0);//Sends it right or left
        while (bounces <= maxBounces)
        {
            yield return new WaitForEndOfFrame();
        }
        while (transform.localScale.x >= 0)
        {
            transform.localScale -= new Vector3(2, 2, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Finish();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == bar)
        {
            bar.transform.GetComponent<Rigidbody2D>().velocity *= new Vector2(-1 * (recoil + 1), 0);
            bounces++;
        }
    }

    void Update()
    {
        if ((Input.GetButtonDown("Primary") || Input.GetButtonDown("Secondary")) && bounces != 0 && bounces <= maxBounces)
        {
            bar.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            float x = Mathf.Abs(bar.transform.localPosition.x);
            if (x < .1)
            {
                dmg = 3;
            }
            else if(x < .345)
            {
                dmg = 2;
            }
            else if (x < .965)
            {
                dmg = 1;
            }
            else
            {
                dmg = 0;
            }
            ZaneSpace.Info.mc.GetComponent<MC_Controller>().DealDamage(transform.parent.GetComponent<Arm_Stats>().dmgList, dmg); 
            bounces = maxBounces + 1;
        }
    }

    void Finish()
    {
        Info.worldPause();
        gameObject.SetActive(false);
    }
}
