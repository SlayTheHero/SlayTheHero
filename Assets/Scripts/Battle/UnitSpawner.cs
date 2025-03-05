using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    const int MAX_SPAWNER_CAPACITY = 4;
    [SerializeField]
    GameObject[] SpawnPos = new GameObject[MAX_SPAWNER_CAPACITY];
    [SerializeField]
    bool is_player_team;
    void Start()
    {
        /*for (int i = 0; i < transform.childCount; i++)
        {
            SpawnPos[i] = transform.GetChild(i).gameObject;
        }*/
        if (is_player_team)
            BattleManager.Instance.PlayerUnitSpawner = this;
        else
            BattleManager.Instance.HeroUnitSpawner = this;

    }

    public void SpawnAll()
    {
        var bm = BattleManager.Instance;
        if (is_player_team)
            SpawnUnits(bm.PlayerTeam);
        else
            SpawnUnits(bm.HeroTeam);
    }
    void SpawnHeroTeam()
    {
        var bm = BattleManager.Instance;
        SpawnUnits(bm.HeroTeam);
    }
    void SpawnUnits<T>(List<T> list) where T : UnitBase
    {
        var bm = BattleManager.Instance;
        foreach (var item in list)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Units/" + item.Name);
            var unit = Instantiate(prefab, SpawnPos[(item.Position - 1) % 4].transform.position, is_player_team ? Quaternion.Euler(0, 180f, 0f) : Quaternion.identity, transform);
            unit.GetComponent<UnitController>().Unit = item;
            BattleManager.Instance.Units.Add(item.Position, unit);

        }
    }
}
