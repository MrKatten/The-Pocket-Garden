using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantsMenu : MonoBehaviour
{
    [SerializeField] public PlaceObjectOnPlane placeObjectOnPlane;
    [SerializeField] public bool[] _buyed;
    [SerializeField] public int[] _prices;
    [SerializeField] public GameObject[] _lockers;
    [SerializeField] public int money;
    [SerializeField] public TMP_Text moneyText;

    private void Awake()
    {
        moneyText.text = money.ToString();
    }
    public void UpdateMoneyUI()
    {
        moneyText.text = money.ToString();
    }
    public void CactusButton()
    {
        placeObjectOnPlane.ChangeObjectToPlace(0);
    }
    public void Coleus1Button()
    {
        if (_buyed[1])
        {
            placeObjectOnPlane.ChangeObjectToPlace(1);
        }
        else if (money >= _prices[1])
        {
            _lockers[1].SetActive(false);
            money -= _prices[1];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(1);
        }
    }
    public void Coleus2Button()
    {
        if (_buyed[2])
        {
            placeObjectOnPlane.ChangeObjectToPlace(2);
        }
        else if (money >= _prices[2])
        {
            _lockers[2].SetActive(false);
            money -= _prices[2];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(2);
        }
    }
    public void Coleus3Button()
    {
        if (_buyed[3])
        {
            placeObjectOnPlane.ChangeObjectToPlace(3);
        }
        else if (money >= _prices[3])
        {
            _lockers[3].SetActive(false);
            money -= _prices[3];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(3);
        }
    }
    public void CucumberButton()
    {
        if (_buyed[4])
        {
            placeObjectOnPlane.ChangeObjectToPlace(4);
        }
        else if (money >= _prices[4])
        {
            _lockers[4].SetActive(false);
            money -= _prices[4];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(4);
        }
    }
    public void MonsteraButton()
    {
        if (_buyed[5])
        {
            placeObjectOnPlane.ChangeObjectToPlace(5);
        }
        else if (money >= _prices[5])
        {
            _lockers[5].SetActive(false);
            money -= _prices[5];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(5);
        }
    }
    public void TomatoButton()
    {
        if (_buyed[6])
        {
            placeObjectOnPlane.ChangeObjectToPlace(6);
        }
        else if (money >= _prices[6])
        {
            _lockers[6].SetActive(false);
            money -= _prices[6];
            moneyText.text = money.ToString();
            placeObjectOnPlane.ChangeObjectToPlace(6);
        }
    }
}
