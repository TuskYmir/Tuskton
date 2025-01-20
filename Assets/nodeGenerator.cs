using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeGenerator : MonoBehaviour
{
    public SaveSystemManager saveSystemManager;
    public PlayerDataManager playerDataManager;

    [Header("Prefabs")]
    public GameObject Prefab; // Single prefab

    [Header("Generation Settings")]
    public int minNodes = 1;
    public int maxNodes = 10;
    public float minRadius = 5f;
    public float maxRadius = 20f;
    public float minDistance = 3f;

    [Header("UI Elements")]
    public Text valueText; // Assign the Text UI element in the Inspector
    public Text HeadingText; // Assign the Text UI element in the Inspector
    public TextMeshProUGUI storyText; // Assign the Text UI element in the Inspector

    public GameObject alert; // Assign the Animation UI element in the Inspector
    public Transform choicesContainer; // Assign the Choices Container in the Inspector
    public Button choiceButtonPrefab; // Assign the Choice Button prefab in the Inspector


    private List<Vector3> generatedPositions = new List<Vector3>();

    private GameObject selectedNode;

    public GameObject menuUI; // Assign a menu UI prefab or GameObject in the Inspector

    public PlayerDataManager moneyManager; // Reference to the MoneyManager

    private Camera mainCamera;

    public TimeManager timeManager;

    public Animator alertAnimator;

    private void Start()
    {
        Debug.Log("NodeGenerator Start");
        // Cache the main camera reference
        mainCamera = Camera.main;
 
    }

    private void Update()
    {
        HandleMouseclick();
       
    }
    
    public void HandleMouseclick()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Node"))
                {
                    selectedNode = hit.collider.gameObject;
                    PREUpdateMunu(selectedNode.GetComponent<NodeInfoManager>().storyID);
                    ShowMenu();

                    // Get the Story component and update the valueText
                    NodeInfoManager valueProvider = selectedNode.GetComponent<NodeInfoManager>();
                    if (valueProvider != null)
                    {
                        valueText.text = $"Value: {valueProvider.randomedValue}";
                    }
                }
                else
                {
                    HideMenu();
                }
            }
        }
    }

    public void GenerateNodes()
    {
        if (selectedNode == null) return;

        if (timeManager.currentActs > 0)
        {
            if (Prefab == null)
            {
                Debug.LogError("No prefab assigned for node generation.");
                return;
            }

            NodeInfoManager valueProvider = selectedNode.GetComponent<NodeInfoManager>();
            if (valueProvider != null)
            {
                if (!valueProvider.alreadyGenerate)
                {
                    Vector3 originPosition = selectedNode.transform.position;
                    int nodeCount = Random.Range(minNodes, maxNodes + 1);

                    // Add the RandomedValue of the selected node to money
                    moneyManager.AddMoney(valueProvider.randomedValue); // Use MoneyManager to add money

                    for (int i = 0; i < nodeCount; i++)
                    {
                        GenerateNode(originPosition);
                    }

                    // Set AlreadyGenerated to true
                    valueProvider.alreadyGenerate = true;
                    timeManager.currentActs -= 1;
                }
                else
                {
                    Debug.Log("This node has already been generated.");
                }
            }
        }
        else
        {
            Debug.Log("No acts left.");
            alertAnimator.SetTrigger("Noacts");

        }

        HideMenu();
    }

    private void GenerateNode(Vector3 originPosition)
    {
        Vector3 newPosition = GenerateRandomPosition(originPosition);

        // Check if the new position is valid
        if (IsPositionValid(newPosition))
        {
            // Instantiate the node
            GameObject newNode = Instantiate(Prefab, newPosition, Quaternion.Euler(90, 0, 0));

            // Tag the new node for detection
            newNode.tag = "Node";

            // Add the position to the list
            generatedPositions.Add(newPosition);
        }
    }

    private Vector3 GenerateRandomPosition(Vector3 origin)
    {
        // Generate a random point within the radius
        float radius = Random.Range(minRadius, maxRadius);
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(origin.x + x, 10, origin.z + z);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in generatedPositions)
        {
            if (Vector3.Distance(existingPosition, position) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void PREUpdateMunu(int storyID)
    {
        // Try to get the story from the dictionary
        if (saveSystemManager.storyDictionary.TryGetValue(storyID, out Story story))
        {
            // Pass the story to the DisplayStory method
            UpdateMenu(story);
        }
        else
        {
            Debug.Log($"Story with ID {storyID} not found!");
        }
    }

    private void UpdateMenu(Story story)
    {
        // Example UI setup for displaying the story
        HeadingText.text = story.heading;
        storyText.text = story.story;

        // Clear previous choices by deactivating them
        foreach (Transform child in choicesContainer)
        {
            child.gameObject.SetActive(false);
        }

        // Display choices as buttons
        int index = 0;
        foreach (var choice in story.choices)
        {
            Button choiceButton;
            if (index < choicesContainer.childCount)
            {
                // Reuse existing button
                choiceButton = choicesContainer.GetChild(index).GetComponent<Button>();
                choiceButton.gameObject.SetActive(true);
            }
            else
            {
                // Instantiate new button if needed
                choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
            }

            choiceButton.GetComponentInChildren<Text>().text = choice.choiceText;

            // Remove previous listeners to avoid multiple calls
            choiceButton.onClick.RemoveAllListeners();

            // Add a listener to handle button click
            choiceButton.onClick.AddListener(() => OnChoiceSelected(story,choice));

            index++;
        }
    }

    private Choice pendingChoice;

    private void OnChoiceSelected(Story story,Choice choice)
    {
        if (pendingChoice == choice)
        {
            // Confirm the choice
            Debug.Log($"Confirmed choice: {choice.choiceText}");
            playerDataManager.AddMoney(choice.outcome.Money);
            playerDataManager.AddFood(choice.outcome.food);
            playerDataManager.AddResource(choice.outcome.resource);
            pendingChoice = null;
            GenerateNodes();
            HideMenu();
        }
        else
        {
            
            storyText.text = $"{story.story}" + "<color=grey>\n__________________________________________________________________________________________" + $"\n<i>({choice.choiceText})</i></color>" + $"<color=yellow>\n{choice.description}</color>";
            // Set the pending choice and trigger the WaitingForConfirm animation
            pendingChoice = choice;
            Debug.Log($"Selected choice: {choice.choiceText}. Click again to confirm.");
            //alertAnimator.SetTrigger("WaitingForConfirm");


            // Trigger UnSelected animation for other choices
            foreach (Transform child in choicesContainer)
            {
                Button button = child.GetComponent<Button>();
                if (button != null && button.GetComponentInChildren<Text>().text != choice.choiceText)
                {
                    Debug.Log("UnSelected");
                    //alertAnimator.SetTrigger("UnSelected");
                }
            }
        }
    }

    private void ShowMenu()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }
    }

    private void HideMenu()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }
    }
}
