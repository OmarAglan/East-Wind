using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData
{
    //Code OF the buildings
    private string _code;
    //Life Points OF The Buildings
    private int __healthPoints;
    //Cost Of Buildings
    private Dictionary<string, int> _cost;

    public BuildingData(string code, int healthPoints, Dictionary<string, int> cost)
    {
        _code = code;
        __healthPoints = healthPoints;
        _cost = cost;
    }
    public bool CanBuy()
    {
        foreach (KeyValuePair<string,int> pair in _cost)
        {
            if (Globals.GAME_RESOURCES[pair.Key].Amount < pair.Value)
            {
                return false;
            }
        }
        return true;
    }

    public string Code { get => _code; }
    public int HP { get => __healthPoints; }
    public Dictionary<string, int> Cost { get => _cost; }
}
