using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualDPad : MonoBehaviour
{
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    public string action;

    // Start is called before the first frame update
    // void Start()
    // {
    //     phaseDisplayText = gameObject.getComponent(typeof(Text)) as Text;
    // }
    // Update is called once per frame
    void Update()
    {
        action = "";

        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }

            else if (theTouch.phase == TouchPhase.Moved)//|| theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    action = "F";
                }

                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    action = x > 0 ? "R" : "L";
                }

                else
                {
                    action = y > 0 ? "U" : "D";
                }
            }
        }
    }
}
