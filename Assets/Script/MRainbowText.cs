using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MRainbowText : MonoBehaviour
{
    [SerializeField]
    Text text;

    private Color lerpedColor = Color.white;
    private float speed = 5.0f;
    private float step = 10.0f;

    void Start () 
    {
       
        text = gameObject.GetComponent<Text> ();
        text.color = lerpedColor;
       
    }

    void Update()
    {
        //lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        //speed = 0.1;
        float h = Mathf.Floor(Mathf.PingPong(Time.time * speed, 1)*step)/step;
        lerpedColor = Color.HSVToRGB( h, 1, 1);
        text.color = lerpedColor;
    }
}
