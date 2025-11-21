using UnityEngine;
using System.Collections;
using TMPro;

public class PlantGrowth : MonoBehaviour
{
    [Header("Настройки роста")]
    public float growthTime = 60f; // Время роста в секундах
    public GameObject[] plantStages; // Массив моделей стадий роста
    public ParticleSystem waterParticles; // Эффект полива (опционально)

    [Header("UI элементы")]
    public TMP_Text growthText; // Текст для отображения прогресса
    public UnityEngine.UI.Button waterButton; // Кнопка полива

    private float currentGrowthTime = 0f;
    private int currentStage = 0;
    private bool isWatered = false;
    private bool isGrowing = false;
    private string plantKey = "PlantGrowthData";

    [System.Serializable]
    public class PlantData
    {
        public float savedGrowthTime;
        public int savedStage;
        public bool savedIsWatered;
    }

    void Start()
    {
        ResetPlant();
        LoadPlantData();
        UpdatePlantModel();

        if (waterButton != null)
        {
            waterButton.onClick.AddListener(WaterPlant);
        }

        if (!isGrowing && isWatered)
        {
            StartGrowth();
        }
    }

    void Update()
    {
        if (isGrowing)
        {
            currentGrowthTime += Time.deltaTime;
            UpdateGrowthUI();

            // Проверяем переход на следующую стадию
            CheckStageTransition();

            // Сохраняем прогресс каждые 5 секунд
            if (Time.frameCount % 300 == 0) // Примерно каждые 5 секунд при 60 FPS
            {
                SavePlantData();
            }
        }
    }

    public void WaterPlant()
    {
        if (!isWatered && !isGrowing)
        {
            isWatered = true;
            waterButton.gameObject.SetActive(false);
            StartGrowth();

            // Воспроизводим эффект полива
            if (waterParticles != null)
            {
                waterParticles.Play();
            }

            SavePlantData();
            Debug.Log("Растение полито! Начинается рост.");
        }
    }

    private void StartGrowth()
    {
        isGrowing = true;

        if (waterButton != null)
        {
            waterButton.interactable = false;
        }
    }

    private void CheckStageTransition()
    {
        int targetStage = Mathf.FloorToInt((currentGrowthTime / growthTime) * (plantStages.Length - 1));
        targetStage = Mathf.Clamp(targetStage, 0, plantStages.Length - 1);

        if (targetStage != currentStage)
        {
            currentStage = targetStage;
            UpdatePlantModel();
            SavePlantData();
        }

        // Проверяем завершение роста
        if (currentGrowthTime >= growthTime && currentStage == plantStages.Length - 1)
        {
            CompleteGrowth();
        }
    }

    private void UpdatePlantModel()
    {
        // Скрываем все модели
        foreach (GameObject stage in plantStages)
        {
            if (stage != null)
                stage.SetActive(false);
        }

        // Показываем текущую стадию
        if (plantStages.Length > 0 && currentStage < plantStages.Length && plantStages[currentStage] != null)
        {
            plantStages[currentStage].SetActive(true);
        }
    }

    private void UpdateGrowthUI()
    {
        if (growthText != null)
        {
            float progress = Mathf.Clamp01(currentGrowthTime / growthTime);
            growthText.text = $"Прогресс роста: {progress * 100:F1}%\n" +
                            $"Стадия: {currentStage + 1}/{plantStages.Length}\n" +
                            $"Время: {Mathf.FloorToInt(currentGrowthTime)}/{growthTime}сек";
        }
    }

    private void CompleteGrowth()
    {
        isGrowing = false;
        Debug.Log("Рост растения завершен!");

        if (growthText != null)
        {
            growthText.text = "Рост завершен!";
            growthText.gameObject.SetActive(false);
        }
    }

    private void SavePlantData()
    {
        PlantData data = new PlantData
        {
            savedGrowthTime = currentGrowthTime,
            savedStage = currentStage,
            savedIsWatered = isWatered
        };

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(plantKey, jsonData);
        PlayerPrefs.Save();
    }

    private void LoadPlantData()
    {
        if (PlayerPrefs.HasKey(plantKey))
        {
            string jsonData = PlayerPrefs.GetString(plantKey);
            PlantData data = JsonUtility.FromJson<PlantData>(jsonData);

            currentGrowthTime = data.savedGrowthTime;
            currentStage = data.savedStage;
            isWatered = data.savedIsWatered;

            Debug.Log("Данные растения загружены");
        }
        else
        {
            ResetPlant();
        }
    }

    public void ResetPlant()
    {
        currentGrowthTime = 0f;
        currentStage = 0;
        isWatered = false;
        isGrowing = false;

        if (waterButton != null)
        {
            waterButton.interactable = true;
        }

        SavePlantData();
        UpdatePlantModel();
    }

    // Метод для отладки (можно удалить в финальной версии)
    [ContextMenu("Быстрый рост")]
    public void FastGrowth()
    {
        currentGrowthTime = growthTime - 1f;
    }

    [ContextMenu("Удалить сохранения")]
    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(plantKey);
        PlayerPrefs.Save();
        Debug.Log("Сохранения растения удалены");
    }
}
