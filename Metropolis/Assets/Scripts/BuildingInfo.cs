using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfo : MonoBehaviour
{
    public TextMeshProUGUI buildingTypeText;
    public TextMeshProUGUI statsText;
    public BuildingTypes buildingType;
    public int occupants;
    public int moneyGenerated;
    public GameObject buildingIcon;

    [SerializeField] Sprite houseIcon;
    [SerializeField] Sprite flatsIcon;
    [SerializeField] Sprite factoryIcon;
    [SerializeField] Sprite supermarketIcon;
    [SerializeField] Sprite hospitalIcon;

    public void UpdateUI()
    {
        switch (buildingType)
        {
            case BuildingTypes.House:
                buildingTypeText.text = $"House";
                buildingIcon.GetComponent<Image>().sprite = houseIcon;
                break;
            case BuildingTypes.Flats:
                buildingTypeText.text = $"Flats";
                buildingIcon.GetComponent<Image>().sprite = flatsIcon;
                break;
            case BuildingTypes.Factory:
                buildingTypeText.text = $"Factory";
                buildingIcon.GetComponent<Image>().sprite = factoryIcon;
                break;
            case BuildingTypes.Supermarket:
                buildingTypeText.text = $"Supermarket";
                buildingIcon.GetComponent<Image>().sprite = supermarketIcon;
                break;
            case BuildingTypes.Hospital:
                buildingTypeText.text = $"Hospital";
                buildingIcon.GetComponent<Image>().sprite = hospitalIcon;
                break;
        }

        statsText.text = $"Occupants: {occupants}\nMoney Being Generated: ${moneyGenerated}";
    }
}
