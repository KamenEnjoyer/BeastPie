using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MapGeneration : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private GameObject locationButtonPrefab;

    private float zoomSpeed = 0.1f;
    private float minZoom = 0.5f;
    private float maxZoom = 2.5f;

    private Vector2 lastMousePosition;
    private float currentZoom = 1f;

    private float width;
    private float height;

    private bool dragLock = false;

    private void Start()
    {
        width = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;
        GenerateMap();
    }

    private void GenerateMap()
    {
        List<MapFactory.MapSlot> mapLocations = MapFactory.LoadMap();
        
        foreach (var location in mapLocations)
        {
            GameObject zoneButton = Instantiate(locationButtonPrefab, transform);

            RectTransform rect = zoneButton.GetComponent<RectTransform>();

            ZoneType zone = Resources.Load<ZoneType>("ZoneTypes/" + location.id);
            rect.anchoredPosition = new Vector2(zone.mapPosition.x * width, zone.mapPosition.y * height);

            MapSlot slot = zoneButton.GetComponent<MapSlot>();

            slot.Setup(zone, location.canMove, location.unlocked);
        }
    }

    public void OnMapZoom(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            SetZoom(currentZoom - zoomSpeed * currentZoom);
        }
        else if (value.Get<float>() < 0)
        {
            SetZoom(currentZoom + zoomSpeed * currentZoom);
        }
    }

    private void SetZoom(float value)
    {
        currentZoom = Mathf.Clamp(value, minZoom, maxZoom);

        transform.localScale = Vector3.one * currentZoom;

        ClampPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragLock) return;

        Vector2 delta = eventData.position - lastMousePosition;

        GetComponent<RectTransform>().anchoredPosition += delta;

        ClampPosition();

        lastMousePosition = eventData.position;
    }

    private void ClampPosition()
    {
        Vector2 mapSize = GetComponent<RectTransform>().rect.size * GetComponent<RectTransform>().localScale;
        Vector2 viewportSize = transform.parent.GetComponent<RectTransform>().rect.size;

        Vector2 position = GetComponent<RectTransform>().anchoredPosition;

        float maxX = (mapSize.x - viewportSize.x) / 2f + (viewportSize.x*0.2f);
        float maxY = (mapSize.y - viewportSize.y) / 2f + (viewportSize.x * 0.2f);

        // Если карта меньше окна, не позволяем ей двигаться.
        if (mapSize.x <= viewportSize.x) position.x = 0;
        else position.x = Mathf.Clamp(position.x, -maxX, maxX);

        if (mapSize.y <= viewportSize.y) position.y = 0;
        else position.y = Mathf.Clamp(position.y, -maxY, maxY);

        GetComponent<RectTransform>().anchoredPosition = position;
    }
}
