using UnityEngine;

public class sup : MonoBehaviour
{
    public int SerialNow = 1;
    public string ChosebaseTag;
    public Quadprefab quadprefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        quadprefab = GameObject.FindGameObjectWithTag("serialmanager").GetComponent<Quadprefab>();
        ChosebaseTag = "serial" + SerialNow;
    }
    public void CloneAllLastSerial()
    {
        ChosebaseTag = "serial" + SerialNow;
        // Find all GameObjects with the tag "serial2"
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(ChosebaseTag);

        foreach (GameObject obj in objectsWithTag)
        {
            // Use SendMessage to call the EventClone() function on the object
            obj.SendMessage("CloneEvent", SendMessageOptions.DontRequireReceiver);
        }

        SerialNow++;
    }
}
