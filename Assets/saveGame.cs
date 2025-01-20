using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public TextAsset jsonFile; // JSON file containing the stories
    public Dictionary<int, Story> storyDictionary; // The dictionary for fast lookups

    public GameObject prefab; // Assign the single prefab in Unity Inspector

    private string savePath;

    void Start()
    {
        Debug.Log("SaveSystemManager Start");
        LoadStoryData();
        savePath = Application.persistentDataPath + "/savegame.json";
        Debug.Log("Save Path: " + savePath);
    }

    public void LoadStoryData()
    {
        // Deserialize the JSON into a StoryData object
        var storyData = JsonUtility.FromJson<StoryData>(jsonFile.text);
        Debug.Log("Loaded " + storyData.stories.Count + " stories.");

        // Initialize the dictionary
        storyDictionary = new Dictionary<int, Story>();

        // Populate the dictionary
        foreach (var story in storyData.stories)
        {
            if (!storyDictionary.ContainsKey(story.storyID)) // Check for duplicate IDs
            {
                storyDictionary.Add(story.storyID, story);
                Debug.Log($"Added storyID {story.storyID} to dictionary.");
            }
            else
            {
                Debug.LogWarning($"Duplicate storyID found: {story.storyID}. Skipping.");
            }
        }
    }

    public void SaveGame()
    {
        RootObject rootObject;

        // Load existing data
        if (File.Exists(savePath))
        {
            string existingJson = File.ReadAllText(savePath);
            rootObject = JsonUtility.FromJson<RootObject>(existingJson);
        }
        else
        {
            rootObject = new RootObject();
        }

        // Clear existing game data
        rootObject.gameData = new GameData();

        // Find all objects with the tag "Node"
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject node in nodes)
        {
            if (node.TryGetComponent<NodeInfoManager>(out NodeInfoManager storyScript))
            {
                rootObject.gameData.clones.Add(CreateCloneData(node.transform.position, storyScript.randomedValue, storyScript.alreadyGenerate));
            }
        }

        try
        {
            string json = JsonUtility.ToJson(rootObject, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game Saved: " + json);
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            RootObject rootObject = JsonUtility.FromJson<RootObject>(json);

            // Destroy all existing nodes
            GameObject[] existingNodes = GameObject.FindGameObjectsWithTag("Node");
            foreach (GameObject node in existingNodes)
            {
                Destroy(node);
            }

            // Recreate clones from saved data
            foreach (CloneData cloneData in rootObject.gameData.clones)
            {
                GameObject newClone = Instantiate(prefab, cloneData.position, Quaternion.Euler(90, 0, 0));
                newClone.tag = "Node";
                AssignCloneData(newClone, cloneData);
            }

            Debug.Log("Game Loaded");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
        }
    }

    private CloneData CreateCloneData(Vector3 position, int randomedValue, bool alreadyGenerate)
    {
        return new CloneData
        {
            position = position,
            randomedValue = randomedValue,
            alreadyGenerate = alreadyGenerate
        };
    }

    private void AssignCloneData(GameObject clone, CloneData cloneData)
    {
        if (clone.TryGetComponent<NodeInfoManager>(out NodeInfoManager storyScript))
        {
            storyScript.randomedValue = cloneData.randomedValue;
            storyScript.alreadyGenerate = cloneData.alreadyGenerate;
        }
    }
}
