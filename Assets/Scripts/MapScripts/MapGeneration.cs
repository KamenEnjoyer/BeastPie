using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private GameObject locationButtonPrefab;

    [SerializeField] private List<ZoneType> locations;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        List<MapFactory.MapSlot> mapLocations = MapFactory.LoadMap(); 
        foreach (var location in mapLocations)
        {
            GameObject zoneButton = Instantiate(locationButtonPrefab, transform);

            float width = gameObject.GetComponent<RectTransform>().rect.width;
            float height = gameObject.GetComponent<RectTransform>().rect.height;
            RectTransform rect = zoneButton.GetComponent<RectTransform>();

            ZoneType zone = Resources.Load<ZoneType>("ZoneTypes/" + location.id);
            rect.anchoredPosition = new Vector2(zone.mapPosition.x * width, zone.mapPosition.y * height);

            MapSlot slot = zoneButton.GetComponent<MapSlot>();

            slot.Setup(zone, location.canMove, location.unlocked);
        }
    }
}
