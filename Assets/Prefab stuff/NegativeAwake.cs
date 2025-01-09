using System.IO;
using JetBrains.Annotations;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic; // Add this for List<T>

public class Negative : MonoBehaviour, INode
{
    [SerializeField]
    private float _randomedValue;
    [SerializeField]
    private bool _alreadyGenerate;
    [SerializeField]
    private int _storyID;
    [SerializeField]
    private string _heading;
    [SerializeField]
    private string _story;
    [SerializeField]
    private string _outcome;

    [Header("Story text")]
    public GameObject headingText;
    public GameObject storyTextUI;
    public float randomedValue
    {
        get => _randomedValue;
        set => _randomedValue = value;
    }

    public bool AlreadyGenerate
    {
        get => _alreadyGenerate;
        set => _alreadyGenerate = value;
    }

    public int storyID
    {
        get => _storyID;
        set => _storyID = value;
    }

    public string heading
    {
        get => _heading;
        set => _heading = value;
    }

    public string storyText
    {
        get => _story;
        set => _story = value;
    }

    public string outcome
    {
        get => _outcome;
        set => _outcome = value;
    }

    public int prefapIndexNumber = 0;
    public float RandomedValue => randomedValue;

    public void randomValue()
    {
        _storyID = Random.Range(201,215);
        randomedValue = Random.Range(-500, 0);
        Debug.Log("randomed");
        AlreadyGenerate = false;
    }

    private string savePath;

    void Awake()
    {
        randomValue();
        savePath = Application.dataPath + "/savegame.json";
        LoadStoryData();
    }


    private void LoadStoryData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            StoryDataListN storyDataList = JsonUtility.FromJson<StoryDataListN>(json);

            StoryDataN story = storyDataList.stories.Find(s => s.storyID == _storyID);
            if (story != null)
            {
                Debug.Log($"Heading: {story.heading}, Story: {story.story}, Outcome: {story.outcome}");
                heading = story.heading;
                storyText = story.story;
                outcome = story.outcome;

            }
            else
            {
                Debug.LogWarning($"Story ID {_storyID} not found in the save file.");
            }
            headingText.GetComponent<TextMesh>().text = heading;
            storyTextUI.GetComponent<TextMesh>().text = storyText;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }
}

[System.Serializable]
public class StoryDataN
{
    public int storyID;
    public string heading;
    public string story;
    public string outcome;
}

[System.Serializable]
public class StoryDataListN
{
    public List<StoryDataN> stories;
}
