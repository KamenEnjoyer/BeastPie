using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MapFactory
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/map.json");

    [Serializable]
    public struct MapSlot
    {
        public string id;
        public bool unlocked;
        public bool canMove;
    }

    public static List<MapSlot> LoadMap()
    {
        if (!File.Exists(savePath))
        {
            List<MapSlot> newZonesList = GenerateDefaultZones();
            SaveResources(newZonesList);
            return newZonesList;
        }
        else
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<Wrapper>(json).slots;
        }
    }

    private static List<MapSlot> GenerateDefaultZones()
    {
        ZoneType[] allZones = Resources.LoadAll<ZoneType>("ZoneTypes/");
        List<MapSlot> newZonesList = new List<MapSlot>();
        foreach(var zone in allZones)
        {
            bool unlocked = false;
            if (zone.zoneType == ZoneType.ZoneName.Reality) unlocked = true;
            newZonesList.Add(new MapSlot
            {
                id = zone.id,
                unlocked = unlocked,
                canMove = true
            });
        }
        return newZonesList;
    }

    public static void SaveResources(List<MapSlot> slots)
    {
        Wrapper wrapper = new Wrapper { slots = slots };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    [System.Serializable]
    private class Wrapper { public List<MapSlot> slots; }
}
