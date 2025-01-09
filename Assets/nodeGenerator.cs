using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> Prefabs = new List<GameObject>();

    [Header("Generation Settings")]
    public int minNodes = 1;
    public int maxNodes = 10;
    public float minRadius = 5f;
    public float maxRadius = 20f;
    public float minDistance = 3f;

    [Header("UI Elements")]
    public Text valueText; // Assign the Text UI element in the Inspector
    public Text HeadingText; // Assign the Text UI element in the Inspector
    public Text StoryText; // Assign the Text UI element in the Inspector
    public Text OutcomeText; // Assign the Text UI element in the Inspector
    public GameObject alert; // Assign the Animation UI element in the Inspector

    private List<Vector3> generatedPositions = new List<Vector3>();

    private GameObject selectedNode;

    public GameObject menuUI; // Assign a menu UI prefab or GameObject in the Inspector

    public MoneyManager moneyManager; // Reference to the MoneyManager

    private Camera mainCamera;

    public TimeManager timeManager;

    private Animator alertAnimator;

    private void Start()
    {
        // Cache the main camera reference
        mainCamera = Camera.main;
        alertAnimator = alert.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Node"))
                {
                    selectedNode = hit.collider.gameObject;
                    ShowMenu(hit.point);

                    // Get the Node component and update the valueText
                    INode valueProvider = selectedNode.GetComponent<INode>();
                    if (valueProvider != null)
                    {
                        valueText.text = $"Value: {valueProvider.randomedValue}";
                        HeadingText.text = $"Heading: {valueProvider.heading}";
                        StoryText.text = $"Story: {valueProvider.storyText}";
                        OutcomeText.text = $"Outcome: {valueProvider.outcome}";
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
            if (Prefabs.Count == 0)
            {
                Debug.LogError("No prefabs assigned for node generation.");
                return;
            }

            INode valueProvider = selectedNode.GetComponent<INode>();
            if (valueProvider != null)
            {
                if (!valueProvider.AlreadyGenerate)
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
                    valueProvider.AlreadyGenerate = true;
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
            // Randomly select prefab
            int prefabIndex = Random.Range(0, Prefabs.Count);
            GameObject prefabToUse = Prefabs[prefabIndex];

            // Instantiate the node
            GameObject newNode = Instantiate(prefabToUse, newPosition, Quaternion.Euler(90, 0, 0));

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

    private void ShowMenu(Vector3 position)
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
