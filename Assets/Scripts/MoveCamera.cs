using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
