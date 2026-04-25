using UnityEngine;
using UnityEngine.UI;

public class VehicleDelivery : MonoBehaviour
{
    [Header("Ressource transportée")]
    public string currentResource = "water"; // water | health | food
    public float deliveryAmount = 40f;
    public bool isLoaded = false;

    [Header("UI")]
    public Text resourceIndicatorText;
    public GameObject loadedIndicator; // lumière verte sur le véhicule

    private Village        villageInRange = null;
    private ResourceDepot  depotInRange   = null;

    void Update()
    {
        // Changer de ressource avec les touches 1 / 2 / 3
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectResource("water");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectResource("health");
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectResource("food");

        // Charger au dépôt (touche E)
        if (Input.GetKeyDown(KeyCode.E) && depotInRange != null && !isLoaded)
            LoadResource();

        // Livrer au village (touche E)
        if (Input.GetKeyDown(KeyCode.E) && villageInRange != null && isLoaded)
            DeliverToVillage();

        UpdateUI();
    }

    void SelectResource(string type)
    {
        currentResource = type;
        isLoaded = false; // doit recharger après changement
    }

    void LoadResource()
    {
        isLoaded = true;
        Debug.Log("Chargé: " + currentResource);
    }

    void DeliverToVillage()
    {
        villageInRange.DeliverResource(currentResource, deliveryAmount);
        isLoaded = false;
        Debug.Log("Livré à: " + villageInRange.villageName);
    }

    void OnTriggerEnter(Collider other)
    {
        Village v = other.GetComponent<Village>();
        if (v != null) villageInRange = v;

        ResourceDepot d = other.GetComponent<ResourceDepot>();
        if (d != null) depotInRange = d;
    }

    void OnTriggerExit(Collider other)
    {
        Village v = other.GetComponent<Village>();
        if (v != null && villageInRange == v) villageInRange = null;

        ResourceDepot d = other.GetComponent<ResourceDepot>();
        if (d != null && depotInRange == d) depotInRange = null;
    }

    void UpdateUI()
    {
        string label = isLoaded 
            ? $"[{currentResource.ToUpper()}] CHARGÉ ✓" 
            : $"[{currentResource.ToUpper()}] — Appuyez E pour charger";

        if (resourceIndicatorText) resourceIndicatorText.text = label;
        if (loadedIndicator) loadedIndicator.SetActive(isLoaded);
    }
}