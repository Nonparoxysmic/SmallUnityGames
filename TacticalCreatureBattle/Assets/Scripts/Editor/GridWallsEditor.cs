using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridWalls))]
public class GridWallsEditor : Editor
{
    void OnSceneGUI()
    {
        GridWalls gridWalls = target as GridWalls;
        if (gridWalls == null)
        {
            return;
        }
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        EventType eventType = e.GetTypeForControl(controlID);
        if (eventType == EventType.MouseDown)
        {
            int height = SceneView.currentDrawingSceneView.camera.pixelHeight;
            Vector3 coords = new Vector3(e.mousePosition.x, height - e.mousePosition.y - 1, 0);
            coords = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(coords);
            gridWalls.Click(coords);
            e.Use();
        }
        Selection.activeObject = gridWalls.gameObject;
    }
}
