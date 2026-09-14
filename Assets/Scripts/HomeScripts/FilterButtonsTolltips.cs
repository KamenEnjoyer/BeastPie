using UnityEngine;
using UnityEngine.EventSystems;
public class FilterButtonsTooltips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipMessage;
    
    private float tooltipDelay = 1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 pos = transform.position + new Vector3(0, -20, 0);
        TooltipUI.Instance.ShowDelayed(tooltipMessage, pos, tooltipDelay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideImmediate();
    }

    public void GetTooltipMessage(string input)
    {
        tooltipMessage = input;
    }
}
