using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;
public class Move : MonoBehaviour
{
    public int speed;
    public int rot;
    bool moving;

    void Update()
    {
        if (Info.time != 0)
        {
            transform.eulerAngles = Vector3.zero;
            float x = Input.GetAxisRaw("Horizontal"), y = Input.GetAxisRaw("Vertical");
            switch (y)
            {
                case 1://Up
                    switch (x)
                    {
                        case 1:
                            rot = -45;
                            break;
                        case -1:
                            rot = 45;
                            break;
                        case 0:
                            rot = 0;
                            break;
                    }
                    break;
                case -1://Down
                    switch (x)
                    {
                        case 1:
                            rot = -135;
                            break;
                        case -1:
                            rot = 135;
                            break;
                        case 0:
                            rot = 180;
                            break;
                    }
                    break;
                case 0://LR
                    switch (x)
                    {
                        case 1:
                            rot = -90;
                            break;
                        case -1:
                            rot = 90;
                            break;
                    }
                    break;
            }
            transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Info.time * speed);
            transform.eulerAngles = new Vector3(0, 0, rot);
        }
    }
}
