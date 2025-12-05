using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float scaleSpeed = 0.01f;

    private GameObject selectedObject;
    private bool isObjectSelected = false;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleSelection(touch.position);
                    break;

                case TouchPhase.Moved:
                    if (isObjectSelected)
                    {
                        HandleRotation(touch);
                    }
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            HandleScale();
        }
    }

    private void HandleSelection(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
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

    private void HandleRotation(Touch touch)
    {
        if (selectedObject == null) return;

        float rotationX = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
        float rotationY = touch.deltaPosition.y * rotationSpeed * Time.deltaTime;

        selectedObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
        selectedObject.transform.Rotate(Vector3.right, rotationY, Space.World);
    }

    private void HandleScale()
    {
        if (selectedObject == null) return;

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        Vector3 newScale = selectedObject.transform.localScale +
                          Vector3.one * difference * scaleSpeed;
        newScale = Vector3.Max(newScale, Vector3.one * 0.1f); // Минимальный размер
        newScale = Vector3.Min(newScale, Vector3.one * 3f);   // Максимальный размер

        selectedObject.transform.localScale = newScale;
    }

    private void HighlightObject(GameObject obj, bool highlight)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (highlight)
            {
                renderer.material.color = Color.yellow;
            }
            else
            {
                renderer.material.color = Color.white;
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
}