using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class story : MonoBehaviour, INode
{
    [SerializeField]
    private float _randomedValue;
    [SerializeField]
    private bool _alreadyGenerate;
    [SerializeField]
    private int _storyID;

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

    public int prefapIndexNumber = 0;
    public float RandomedValue => randomedValue;

    public void randomValue()
    {
        _storyID = Random.Range(0, 10);
        randomedValue = Random.Range(-500, 0);
        Debug.Log("randomed");
        AlreadyGenerate = false;
    }

    void Awake()
    {
        randomValue();
    }
}
