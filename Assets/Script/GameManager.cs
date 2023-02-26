using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------
    //Set singleton for GameManager
    private static GameManager _instance;
    public static GameManager Instance
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
    [SerializeField] GameObject SplashScreen;
    [SerializeField] GameObject ScoreScreen;
    [SerializeField] GameObject RainbowScreen;

    private bool b_rainbow_unlocked = false;
    private MTimer fadeTimer;

    public void Start()
    {
        SplashScreen.SetActive(true);
        ScoreScreen.SetActive(false);
        RainbowScreen.SetActive(false);

        fadeTimer = new MTimer();
    }

    public void CallStartGame()
    {
        SplashScreen.gameObject.SetActive(false);
        MWorldManager.Instance.CallStartGame();
    }

    public void CallEndGame()
    {
        ScoreScreen.SetActive(true);
    }

    public void CallUnlockRainbow()
    {
        if(!b_rainbow_unlocked){
            b_rainbow_unlocked = true;
            RainbowScreen.SetActive(true);
            fadeTimer.setActive(3);
        }
        
    }

    public void Update()
    {
        if(fadeTimer.b_timer_active)
        {
            fadeTimer.UpdateTimer();
            if(fadeTimer.b_timer_end){
                RainbowScreen.SetActive(false);
            }
        }
        
    }

}
