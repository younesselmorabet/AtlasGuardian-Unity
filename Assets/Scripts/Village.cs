using UnityEngine;
using UnityEngine.UI;

public class Village : MonoBehaviour
{
    [Header("Nom du village")]
    public string villageName = "Village Atlas";

    [Header("Ressources (0 à 100)")]
    public float water = 80f;
    public float health = 90f;
    public float food = 70f;

    [Header("Vitesse de perte (par seconde)")]
    public float waterDrain = 3f;
    public float healthDrain = 1.5f;
    public float foodDrain = 2f;

    [Header("Seuil critique")]
    public float criticalLevel = 30f;

    [Header("UI du village (world space)")]
    public Slider waterSlider;
    public Slider healthSlider;
    public Slider foodSlider;
    public GameObject alertIcon;
    public Text villageNameText;

    [Header("Visuel")]
    public Renderer buildingRenderer;

    private bool isInCrisis = false;

    void Start()
    {
        if (villageNameText) villageNameText.text = villageName;

        // S'enregistrer dans le GameManager automatiquement
        if (GameManager.instance != null)
            GameManager.instance.allVillages.Add(this);

        UpdateUI();
    }

    void Update()
    {
        // Perte de ressources avec le temps
        water  = Mathf.Max(0, water  - waterDrain  * Time.deltaTime);
        health = Mathf.Max(0, health - healthDrain * Time.deltaTime);
        food   = Mathf.Max(0, food   - foodDrain   * Time.deltaTime);

        UpdateUI();
        CheckCrisis();

        // Condition Game Over
        if (water <= 0 || health <= 0 || food <= 0)
            GameManager.instance.GameOver(villageName);
    }

    void CheckCrisis()
    {
        bool critical = water < criticalLevel || 
                        health < criticalLevel || 
                        food < criticalLevel;

        if (critical == isInCrisis) return;

        isInCrisis = critical;
        if (alertIcon) alertIcon.SetActive(isInCrisis);
        if (buildingRenderer)
            buildingRenderer.material.color = isInCrisis ? Color.red : Color.white;
    }

    public void DeliverResource(string resourceType, float amount)
    {
        switch (resourceType)
        {
            case "water":  water  = Mathf.Min(100f, water  + amount); break;
            case "health": health = Mathf.Min(100f, health + amount); break;
            case "food":   food   = Mathf.Min(100f, food   + amount); break;
        }

        GameManager.instance.AddScore(CalculateScore(resourceType));
        UpdateUI();
    }

    int CalculateScore(string resourceType)
    {
        float stat = resourceType == "water"  ? water  :
                     resourceType == "health" ? health : food;
        return stat < criticalLevel ? 200 : 100; // bonus si village était critique
    }

    void UpdateUI()
    {
        if (waterSlider)  waterSlider.value  = water  / 100f;
        if (healthSlider) healthSlider.value = health / 100f;
        if (foodSlider)   foodSlider.value   = food   / 100f;
    }

    public bool IsCritical()
    {
        return water < criticalLevel || health < criticalLevel || food < criticalLevel;
    }

    public string GetUrgentResource()
    {
        if (water <= health && water <= food)   return "water";
        if (health <= water && health <= food)  return "health";
        return "food";
    }
}