using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend : MonoBehaviour
{
    public Circle circle;

    Circle[] circles;

    // Start is called before the first frame update
    void Start()
    {
        circles = new Circle[Data.colors.Count];
        Vector3 newPosition;
        float scale = (Data.width + 1f) / Data.colors.Count;

        for (int i=0; i<circles.Length; i++)
        {
            newPosition = new Vector3(0.5f + transform.position.x + i, transform.position.y, transform.position.z);
            circles[i] = Instantiate(circle, newPosition, Quaternion.identity, transform);
            circles[i].SetLevel(i+1);
            if (circles[i].level > Data.maxLevel)
            {
                circles[i].MuteColor(true);
            }
            circles[i].enabled = false;
        }

        transform.localScale = new Vector3(scale,scale,1f);
    }

    public void UpdateLegend()
    {
        for (int i=0; i<circles.Length; i++)
        {
            circles[i].enabled = true;
            if (circles[i].level > Data.maxLevel)
            {
                circles[i].MuteColor(true);
            }
            else
            {
                circles[i].MuteColor(false);
            }
            circles[i].enabled = false;
        }
    }
}
