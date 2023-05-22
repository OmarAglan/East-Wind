using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData
{
    //Code OF the buildings
    private string _code;
    //Life Points OF The Buildings
    private int __healthPoints;

    public BuildingData(string code, int healthPoints)
    {
        _code = code;
        __healthPoints = healthPoints;
    }

    public string Code { get => _code; }
    public int HP { get => __healthPoints; }
}
