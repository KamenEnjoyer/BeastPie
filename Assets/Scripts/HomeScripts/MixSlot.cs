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
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
    }

    public StorageContentData GetIng()
    {
        return ingredient;
    }

    public void Clear()
    {
        icon.sprite = null;
        text.text = "";
        ingredient = null;
        gameObject.SetActive(false);
    }
}
