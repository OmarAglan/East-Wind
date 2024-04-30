using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //ref To the buildings Placer
    private BuildingPlacer _buildingPlacer;

    /// <summary>
    /// For Buildings Stuff
    /// </summary>
    public Transform buildingMenu;
    public GameObject buildingButtonPrefab;
    /// <summary>
    /// For Resources Handling
    /// </summary>
    public Transform resourcesUIParent;
    public GameObject gameResourceDisplayPrefab;

    private Dictionary<string, TMP_Text> _resourceTexts;

    private Dictionary<string, Button> _buildingButtons;
    private void Awake()
    {
        _buildingPlacer = GetComponent<BuildingPlacer>();

        // Create texts for each in-game resource (For Now The IS Only "Gold").
        _resourceTexts = new Dictionary<string, TMP_Text>();

        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            GameObject display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent);
            display.name = pair.Key;
            _resourceTexts[pair.Key] = display.transform.Find("Text").GetComponent<TMP_Text>();
            _SetResourceText(pair.Key, pair.Value.Amount);
        }
        // Create buttons for each building type
        _buildingButtons = new Dictionary<string, Button>();
        for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            GameObject button = GameObject.Instantiate(
                buildingButtonPrefab,
                buildingMenu);
            string code = Globals.BUILDING_DATA[i].Code;
            button.name = code;
            button.transform.Find("Text").GetComponent<TMP_Text>().text = code;
            Button b = button.GetComponent<Button>();
            _AddBuildingButtonListener(b, i);

            _buildingButtons[code] = b;
            if (!Globals.BUILDING_DATA[i].CanBuy())
            {
                b.interactable = false;
            }
        }
    }

    private void _AddBuildingButtonListener(Button b, int i)
    {
        b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
    }
    private void _SetResourceText(string resource, int value)
    {
        _resourceTexts[resource].text = value.ToString();
    }
    public void UpdateResourceTexts()
    {
        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            _SetResourceText(pair.Key, pair.Value.Amount);
        }
    }
    public void CheckBuildingButtons()
    {
        foreach (BuildingData data in Globals.BUILDING_DATA)
        {
            _buildingButtons[data.Code].interactable = data.CanBuy();
        }
    }
}
