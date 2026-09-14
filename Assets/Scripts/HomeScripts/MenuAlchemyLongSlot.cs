using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAlchemyLongSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI nameText;

    private GILData potionData;

    public void Setup(GILData data)
    {
        potionData = data;

        icon.sprite = Resources.Load<Sprite>("IngredientsSprites/default");
        nameText.text = potionData.ingName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(potionData == null) return;
        MenuAlchemy.Instance?.ShowPotionInformation(potionData);
    }
}
