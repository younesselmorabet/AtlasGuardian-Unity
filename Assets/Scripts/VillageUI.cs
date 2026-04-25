using UnityEngine;
using UnityEngine.UI;

public class VillageUI : MonoBehaviour
{
    [Header("Panneau village sélectionné")]
    public GameObject villageInfoPanel;
    public Text selectedVillageName;
    public Slider selectedWater;
    public Slider selectedHealth;
    public Slider selectedFood;

    [Header("Suggestion IA")]
    public Text aiSuggestionText;

    [Header("Indicateur véhicule")]
    public Text vehicleStatusText;

    private Village selectedVillage;
    private VehicleDelivery vehicle;

    void Start()
    {
        vehicle = FindFirstObjectByType<VehicleDelivery>();
        if (villageInfoPanel) villageInfoPanel.SetActive(false);
    }

    void Update()
    {
        // Cliquer sur un village pour voir ses stats
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Village v = hit.collider.GetComponent<Village>();
                if (v != null) SelectVillage(v);
            }
        }

        if (selectedVillage != null) RefreshSelectedVillage();

        UpdateAISuggestion();
        UpdateVehicleStatus();
    }

    void SelectVillage(Village v)
    {
        selectedVillage = v;
        if (villageInfoPanel) villageInfoPanel.SetActive(true);
        if (selectedVillageName) selectedVillageName.text = v.villageName;
    }

    void RefreshSelectedVillage()
    {
        if (selectedWater)  selectedWater.value  = selectedVillage.water  / 100f;
        if (selectedHealth) selectedHealth.value = selectedVillage.health / 100f;
        if (selectedFood)   selectedFood.value   = selectedVillage.food   / 100f;
    }

    void UpdateAISuggestion()
    {
        if (GameManager.instance == null || aiSuggestionText == null) return;

        Village critical = GameManager.instance.GetMostCriticalVillage();
        if (critical == null) return;

        string resource = critical.GetUrgentResource();
        string icon = resource == "water" ? "💧" : resource == "health" ? "🏥" : "🍞";
        aiSuggestionText.text = $"⚠ IA → {critical.villageName} a besoin de {icon}";
    }

    void UpdateVehicleStatus()
    {
        if (vehicle == null || vehicleStatusText == null) return;
        vehicleStatusText.text = vehicle.isLoaded
            ? $"Véhicule: {vehicle.currentResource.ToUpper()} chargé"
            : "Véhicule: vide — allez au dépôt";
    }
}