using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    public float randomedValue;
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
    public List<storyData> storyData = new List<storyData>();
    public GameData gameData = new GameData();
}


public class saveGame : MonoBehaviour
{
    public List<GameObject> prefabs; // Assign prefabs in Unity Inspector
    private Dictionary<string, GameObject> prefabDictionary;

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/savegame.json";
        Debug.Log("Save Path: " + savePath);

        // Initialize the dictionary
        prefabDictionary = new Dictionary<string, GameObject>();
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.TryGetComponent<INode>(out INode nodeScript))
            {
                string typeName = nodeScript.GetType().Name;
                prefabDictionary[typeName] = prefab;
            }
        }
    }

    public void SaveGame()
    {
        RootObject rootObject = new RootObject();

        // Load existing data
        if (File.Exists(savePath))
        {
            string existingJson = File.ReadAllText(savePath);
            rootObject = JsonUtility.FromJson<RootObject>(existingJson);
        }

        // Clear existing game data
        rootObject.gameData = new GameData();

        // Find all objects with the tag "Node"
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject node in nodes)
        {
            if (node.TryGetComponent<INode>(out INode nodeScript))
            {
                string type = nodeScript.GetType().Name;
                rootObject.gameData.clones.Add(CreateCloneData(node.transform.position, type, nodeScript.randomedValue, nodeScript.AlreadyGenerate));
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
                GameObject prefabToUse = GetPrefabByType(cloneData.type);
                if (prefabToUse != null)
                {
                    GameObject newClone = Instantiate(prefabToUse, cloneData.position, Quaternion.Euler(90, 0, 0));
                    newClone.tag = "Node";
                    AssignCloneData(newClone, cloneData);
                }
            }

            Debug.Log("Game Loaded");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
        }
    }


    private CloneData CreateCloneData(Vector3 position, string type, float randomedValue, bool alreadyGenerate)
    {
        return new CloneData
        {
            position = position,
            type = type,
            randomedValue = randomedValue,
            alreadyGenerate = alreadyGenerate
        };
    }

    private GameObject GetPrefabByType(string type)
    {
        if (prefabDictionary.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    private void AssignCloneData(GameObject clone, CloneData cloneData)
    {
        if (clone.TryGetComponent<INode>(out INode nodeScript))
        {
            nodeScript.randomedValue = cloneData.randomedValue;
            nodeScript.AlreadyGenerate = cloneData.alreadyGenerate;
        }
    }
}
