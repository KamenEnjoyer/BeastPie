using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text text;
    private StorageContentData ingredient;

    public void Setup(StorageContentData data, bool checkCount = false)
    {
        if (checkCount && data.count == 0)
        {
            Clear();
            return;
        }
        ingredient = data;
        icon.sprite = data.icon;
        text.text = data.data.ingName;
        gameObject.GetComponent<IngredientHover>()?.SetIngredient(data);
        gameObject.SetActive(true);
    }
    
    public void SetSlotScale(float scale = 1.2f)
    {
        gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 
            gameObject.GetComponent<RectTransform>().rect.height * scale);
    }

    public StorageContentData GetIng()
    {
        return ingredient;
    }

    public void Clear(bool update = false)
    {
        icon.sprite = null;
        text.text = "";
        ingredient = null;
        gameObject.SetActive(false);
        if (update) MixDishMode.Instance.UpdateResult(); //А С ЗЕЛЬЯМИ ТО ЧЁ ДЕЛАТЬ?!
    }
}
