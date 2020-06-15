using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;
public class Chase : MonoBehaviour
{
    GameObject mc;
    Vector2 heading, playerLoc;
    public float speed;
    int segments = 8;//just in case
    void Start()
    {
        mc = ZaneSpace.Info.mc;
    }

    void Update()
    {
        playerLoc = mc.transform.position;//Find Player
        heading = playerLoc - (Vector2)transform.position;
        heading = heading / heading.magnitude; //Find only the direction to the player
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);//Reset orientation to properly follow player
        transform.Translate(heading * Info.time * speed);//Follow the player
        float enemyAngle = (Mathf.Atan2(transform.position.y - playerLoc.y, transform.position.x - playerLoc.x) * 180 / Mathf.PI) + 90;
        int direction = (int)(((enemyAngle + (segments * 2.8125)) / (segments * 5.625)) - (segments + 1));
        gameObject.transform.eulerAngles = new Vector3(0, 0, direction * 45);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject == mc && !mc.GetComponent<MC_Controller>().invincible)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
