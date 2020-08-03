using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ZaneSpace;

public class Directional_Song_Change : MonoBehaviour
{
    Music_Manager mm;
    Transform mc;
    Vector2 enterPos;
    [SerializeField] string newSong = "";
    [SerializeField] bool north = false, east = false, south = false, west = false;

    void Start()
    {
        mm = Info.gm.GetComponent<Music_Manager>();
        mc = Info.mc.transform;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == Info.mc.GetComponent<CapsuleCollider2D>())
        {
            enterPos = mc.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == Info.mc.GetComponent<CapsuleCollider2D>())
        {
            if (mc.position.y > enterPos.y && north)
            {
                mm.ChangeSong(newSong);
            }
            else if (mc.position.y < enterPos.y && south)
            {
                mm.ChangeSong(newSong);
            }
            else if (mc.position.x > enterPos.x && east)
            {
                mm.ChangeSong(newSong);
            }
            else if (mc.position.x < enterPos.x && west)
            {
                mm.ChangeSong(newSong);
            }
        }
    }
}
