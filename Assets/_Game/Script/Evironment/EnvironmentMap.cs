namespace Luzart
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    
    public class EnvironmentMap : MonoBehaviour
    {
        public WavingPointGizmos wavingPointGizmos;
        public Transform[] startPoint;
        public Transform parentMotorbike;
        [SerializeField]
        private MiniMapEnvironment _miniMapEnvironment;
        public MiniMapEnvironment miniMapEnvironment
        {
            get
            {
                if(_miniMapEnvironment == null)
                {
                    _miniMapEnvironment = FindAnyObjectByType<MiniMapEnvironment>();
                }
                return _miniMapEnvironment;
            }
        }
        private void Awake()
        {
            cameraStartGame.gameObject.SetActive(false);
        }
        public static Action<EnvironmentMap> actionMap;
        [Space, Header("Lighting Motorbike")]
        public bool isOverrideMotorLighting = false;
        public Color colorMotorLighting;
        public float intensityMotorLighting = 0.4f;
    
        public SequenceCameraCinemachineTrackedDolly cameraStartGame;
        public SequenceCameraCinemachineTrackedDolly cameraEndGame; 
        public void StartCameraGame()
        {
            cameraStartGame.gameObject.SetActive(true);
        }
        public void StartCameraEndGame()
        {
            cameraEndGame.gameObject.SetActive(true);
        }
        public void Start()
        {
    #if UNITY_EDITOR
            if(TestManager.Instance != null)
            {
    
            }
            else
    #endif
            {
                InvokeRegisterMap();
            }
    
        }
        public void InvokeRegisterMap()
        {
            actionMap?.Invoke(this);
        }
        public Transform GetStartPoint(int index)
        {
            return startPoint[index];
        }
    
    #if UNITY_EDITOR
        [Space, Header("Editor")]
        public Transform startPointEditor;
        public bool IsSetStart = false;
        [Sirenix.OdinInspector.Button]
        public void GetAllStartPoint()
        {
            startPoint = startPointEditor.GetComponentsInChildren<Transform>();
            List<Transform> listWave = new List<Transform>();
            for (int i = 0; i < startPoint.Length; i++)
            {
                if (!startPoint[i].name.Contains("StartPoint"))
                {
                    continue;
                }
                listWave.Add(startPoint[i]);
            }
            startPoint = listWave.ToArray();
        }
        [Sirenix.OdinInspector.Button]
        public void SetUpMap()
        {
            SetAllColliderAndLayer();
            FindWavePoint();
            FindStartRace();
            FindParentMotorbike();
    
        }
        public void FindWavePoint()
        {
            GameObject ob = GameObject.Find("W-P-C");
            wavingPointGizmos = ob.GetComponent<WavingPointGizmos>();
            if (wavingPointGizmos == null)
            {
                wavingPointGizmos = ob.AddComponent<WavingPointGizmos>();
            }
            wavingPointGizmos.AddWavingPointChildren();
            wavingPointGizmos.GetAllWavePointEditor();
        }
        public void FindStartRace()
        {
            GameObject startRace = GameObject.Find("StartRace");
            if (startRace == null)
            {
                string prefabPath = "Assets/_Game/Prefabs/ToolMap/StartEndRace.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                PrefabUtility.InstantiatePrefab(prefab, transform);
            }
            startRace = GameObject.Find("StartEndRace");
            StartEndRace startEndRace = startRace.GetComponent<StartEndRace>();
            startEndRace.GetAllStartPoint();
            startPointEditor = startRace.transform;
            GameObject startGrid = GameObject.Find("StartGrid");
            if (startGrid != null)
            {
                startPointEditor.transform.position = startGrid.transform.position;
            }
    
            GetAllStartPoint();
        }
        public void FindParentMotorbike()
        {
            GameObject obParentMotorbike = GameObject.Find("ParentMotorbike");
            if (obParentMotorbike == null)
            {
                obParentMotorbike = new GameObject("ParentMotorbike");
                obParentMotorbike.transform.parent = transform;
                obParentMotorbike.transform.localPosition = Vector3.zero;
            }
            parentMotorbike = obParentMotorbike.transform;
        }
        private void SetAllColliderAndLayer()
        {
            LayerMask layerWall = LayerMask.NameToLayer("Wall");
            LayerMask layerGround = LayerMask.NameToLayer("Ground");
            LayerMask layerRoad = LayerMask.NameToLayer("Road");
    
            List<GameObject> obFences = GameUtil.FindGameObjectsByName("Collider-Fences");
            foreach (GameObject item in obFences)
            {
                item.layer = layerWall;
            }
    
            List<GameObject> obGrass = GameUtil.FindGameObjectsByName("Collider-Grass");
            foreach (GameObject item in obGrass)
            {
                item.layer = layerGround;
            }
    
            List<GameObject> obRoad = GameUtil.FindGameObjectsByName("Collider-Road");
            foreach (GameObject item in obRoad)
            {
                item.layer = layerRoad;
            }
            List<GameObject> obBridge = GameUtil.FindGameObjectsByName("Collider-Bridge");
            foreach (GameObject item in obBridge)
            {
                item.layer = layerRoad;
            }
        }
        public void RemoveCameraBigScreen()
        {
            List<GameObject> obs = GameUtil.FindGameObjectsByName("Camera");
            for (int i = 0; i < obs.Count; i++)
            {
    
            }
        }
    
    #endif
    }
}
