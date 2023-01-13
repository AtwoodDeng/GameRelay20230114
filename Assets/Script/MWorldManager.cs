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
    public List<MonsterSpawnerSetting> spawnerSettings = new List<MonsterSpawnerSetting>();

    public GameObject monsterPrefab;

    public Vector2 groundSize = new Vector2(9.5f, 4.5f);

    public void Spawn(MonsterSpawnerSetting setting)
    {
        for( int i = 0; i < setting.Num; ++ i )
        {
            var obj = Instantiate(monsterPrefab) as GameObject;

            var monster = obj.GetComponent<MMonster>();
            monster.InitVisual(setting.col);

            var pos = new Vector3(Random.RandomRange(-groundSize.x, groundSize.x), Random.RandomRange(-groundSize.y, groundSize.y) , Random.RandomRange(0,2f) );

            monster.transform.position = pos;
        }
    }

    private void Start()
    {
        foreach(var s in spawnerSettings)
            Spawn(s);
    }

}
