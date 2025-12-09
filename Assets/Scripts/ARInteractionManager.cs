using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float scaleSpeed = 0.01f;
    [SerializeField] private InputActionAsset ARInputActions;

    private GameObject selectedObject;
    private bool isObjectSelected = false;
    private Vector2 lastTouchPosition;
    private Vector2 touchZeroStartPos;
    private Vector2 touchOneStartPos;

    // Input Actions
    private InputAction touchStartAction;
    private InputAction touchPositionAction;
    private InputAction touchDeltaAction;

    void Awake()
    {
        SetupInputActions();
    }

    void OnEnable()
    {
        EnableInputActions();
    }

    void OnDisable()
    {
        DisableInputActions();
    }

    void Update()
    {
        // Обработка масштабирования двумя пальцами
        HandleTwoFingerScaling();
    }

    private void SetupInputActions()
    {
        // Получаем Action Map "ARControls"
        var arControlsMap = ARInputActions.FindActionMap("ARControls");
        // Получаем действия
        touchStartAction = arControlsMap.FindAction("TouchStart");
        touchPositionAction = arControlsMap.FindAction("TouchPosition");
        touchDeltaAction = arControlsMap.FindAction("TouchDelta");
    }

    private void EnableInputActions()
    {
        touchStartAction?.Enable();
        touchPositionAction?.Enable();
        touchDeltaAction?.Enable();

        // Подписываемся на события
        if (touchStartAction != null)
        {
            touchStartAction.started += OnTouchStarted;
            touchStartAction.canceled += OnTouchCanceled;
        }

        if (touchDeltaAction != null)
        {
            touchDeltaAction.performed += OnTouchMoved;
        }
    }

    private void DisableInputActions()
    {
        // Отписываемся от событий
        if (touchStartAction != null)
        {
            touchStartAction.started -= OnTouchStarted;
            touchStartAction.canceled -= OnTouchCanceled;
        }

        if (touchDeltaAction != null)
        {
            touchDeltaAction.performed -= OnTouchMoved;
        }

        touchStartAction?.Disable();
        touchPositionAction?.Disable();
        touchDeltaAction?.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (touchPositionAction != null)
        {
            Vector2 touchPosition = touchPositionAction.ReadValue<Vector2>();
            HandleSelection(touchPosition);
            lastTouchPosition = touchPosition;
        }
    }

    private void OnTouchMoved(InputAction.CallbackContext context)
    {
        if (isObjectSelected && selectedObject != null)
        {
            Vector2 deltaPosition = context.ReadValue<Vector2>();
            HandleRotation(deltaPosition);
        }
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        // Снимаем выделение при отмене касания
        if (selectedObject != null)
        {
            HighlightObject(selectedObject, false);
        }
        isObjectSelected = false;
        selectedObject = null;
    }

    private void HandleTwoFingerScaling()
    {
        // Используем Touchscreen для получения данных о нескольких касаниях
        var touchscreen = Touchscreen.current;
        if (touchscreen == null || selectedObject == null) return;

        // Получаем все активные касания
        var touches = touchscreen.touches;
        int touchCount = 0;
        Vector2 touchZeroPos = Vector2.zero;
        Vector2 touchOnePos = Vector2.zero;

        foreach (var touch in touches)
        {
            if (touch.isInProgress)
            {
                if (touchCount == 0)
                {
                    touchZeroPos = touch.position.ReadValue();
                    touchCount++;
                }
                else if (touchCount == 1)
                {
                    touchOnePos = touch.position.ReadValue();
                    touchCount++;
                    break;
                }
            }
        }

        if (touchCount == 2)
        {
            float currentDistance = Vector2.Distance(touchZeroPos, touchOnePos);

            // Если это начало масштабирования, сохраняем начальные позиции
            if (touchZeroStartPos == Vector2.zero && touchOneStartPos == Vector2.zero)
            {
                touchZeroStartPos = touchZeroPos;
                touchOneStartPos = touchOnePos;
            }
            else
            {
                float startDistance = Vector2.Distance(touchZeroStartPos, touchOneStartPos);
                float difference = currentDistance - startDistance;

                Vector3 newScale = selectedObject.transform.localScale +
                                  Vector3.one * difference * scaleSpeed;
                newScale = Vector3.Max(newScale, Vector3.one * 0.1f);
                newScale = Vector3.Min(newScale, Vector3.one * 3f);

                selectedObject.transform.localScale = newScale;

                // Обновляем стартовые позиции
                touchZeroStartPos = touchZeroPos;
                touchOneStartPos = touchOnePos;
            }
        }
        else
        {
            // Сбрасываем стартовые позиции
            touchZeroStartPos = Vector2.zero;
            touchOneStartPos = Vector2.zero;
        }
    }

    private void HandleSelection(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Снимаем выделение с предыдущего объекта
            if (selectedObject != null && selectedObject != hit.collider.gameObject)
            {
                HighlightObject(selectedObject, false);
            }

            selectedObject = hit.collider.gameObject;
            isObjectSelected = true;

            // Визуальная обратная связь
            HighlightObject(selectedObject, true);
        }
        else
        {
            if (selectedObject != null)
            {
                HighlightObject(selectedObject, false);
            }
            isObjectSelected = false;
            selectedObject = null;
        }
    }

    private void HandleRotation(Vector2 deltaPosition)
    {
        if (selectedObject == null) return;

        float rotationX = deltaPosition.x * rotationSpeed * Time.deltaTime;
        float rotationY = deltaPosition.y * rotationSpeed * Time.deltaTime;

        selectedObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
        selectedObject.transform.Rotate(Vector3.right, rotationY, Space.World);
    }

    private void HighlightObject(GameObject obj, bool highlight)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Сохраняем оригинальный материал
            if (highlight && !obj.GetComponent<ObjectHighlight>())
            {
                var highlightComponent = obj.AddComponent<ObjectHighlight>();
                highlightComponent.originalMaterial = renderer.material;
                renderer.material.color = Color.yellow;
            }
            else if (!highlight)
            {
                var highlightComponent = obj.GetComponent<ObjectHighlight>();
                if (highlightComponent != null && highlightComponent.originalMaterial != null)
                {
                    renderer.material = highlightComponent.originalMaterial;
                    Destroy(highlightComponent);
                }
                else
                {
                    renderer.material.color = Color.white;
                }
            }
        }
    }

    public void DeleteSelectedObject()
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
            selectedObject = null;
            isObjectSelected = false;
        }
    }

    // Вспомогательный класс для хранения оригинального материала
    private class ObjectHighlight : MonoBehaviour
    {
        public Material originalMaterial;
    }
}