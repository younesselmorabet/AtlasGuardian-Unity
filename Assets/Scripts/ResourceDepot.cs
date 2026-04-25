using UnityEngine;
using UnityEngine.UI;

public class ResourceDepot : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt; // "Appuyez E pour charger"

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VehicleDelivery>() != null)
            if (interactPrompt) interactPrompt.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<VehicleDelivery>() != null)
            if (interactPrompt) interactPrompt.SetActive(false);
    }
}