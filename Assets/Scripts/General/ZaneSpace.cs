using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZaneSpace
{
    public class Info
    {
        public static GameObject mc, gm;
        public static Rigidbody2D rb;
        public static float time = .02f;
        public static bool worldPaused = false, gamePaused = false;
        static float tempTime = time;
        static Vector2 tempVelocity = new Vector2();
        
        public static void gamePause()
        {
            gamePaused = !gamePaused;
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                time = tempTime;
            }
            else
            {
                Time.timeScale = 0;
                tempTime = time;
            }
        }

        public static void worldPause()
        {
            worldPaused = !worldPaused;
            if (time == 0)
            {
                rb.velocity += tempVelocity;
                time = .02f;
            }
            else
            {
                tempVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                time = 0;
            }
        }
    }
    public class Wait
    {
        public static IEnumerator WaitMySeconds(float seconds)
        {
            float timeLeft = seconds;
            while (timeLeft >= 0)
            {
                yield return new WaitForEndOfFrame();
                if (!Info.worldPaused)
                {
                    timeLeft -= Time.deltaTime;
                }
            }
        }
    }

    public class InterfaceInfo : MonoBehaviour
    {
        public static ITextEvent TextEvent
        {
            get { return interfaceText.Result; }
            set { interfaceText.Result = value; }
        }
        public static ITextEventContainer interfaceText;
    }

    public class ZScene : MonoBehaviour
    {
        public static void Load(string sceneName)
        {
            //Info.gm.GetComponent<Game_Manager>().ChangeSortingOrder(-1);
            SceneManager.LoadScene(sceneName);
        } 
    }
}