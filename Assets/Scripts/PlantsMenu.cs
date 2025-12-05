using UnityEngine;

public class PlantsMenu : MonoBehaviour
{
    [SerializeField] public PlaceObjectOnPlane placeObjectOnPlane;
    public void CactusButton()
    {
        placeObjectOnPlane.ChangeObjectToPlace(0);
    }
    public void Coleus1Button()
    {
        placeObjectOnPlane.ChangeObjectToPlace(1);
    }
    public void Coleus2Button()
    {
        placeObjectOnPlane.ChangeObjectToPlace(2);
    }
    public void Coleus3Button()
    {
        placeObjectOnPlane.ChangeObjectToPlace(3);
    }
    public void CucumberButton()
    {
        placeObjectOnPlane.ChangeObjectToPlace(4);
    }
    public void MonsteraButton()
    {
        placeObjectOnPlane.ChangeObjectToPlace(5);
    }
    public void TomatoButton()
    {
        placeObjectOnPlane.ChangeObjectToPlace(6);
    }
}
