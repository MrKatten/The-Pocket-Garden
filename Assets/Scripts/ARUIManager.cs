using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ARUIManager : MonoBehaviour
{
    [SerializeField]
    private PlaceObjectOnPlane placeObjectScript;

    [SerializeField]
    private ARInteractionManager interactionManager;

    [SerializeField]
    private GameObject[] objectPrefabs;

    [SerializeField]
    private Button[] objectButtons;

    [SerializeField]
    private Button deleteButton;

    [SerializeField]
    private Button resetButton;

    void Start()
    {
        // Настройка кнопок выбора объектов
        for (int i = 0; i < objectButtons.Length; i++)
        {
            int index = i; // Локальная копия для замыкания
            objectButtons[i].onClick.AddListener(() => SelectObject(index));
        }

        // Кнопка удаления
        deleteButton.onClick.AddListener(() => interactionManager.DeleteSelectedObject());

        // Кнопка сброса
        resetButton.onClick.AddListener(ResetScene);
    }

    private void SelectObject(int prefabIndex)
    {
        if (prefabIndex < objectPrefabs.Length)
        {
            // Визуальная обратная связь
            HighlightSelectedButton(prefabIndex);
        }
    }

    private void HighlightSelectedButton(int selectedIndex)
    {
        for (int i = 0; i < objectButtons.Length; i++)
        {
            var colors = objectButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.green : Color.white;
            objectButtons[i].colors = colors;
        }
    }

    private void ResetScene()
    {
        // Удаляем все размещенные объекты
        GameObject[] placedObjects = GameObject.FindGameObjectsWithTag("ARObject");
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj);
        }
    }
}