using System.IO;
using JetBrains.Annotations;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;
using System.Collections;
using TMPro; // Add this for List<T>

public class NodeInfoManager : MonoBehaviour
{
    [Header("Story text")]
    public GameObject textContainer;
    public GameObject headingText;
    public GameObject storyText;

    public int randomedValue;
    public bool alreadyGenerate;
    public int storyID;

    [Header("Story Type")]
    public int storyType;
    public int StoryTypeMultiplier = 100;
    public int StoryTypeOffset = 15;
    public int RandomValueMax = 5000;

    Animator animator;

    [SerializeField]
    
    private SaveSystemManager saveSystemManager;

    public GameObject mainCamera;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("NodeInfoManager Awake");

        // Assign saveSystemManager if not already assigned
        if (saveSystemManager == null)
        {
            saveSystemManager = FindFirstObjectByType<SaveSystemManager>();
            if (saveSystemManager == null)
            {
                Debug.LogError("SaveSystemManager not found in the scene.");
            }
        }

        // Assign mainCamera if not already assigned
        if (mainCamera == null)
        {
            Debug.Log("Finding MainCamera");
            mainCamera = GameObject.FindWithTag("MainCamera");
            if (mainCamera == null)
            {
                Debug.LogError("MainCamera not found in the scene.");
            }
        }

        StartCoroutine(InitializeAfterDelay());
        animator.SetTrigger("Generate");
    }

    private void Update()
    {
        if(mainCamera.transform.position.y > 40 )
        {   
            textContainer.SetActive(false);
        }
        else
        {
            textContainer.SetActive(true);
        }
    }

    private IEnumerator InitializeAfterDelay()
    {
        // Wait until the end of the frame to ensure all Awake methods are called
        yield return new WaitForEndOfFrame();

        if (saveSystemManager == null)
        {
            Debug.LogError("SaveSystemManager is not assigned in the inspector.");
            yield break;
        }

        randomValue();
        PRESetNodeText(storyID);
    }

    public void randomValue()
    {
        storyType = Random.Range(0, 3);
        storyID = Random.Range((storyType * StoryTypeMultiplier) + 1, (storyType * StoryTypeMultiplier) + StoryTypeOffset);
        randomedValue = Random.Range(0, RandomValueMax);
        alreadyGenerate = false;
        Debug.Log("Story Type: " + storyType + " Story ID: " + storyID + " Random Value: " + randomedValue);
    }

    public void PRESetNodeText(int storyID)
    {
        if (saveSystemManager == null)
        {
            Debug.LogError("SaveSystemManager is not assigned.");
            return;
        }

        // Try to get the story from the dictionary
        if (saveSystemManager.storyDictionary.TryGetValue(storyID, out Story story))
        {
            // Pass the story to the DisplayStory method
            SetNodeText(story);
        }
        else
        {
            Debug.Log($"Story with ID {storyID} not found!");
        }
    }

    public void SetNodeText(Story story)
    {
        // Example UI setup for displaying the story
        headingText.GetComponent<TextMesh>().text =$"StoryID_{story.storyID}";
        storyText.GetComponent<TextMesh>().text = story.heading;
    }
}
