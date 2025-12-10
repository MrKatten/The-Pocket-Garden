using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] public GameObject objectToPlace;
    [SerializeField] private GameObject[] objectsList;
    [SerializeField] private InputActionAsset ARInputActions;
    private InputAction placeObjectAction;

    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        var actionMap = ARInputActions.FindActionMap("ARControls");
        placeObjectAction = actionMap.FindAction("PlaceObject");
        placeObjectAction.Enable();
        Debug.Log(actionMap);
        Debug.Log(placeObjectAction);
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        placeObjectAction.performed += ctx =>
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Клик был на UI элементе - игнорируем");
                return; // Не обрабатываем клик дальше
            }
            else if (Touchscreen.current != null) 
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Debug.Log("Тап был на UI элементе - игнорируем");
                    return;
                }
            }
            else
            {
                Debug.Log("place");
                PlaceObject();
            }
        };
    }

    private void UpdatePlacementPose()
    {
        // Получаем центр экрана
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        // Raycast для поиска плоскостей
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            // Корректируем поворот объекта
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            if (placementPoseIsValid)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(
                    placementPose.position,
                    placementPose.rotation
                );
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    // Метод для смены размещаемого объекта
    public void ChangeObjectToPlace(int id)
    {
        objectToPlace = objectsList[id];
    }
}