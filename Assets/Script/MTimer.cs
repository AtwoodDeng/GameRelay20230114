using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTimer : MonoBehaviour
{
    private static MTimer _instance;
    public static MTimer Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }


    public float timeRemaining = 2;
    public bool b_timer_end = false;
    public bool b_timer_active = false;

    public void setActive(float _time)
    {
        if(!b_timer_end && !b_timer_active)
        {
            timeRemaining = _time;
            b_timer_active = true;
        }
        
    }

    public void UpdateTimer()
    {
        if(b_timer_active){
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                b_timer_end = true;
                b_timer_active = false;
                //Debug.Log("Timer Ends!");
            }    
        }
        // send event ??
    }


}
