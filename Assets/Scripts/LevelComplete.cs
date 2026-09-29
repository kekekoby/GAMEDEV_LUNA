
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public GameObject winCanvas;

    void OnTriggerEnter(Collider other)
    {
        winCanvas.SetActive(true);
    }
}