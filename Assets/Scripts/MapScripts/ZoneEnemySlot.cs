using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoneEnemySlot : MonoBehaviour
{
    public Image icon;

    public void Setup(EnemyTypeAsset data)
    {
        icon.sprite = Resources.Load<Sprite>("EnemyIconSprites/" + data.id);
    }
}
