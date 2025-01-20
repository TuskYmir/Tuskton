using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outcome
{
    public int Money;
    public int food;
    public int resource;
    public int itemID;
}

[System.Serializable]
public class Choice
{
    public int choiceID;
    public string choiceText;
    public string description;
    public Outcome outcome;
}

[System.Serializable]
public class Story
{
    public int storyID;
    public string heading;
    public string story;
    public List<Choice> choices;
}

[System.Serializable]
public class StoryData
{
    public List<Story> stories; // This comes from the JSON file
}

[System.Serializable]
public class storyData
{
    public int storyID;
    public string heading;
    public string story;
    public string outcome;
}

[System.Serializable]
public class CloneData
{
    public Vector3 position;
    public string type;
    public int randomedValue;
    public bool alreadyGenerate;
}

[System.Serializable]
public class GameData
{
    public List<CloneData> clones = new List<CloneData>();
}

[System.Serializable]
public class RootObject
{
    public GameData gameData = new GameData();
}