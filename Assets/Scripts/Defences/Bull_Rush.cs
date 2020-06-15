using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Bull_Rush : MonoBehaviour
{//ATTATCHED TO SKULL!!
    GameObject mcBall, board;
    Rigidbody2D skull;
    public float skullSpd, ballSpd, waitTime;

    void OnEnable()//Reset the thingys
    {
        skull = GetComponent<Rigidbody2D>();
        board = transform.parent.gameObject;
        mcBall = board.transform.GetChild(1).gameObject;
        transform.localPosition = new Vector2(0, 1);
        mcBall.transform.localPosition = new Vector2(0, -1);
        Info.worldPause();
        board.transform.position = GameObject.Find("Camera").transform.position + new Vector3(0, 0, 5);
        board.transform.eulerAngles = Vector3.zero;
        board.transform.localScale = Vector2.zero;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        while (transform.parent.localScale.x <= 2)
        {
            transform.parent.localScale += new Vector3(2, 2, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        skull.velocity = new Vector2(Random.Range(-1 * skullSpd, skullSpd), 0);
        yield return new WaitForSeconds(waitTime);
        skull.velocity = new Vector2(0, -skullSpd);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == board)
        {
            if (skull.velocity.y == 0)
            {
                skull.velocity = new Vector2(skull.velocity.x * -1, 0);//Bounce off the wall at the start
            }
            else
            {
                Knockback();
            }
        }
        if (other.gameObject == mcBall)
        {
            Damage();
        }
    }
    void Damage()
    {
        transform.parent.parent.GetComponent<Enemy_Controller>().DealDamage();
        StartCoroutine(Finish());
    }
    void Knockback()
    {
        transform.parent.parent.GetComponent<Enemy_Controller>().Push();
        StartCoroutine(Finish());
    }
    void Update()
    {
        if (transform.parent.localScale.x >= 2 && !Info.worldPaused)
        {
            float x = Input.GetAxisRaw("Horizontal"), y = Input.GetAxisRaw("Vertical");
            mcBall.transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * ballSpd);
        }
    }

    IEnumerator Finish()
    {
        while (transform.parent.localScale.x >= 0)
        {
            transform.parent.localScale -= new Vector3(2, 2, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Info.worldPause();
        transform.parent.gameObject.SetActive(false);
    }
}
