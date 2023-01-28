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

    public void Start()
    {
        SplashScreen.SetActive(true);
        ScoreScreen.SetActive(false);
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
}
