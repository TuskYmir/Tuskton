using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class story : MonoBehaviour, IRandomedValueProvider
{
    public float randomedValue;
    public bool AlreadyGenerate = false;

    public float RandomedValue => randomedValue;
    public Text text;
    public void randomValue()
    {
        randomedValue = Random.Range(500, 1000);
        Debug.Log("randomed");


    }
    void Awake()
    {
        randomValue();
    }
}