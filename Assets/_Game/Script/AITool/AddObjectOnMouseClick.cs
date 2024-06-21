using UnityEngine;
using UnityEditor;

public class AddObjectOnMouseClick : EditorWindow
{
    private GameObject objectToInstantiate;
    private bool isEnabled = false;

    [MenuItem("Tools/Add Object On Mouse Click")]
    public static void ShowWindow()
    {
        GetWindow<AddObjectOnMouseClick>("Add Object On Mouse Click");
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        objectToInstantiate = (GameObject)EditorGUILayout.ObjectField("Object to Instantiate", objectToInstantiate, typeof(GameObject), false);

        if (GUILayout.Button(isEnabled ? "Disable" : "Enable"))
        {
            isEnabled = !isEnabled;
            SceneView.duringSceneGui -= OnSceneGUI;
            if (isEnabled)
            {
                SceneView.duringSceneGui += OnSceneGUI;
            }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.I)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                Vector3 position = hitInfo.point;
                if (objectToInstantiate != null)
                {
                    GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToInstantiate);
                    newObject.transform.position = position;
                    Transform parent = Transform.FindAnyObjectByType<WavingPointGizmos>().transform;
                    newObject.transform.SetParent(parent);
                    Undo.RegisterCreatedObjectUndo(newObject, "Add " + newObject.name);
                }
            }
            e.Use();
        }
    }
}