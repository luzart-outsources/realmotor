namespace Luzart
{
    #if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.SceneManagement;
    using UnityEditor.SceneManagement;
    
    public class AddObjectOnMouseClick : EditorWindow
    {
        private GameObject objectToInstantiate;
        private bool isEnabled = false;
    
        [MenuItem("Tools/Add Object On Mouse Click")]
        public static void ShowWindow()
        {
            GetWindow<AddObjectOnMouseClick>("Add Object On Mouse Click");
        }
        [MenuItem("Tools/Game")]
        public static void Game()
        {
            // Tên của scene bạn muốn chuyển đến
            string sceneName = "Game";
    
            // Kiểm tra xem scene có tồn tại trong Build Settings hay không
            if (IsSceneInBuildSettings(sceneName))
            {
                // Chuyển scene
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(GetBuildIndex(sceneName)));
            }
        }
    
        [MenuItem("Tools/Garage")]
        public static void Garage()
        {
            // Tên của scene bạn muốn chuyển đến
            string sceneName = "Garage";
    
            // Kiểm tra xem scene có tồn tại trong Build Settings hay không
            if (IsSceneInBuildSettings(sceneName))
            {
                // Chuyển scene
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(GetBuildIndex(sceneName)));
            }
        }
        [MenuItem("Tools/Play")]
        public static void Play()
        {
            Game();
            EditorApplication.isPlaying = true;
        }
        // Kiểm tra xem scene có tồn tại trong Build Settings hay không
        static bool IsSceneInBuildSettings(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneFileName == sceneName)
                {
                    return true;
                }
            }
            return false;
        }
        static int GetBuildIndex(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneFileName == sceneName)
                {
                    return i;
                }
            }
            return -1;
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
        // Thêm mục vào menu Tools
        [MenuItem("Tools/Hide Layer")]
        static void HideLayer()
        {
            // Tên của layer bạn muốn ẩn
            string layerName = "MiniMapLayer";
    
            // Lấy số thứ tự của layer từ tên layer
            int layer = LayerMask.NameToLayer(layerName);
    
            if (layer != -1)
            {
                // Tạo một mask để ẩn layer
                Tools.visibleLayers &= ~(1 << layer);
                SceneView.RepaintAll();
                Debug.Log("Layer '" + layerName + "' is now hidden in the Scene view.");
            }
            else
            {
                Debug.LogError("Layer '" + layerName + "' does not exist.");
            }
        }
    
        // Thêm mục vào menu Tools
        [MenuItem("Tools/Show Layer")]
        static void ShowLayer()
        {
            // Tên của layer bạn muốn hiển thị
            string layerName = "MiniMapLayer";
    
            // Lấy số thứ tự của layer từ tên layer
            int layer = LayerMask.NameToLayer(layerName);
    
            if (layer != -1)
            {
                // Tạo một mask để hiển thị layer
                Tools.visibleLayers |= (1 << layer);
                SceneView.RepaintAll();
                Debug.Log("Layer '" + layerName + "' is now visible in the Scene view.");
            }
            else
            {
                Debug.LogError("Layer '" + layerName + "' does not exist.");
            }
        }
    }
    #endif
}
