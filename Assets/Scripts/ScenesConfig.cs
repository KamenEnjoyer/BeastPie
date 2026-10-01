using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static GILData;

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

