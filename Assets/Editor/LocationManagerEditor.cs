using UnityEditor;
using UnityEngine;

namespace GeViLab.Backend
{
    [CustomEditor(typeof(LocationManager))]
    public class LocationManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocationManager locationManager = (LocationManager)target;
            if (locationManager == null || Locations.GetLocations() == null)
            {
                return;
            }

            GUILayout.Space(10);

            GUILayout.Label("Locations:");

            foreach (Location location in Locations.GetLocations())
            {
                GUILayout.Label(location.name + ":" + location.description);                
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Save Locations to JSON"))
            {
                string path = EditorUtility.SaveFilePanel(
                    "Save Locations to JSON",
                    locationManager.config.LocationFilePath,
                    locationManager.config.LocationFileName,
                    "json"
                );
                if (path.Length != 0)
                {
                    Locations.SerializeLocationsToJson(path);
                }
            }
        }
    }
}
