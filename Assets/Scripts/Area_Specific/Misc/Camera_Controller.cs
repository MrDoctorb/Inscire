using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Camera_Controller : MonoBehaviour
{
    GameObject mc;
    public bool still;
    public Vector3 slidePos;
    float slideSpd;
    [SerializeField] float startBrightness = 0;
    
    void Start()
    {
        GameObject.Find("World_Light").GetComponent<Light2D>().intensity = startBrightness;
        slideSpd = .15f;//After testing, seems good
        mc = ZaneSpace.Info.mc;
        if (still)
        {
            transform.position = slidePos + new Vector3(0, 0, -1);
        }
        else
        {
            transform.position = mc.transform.position + new Vector3(0, 0, -1);
        }
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, mc.transform.position);

        if (!still)//follow player
        {
            if (dist > 1)
            {
                transform.position = Vector3.Lerp(transform.position, mc.transform.position + new Vector3(0, 0, -1), slideSpd / 2);
            }
            else
            {
                transform.position = mc.transform.position + new Vector3(0, 0, -1);
            }
        }
        else if (Vector3.Distance(transform.position, slidePos) > .01)//Move to Loc
        {
            still = true;
            slidePos.z = -1;
            transform.position = Vector3.Lerp(transform.position, slidePos, slideSpd / 2);
        }
    }
}
