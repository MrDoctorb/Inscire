using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;

public class Enemy_Controller : MonoBehaviour, IDamageable, IKillable, IKnockbackable
{
    public int health, dmg;
    Animator anime;
    GameObject mc;
    void Start()
    {
        anime = GetComponent<Animator>();
        mc = ZaneSpace.Info.mc;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            anime.SetBool("Death", true);
            StartCoroutine(Die());
        }
    }

    public void DealDamage()
    {
        mc.GetComponent<IDamageable>().TakeDamage(dmg);
        Push();
    }

    public void Push()
    {
        mc.GetComponent<IKnockbackable>().Knockback((mc.transform.position - transform.position) 
        / (mc.transform.position - transform.position).magnitude, 10);
    }
    public IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anime.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public void Knockback(Vector2 direction, float force)
    {
        GetComponent<Rigidbody2D>().velocity = direction * force;
    }
}
