using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;

    private ZoneType zone;
    private bool canMove;
    private bool unlocked;

    public void Setup(ZoneType data, bool canMove, bool unlocked)
    {
        if (!unlocked) icon.sprite = Resources.Load<Sprite>("ZonesSprites/default");
        else icon.sprite = Resources.Load<Sprite>("ZonesSprites/" + data.id);

        if (canMove) icon.color = new Color(1f, 1f, 1f, 1f);
        else icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        zone = data;
        this.canMove = canMove;
        this.unlocked = unlocked;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ZoneDescription.Instance.SetInfo(zone, canMove, unlocked);
    }
}
