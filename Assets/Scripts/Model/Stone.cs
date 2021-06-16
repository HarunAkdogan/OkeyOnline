using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public bool takeable = true;
    public bool stock = true;
    public string type = "normal";
    public int number;
    public string color;
    public Sprite original, cover;
    
    public void setVisible(bool vis)
    {
        if(vis)
        {
            GetComponent<SpriteRenderer>().sprite = original;
        }else
        {
            GetComponent<SpriteRenderer>().sprite = cover;
        }

    }

    public enum StoneType
    {
        Red,
        Blue,
        Black,
        Yellow
    }

}
