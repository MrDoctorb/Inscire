using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable_Hitbox : MonoBehaviour
{
    public void Disable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
