using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Four_State_Button_Controller : MonoBehaviour, IDamageable
{
    public int health;
    Animator anime;
    int startHealth;
    void Start()
    {
        startHealth = health;
        anime = GetComponent<Animator>();
        anime.SetInteger("Health", health);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health < 0)
        {
            health = 0;
        }
        anime.SetTrigger("Push");
        anime.SetInteger("Health", health);
    }

    public void Reset()
    {
        anime.SetTrigger("Reset");
        health = startHealth;
    }
}