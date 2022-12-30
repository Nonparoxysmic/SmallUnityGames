using UnityEngine;
using UnityEditor;

public class GridWallsWindow : EditorWindow
{
    bool _gridWallsSelected;
    GridWalls _selected;

    GridWallsWindow()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem("Window/TacticalCreatureBattle/Grid Walls Tool")]
    static void ShowWindow()
    {
        GridWallsWindow window = GetWindow<GridWallsWindow>();
        window.titleContent = new GUIContent("Grid Walls Tool");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (_gridWallsSelected)
        {
            // Draw GUI for editing tools.
            bool doRedraw = false;
            Color newWallColor = EditorGUILayout.ColorField("Wall Color", _selected.WallColor, null);
            if (newWallColor != _selected.WallColor)
            {
                _selected.WallColor = newWallColor;
                doRedraw = true;
            }
            EditorGUILayout.Space();
            float newThickness = EditorGUILayout.FloatField
                (
                    "Wall Thickness",
                    _selected.WallThickness,
                    null as GUILayoutOption[]
                );
            if (newThickness != _selected.WallThickness)
            {
                _selected.WallThickness = newThickness;
                doRedraw = true;
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Remember to save the scene after making changes.");
            if (doRedraw)
            {
                EditorUtility.SetDirty(_selected);
                SceneView.RepaintAll();
            }
        }
        else
        {
            // Draw GUI for no selection.
            EditorGUILayout.LabelField("Select a GridWalls object to edit its properties.");
        }
    }

    void OnSelectionChange()
    {
        UpdateCurrentSelection();
        Repaint();
    }

    void OnFocus()
    {
        UpdateCurrentSelection();
        Repaint();
    }

    void UpdateCurrentSelection()
    {
        GameObject selected = Selection.activeObject as GameObject;
        if (selected == null)
        {
            _gridWallsSelected = false;
            _selected = null;
            return;
        }
        GridWalls gridWalls = selected.GetComponent<GridWalls>();
        if (gridWalls == null)
        {
            _gridWallsSelected = false;
            _selected = null;
            return;
        }
        _selected = gridWalls;
        _gridWallsSelected = true;
    }

    void OnPlayModeStateChanged(PlayModeStateChange stateChange)
    {
        if (_gridWallsSelected)
        {
            Selection.activeObject = null;
            _gridWallsSelected = false;
            _selected = null;
        }
    }
}
