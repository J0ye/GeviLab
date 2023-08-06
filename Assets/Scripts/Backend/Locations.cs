using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Locations : MonoBehaviour
{
    private static Locations instance;
    private static List<Location> locations = null;

    public static Locations Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Locations();
            }
            return instance;
        }
    }

    public void AddLocation(Location location)
    {
        if (locations != null)
            locations.Add(location);
    }

    public void RemoveLocation(Location location)
    {
        if (locations != null)
            locations.Remove(location);
    }

    public static List<Location> GetLocations()
    {
        return locations;
    }

    public static void InitializeLocations(GameObject spherePrefab, GameObject itemPrefab, GameObject bridgePrefab)
    {
        foreach (Location location in locations)
        {
            location.Initialize(spherePrefab, itemPrefab, bridgePrefab);
        }
    }
    public static void SyncLocations()
    {
        foreach (Location location in locations)
        {
            location.Sync();
        }
    }
    public static bool LoadLocationsLocal(string filePath)
    {
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (json.StartsWith("{"))
            {
                // If the JSON string starts with a '{', it is a JSON object and needs to be wrapped in an array
                json = "[" + json + "]";
            }
            locations = JsonConvert.DeserializeObject<List<Location>>(json);
            // Debug.Log("Loaded " + locations.Count + " locations from " + fileName);
            // foreach (Location location in locations)
            // {
            //     Debug.Log(
            //         $"{location.id} {location.name} {location.description} {location.position} {location.rotation} {location.scale}"
            //     );
            // }
            return true;
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
        return false;
    }

    /// <summary>
    /// Serializes the list of locations to a JSON file at the specified file path.
    /// </summary>
    /// <param name="filePath">The file path where the JSON file will be saved.</param>
    public static void SerializeLocationsToJson(string filePath)
    {
        SyncLocations(); // Sync the transform properties to the current object transformations before serializing
        // Debug.Log("Serializing " + locations.Count + " locations to " + filePath);
        string json = JsonConvert.SerializeObject(locations, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}
