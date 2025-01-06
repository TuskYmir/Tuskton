using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CloneData
{
    public Vector3 position; // Position of the clone
    public string type;      // Type of the clone ("Positive" or "Negative")
    public float randomedValue; // Unique random value
    public bool alreadyGenerate; // Whether it has generated something
}

[System.Serializable]
public class GameData
{
    public List<CloneData> clones = new List<CloneData>(); // List of all clone data
}
public class saveGame : MonoBehaviour
{
    public GameObject positivePrefab; // Assign Positive prefab in Unity
    public GameObject negativePrefab; // Assign Negative prefab in Unity

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/savegame.json";
        Debug.Log("Save Path: " + savePath);
    }

    public void SaveGame()
    {
        GameData gameData = new GameData();

        // Find all objects with the tag "Node"
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject node in nodes)
        {
            // Check for Positive or Negative scripts
            if (node.TryGetComponent<Positive>(out Positive positiveScript))
            {
                gameData.clones.Add(new CloneData
                {
                    position = node.transform.position,
                    type = "Positive",
                    randomedValue = positiveScript.randomedValue,
                    alreadyGenerate = positiveScript.AlreadyGenerate
                });
            }
            else if (node.TryGetComponent<Negative>(out Negative negativeScript))
            {
                gameData.clones.Add(new CloneData
                {
                    position = node.transform.position,
                    type = "Negative",
                    randomedValue = negativeScript.randomedValue,
                    alreadyGenerate = negativeScript.AlreadyGenerate
                });
            }
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + json);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);

        // Destroy all existing nodes
        GameObject[] existingNodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject node in existingNodes)
        {
            Destroy(node);
        }

        // Recreate clones from saved data
        foreach (CloneData cloneData in gameData.clones)
        {
            GameObject prefabToUse = null;

            // Determine which prefab to use based on the type
            if (cloneData.type == "Positive")
                prefabToUse = positivePrefab;
            else if (cloneData.type == "Negative")
                prefabToUse = negativePrefab;

            if (prefabToUse != null)
            {
                GameObject newClone = Instantiate(prefabToUse, cloneData.position, Quaternion.Euler(90, 0, 0));
                newClone.tag = "Node";

                // Assign saved values to the script
                if (newClone.TryGetComponent<Positive>(out Positive positiveScript))
                {
                    positiveScript.randomedValue = cloneData.randomedValue;
                    positiveScript.AlreadyGenerate = cloneData.alreadyGenerate;
                }
                else if (newClone.TryGetComponent<Negative>(out Negative negativeScript))
                {
                    negativeScript.randomedValue = cloneData.randomedValue;
                    negativeScript.AlreadyGenerate = cloneData.alreadyGenerate;
                }
            }
        }

        Debug.Log("Game Loaded");
    }
}
