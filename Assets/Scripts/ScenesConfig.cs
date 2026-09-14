using UnityEngine;

public static class ScenesConfig
{
    public static bool IsHome { get; set; } = true;

    public enum sortingVariables
    {
        Default, 
        Name,
        Density,
        Quantity,
        Type
    }
    public static sortingVariables currentSorting { get; set; } = sortingVariables.Default;
}

