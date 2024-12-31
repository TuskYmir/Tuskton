using UnityEngine;

public class Quadprefab : MonoBehaviour
{
    public string baseTag = "serial";
    public Vector3 positionOfCreation = Vector3.zero;
    private int cloneIndex = 2;
    public float XpositionOfCreattion;
    public float ZpositionOfCreattion;
    public float RotationOfCreattion = 0;
    public float NewRotationOfCreattion;
    public float ForCodeRotationOfCreattion;
    public float RadiusOfCreattion;
    public int NumberToCreattion;
    public int SerialOfCreattion = 0;
    public int NumberOfCreattion = 1;
    public int PastNumberOfCreattion = 0;
    public float firstRotation;


    public bool start = true;

    public sup quadprefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ForCodeRotationOfCreattion = RotationOfCreattion;
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void CloneEvent()
    {
        quadprefab = GameObject.FindGameObjectWithTag("serialmanager").GetComponent<sup>();
        if (start == true)
        {
            NumberToCreattion = Random.Range(3, 5);
        }
        else
        {
            NumberToCreattion = Random.Range(1, 4);
        }

        firstRotation = 360 / NumberToCreattion;

        for (int i = 0; i < NumberToCreattion; i++)
        {

            SerialOfCreattion = quadprefab.SerialNow;
            if (start == true)
            {
                NumberOfCreattion = 0;
            }
            else
            {
                NumberOfCreattion = i;
            }

            Debug.Log("serialNumber" + (quadprefab.SerialNow+1) + " , " + "create number = " + i + " , form serial" + SerialOfCreattion + " number " + NumberOfCreattion);
            RadiusOfCreattion = Random.Range(10, 20);

            if (start == true)
            {
                NewRotationOfCreattion = i* firstRotation;
                RotationOfCreattion = NewRotationOfCreattion;
            }
            else 
            {
                NewRotationOfCreattion = Random.Range(ForCodeRotationOfCreattion - 60, ForCodeRotationOfCreattion + 60);
                RotationOfCreattion = NewRotationOfCreattion;
            }

            float RotationOfCreattionInRadians = NewRotationOfCreattion * Mathf.Deg2Rad;
            XpositionOfCreattion = RadiusOfCreattion * Mathf.Cos(RotationOfCreattionInRadians);
            ZpositionOfCreattion = RadiusOfCreattion * Mathf.Sin(RotationOfCreattionInRadians);

            GameObject Event = Instantiate(gameObject, new Vector3(transform.position.x + XpositionOfCreattion, 1, transform.position.z + ZpositionOfCreattion), Quaternion.identity);

            Event.name = $"{baseTag}{cloneIndex} form serial {cloneIndex-1} number {PastNumberOfCreattion}";
            Event.tag = $"{baseTag}{cloneIndex}";
            Event.transform.eulerAngles = new Vector3(90, 0, 0);

            Quadprefab cloneScript = Event.GetComponent<Quadprefab>();
            cloneScript.cloneIndex = cloneIndex + 1;
            cloneScript.RotationOfCreattion = RotationOfCreattion;
            cloneScript.start = false;
            if (start == true)
            {
                cloneScript.PastNumberOfCreattion = i;
            }
        }
        start = false;
    }
}