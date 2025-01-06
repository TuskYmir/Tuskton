using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Positive : MonoBehaviour, IRandomedValueProvider
{
    public float randomedValue;
    public float RandomedValue => randomedValue;
    public Text text;
    public void randomValue()
    {
        randomedValue = Random.Range(100, 500);
        Debug.Log("randomed");


    }
    void Awake()
    {
        randomValue();

    }
}