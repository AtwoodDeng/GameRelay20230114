using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterSpawnerSetting
{
    public Color col;
    public int Num;
}

public class MWorldManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------
    //Set singleton for WorldManager
    private static MWorldManager _instance;
    public static MWorldManager Instance
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
    //----------------------------------------------------------------------------------------------
    public List<MonsterSpawnerSetting> spawnerSettings = new List<MonsterSpawnerSetting>();

    public GameObject monsterPrefab;

    public Vector2 groundSize = new Vector2(9.5f, 4.5f);

    // game stats 
    private int failCount = 0; // how many times that a player fail to match P with Monster
    private MFart selectedFart; // the fart that is currently selected 

    [HideInInspector]
    public List<MMonster> Monsters;



    public void CallStartGame()
    {
        foreach (var s in spawnerSettings)
            Spawn(s);
    }

    public void Spawn(MonsterSpawnerSetting setting)
    {
        for( int i = 0; i < setting.Num; ++ i )
        {
            var obj = Instantiate(monsterPrefab) as GameObject;

            var monster = obj.GetComponent<MMonster>();
            monster.InitVisual(setting.col);
            var pos = new Vector3(Random.Range(-groundSize.x, groundSize.x), Random.Range(-groundSize.y, groundSize.y) , Random.Range(0,2f) );
            monster.transform.position = pos;
            Monsters.Add(monster);
        }
    }

    public void CallKill(MMonster monster)
    {
        Monsters.Remove(monster);
        Destroy(monster.gameObject);

        //When all monsters are killed, the game ends
        if (Monsters.Count == 0)
            GameManager.Instance.CallEndGame();
    }

    public void CallOnSelectedFart(MMonster monster, MFart fart)
    {
        //Only 1 fart is allowed to be selected
        //Debug.Log("Select farts");
        selectedFart = fart;
        foreach(var m in Monsters)
        {
            if (m == monster)
            {
                m.SetFartsState(fart);
            }else
            {
                m.SetFartsState(null);

            }
        }
    }

    public void CallMatchFail(MMonster monster)
    {
        // when the match fails 
        failCount += 1;
        if(failCount >= 7){
            GameManager.Instance.CallUnlockRainbow();
        }

        // reset the wait status of all monsters 
        selectedFart.SetState(false);
        //selectedFart = null;
        foreach(var m in Monsters)
        {
            m.SetWaitForFart(false);
        }
        
    }
}
