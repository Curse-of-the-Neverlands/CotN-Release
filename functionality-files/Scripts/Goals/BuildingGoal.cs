using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


//this is for testing purposes only and will be removed later on

public class BuildingGoal : Quest.QuestGoal
{
    public string Building;

    public override string GetDescription()
    {
        return $"Build a {Building}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<BuildingGameEvent>(OnBuilding);
    }

    private void OnBuilding(BuildingGameEvent eventInfo)
    {
        if(eventInfo.BuildingName == Building)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}