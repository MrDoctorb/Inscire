using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZaneSpace;


public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> leftArmUnlockables = new List<GameObject>(),
    rightArmUnlockables = new List<GameObject>(), caudalUnlockables = new List<GameObject>(),
    dorsalUnlockables = new List<GameObject>(), eyeUnlockables = new List<GameObject>();
    Dictionary<string, GameObject> leftArm, rightArm, caudal, dorsal, eye;
    static List<Dictionary<string, GameObject>> unlockableParts = new List<Dictionary<string, GameObject>>();

    void OnEnable()
    {
        Info.mc = GameObject.Find("MC");
        Info.gm = gameObject;
        Info.rb = Info.mc.GetComponent<Rigidbody2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(ZaneSpace.Info.mc);
        DontDestroyOnLoad(gameObject);

        //This seems jank
        leftArm = ListToDictionary(leftArmUnlockables);
        rightArm = ListToDictionary(rightArmUnlockables);
        caudal = ListToDictionary(caudalUnlockables);
        dorsal = ListToDictionary(dorsalUnlockables);
        eye = ListToDictionary(eyeUnlockables);
        unlockableParts.Add(leftArm);
        unlockableParts.Add(rightArm);
        unlockableParts.Add(caudal);
        unlockableParts.Add(dorsal);
        unlockableParts.Add(eye);
        //Oh well

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer rend in renderers)
        {
            float temp = rend.transform.position.y * -100;
            rend.sortingOrder = (int)temp + rend.sortingOrder;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        print(sceneName + " loaded");
        Save_Manager.LoadSceneData(sceneName);

    }
    void Update()
    {
        if (Info.time != 0)
        {
            Info.time = Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Info.gamePause();
            GameObject pauseMenu = Info.gm.transform.GetChild(1).gameObject;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown("v"))
        {
            Save_Manager.LoadPlayerData();
        }
        if (Input.GetKeyDown("c"))
        {
            Save_Manager.SavePlayerData();
        }
        if (Input.GetKeyDown("b"))
        {
            Save_Manager.ClearAllData();
        }
#endif
    }

    public static void MakeMC(GameObject mc)
    {
        Instantiate(mc);
    }

    Dictionary<string, GameObject> ListToDictionary(List<GameObject> list)
    {
        Dictionary<string, GameObject> tempDict = new Dictionary<string, GameObject>();
        foreach (GameObject obj in list)
        {
            tempDict.Add(obj.name, obj);
        }
        return tempDict;
    }

    public void UnlockPart(int location, string partName)//numbers same as MC_Controller parts
    {
        Info.mc.GetComponent<MC_Controller>().unlockedParts[location].Add(unlockableParts[location][partName]);
    }
}
