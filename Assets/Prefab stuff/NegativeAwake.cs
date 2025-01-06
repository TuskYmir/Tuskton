using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Negative : MonoBehaviour, IRandomedValueProvider
{
    public float randomedValue;
    public bool AlreadyGenerate = false;
    public int prefapIndexNumber = 0;
    public float RandomedValue => randomedValue;
    public Text text;
    public void randomValue()
    {
        randomedValue = Random.Range(-500,0);
        Debug.Log("randomed");

    }
    void Awake()
    {
        randomValue();
    }
}