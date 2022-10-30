using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingGameEvent : UnityEvent //for testing purposes and will not be implemented later
{
    public string BuildingName;

    public BuildingGameEvent(string name)
    {
        BuildingName = name;
    }
}

public class TutorialEvent : UnityEvent
{

}