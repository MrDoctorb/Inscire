using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Move : MonoBehaviour
{
    [SerializeField] string scene = "";
    [SerializeField] Vector2 newLocation = new Vector2();
    [SerializeField] int rotation = 0;
    GameObject mc;
    void Start()
    {
        mc = ZaneSpace.Info.mc;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == mc)
        {
            Save_Manager.SoftSave();
            print(SceneManager.GetActiveScene().name + " saved");

            mc.transform.position = newLocation;
            mc.GetComponent<Move>().rot = rotation;
            mc.GetComponent<Move>().enabled = false;
            ZaneSpace.ZScene.Load(scene);
            mc.GetComponent<Move>().enabled = true;
        }
    }
}