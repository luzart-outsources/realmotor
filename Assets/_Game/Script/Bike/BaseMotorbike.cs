namespace Luzart
{
    using MoreMountains.HighroadEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    public class BaseMotorbike : MonoBehaviour
    {
        [SerializeField]
        private BaseMotor baseMotor;
        [SerializeField]
        private BaseCharacter baseCharacter;
        [SerializeField]
        private MiniMapPlayer miniMapPlayer;
        private BaseController baseController;
        public InforMotorbike inforMotorbike;
        [Sirenix.OdinInspector.ShowInInspector]
        public int currentIndex { get; set; } = 0;
        [SerializeField]
        private float radiusCheckPoint = 10f;
        [SerializeField]
        private LayerMask layerCheckPoint;
        private string _strMyName;
        public string strMyName
        {
            get
            {
                return _strMyName;
            }
            set
            {
                inforRacing.InitName(value);
                _strMyName = value;
            }
        }
        public ETeam eTeam { get; set; }
        [SerializeField]
        private Transform parentVisualMotor;
        [SerializeField]
        private InforRacing inforRacing;
    
        public DB_Motorbike dbMotorbike { get; set; }
        private DB_Character dbCharacter;
    
        public SoundMotorbike soundMotorbike;
        public List<int> listIndex = new List<int>();
        public EStateMotorbike eState { get; set; }
    
        public Transform parentCam;
        public Transform targetCameraStartGame;
        public AutoRotationDownProjector autoRotation;
    
        public Transform posCamera;
    
        [Sirenix.OdinInspector.ShowInInspector]
        public int round { get; set; } = 0;
        public float Speed
        {
            get
            {
                return baseMotor.Speed;
            }
            set
            {
                baseMotor.Speed = value;
            }
        }
        public bool isFall { get; set; } = false;
    
        public float timePlay { get; set; }
    
        public Vector3 velocity
        {
            get
            {
                return transform.InverseTransformDirection(transform.forward * Speed);
            }
        }
        [Sirenix.OdinInspector.ShowInInspector]
        public int currentGear
        {
            get
            {
                if (inforMotorbike != null)
                {
                    float maxSpeed = inforMotorbike.maxSpeed;
                    float currentSpeed = Speed;
                    return ConfigStats.GetGear(currentSpeed, maxSpeed);
                }
                return 0;
            }
        }
        [Sirenix.OdinInspector.ShowInInspector]
        public float revValue
        {
            get
            {
                if (inforMotorbike != null)
                {
                    float maxSpeed = inforMotorbike.maxSpeed;
                    float currentSpeed = Speed;
                    return ConfigStats.GetRevValue(currentSpeed, maxSpeed);
                }
                return 0;
            }
        }
        public Action<ResultOnCollisionLayer> actionOnCollisionLayer;
        public void UpdateIndex(int index)
        {
            inforRacing.UpdateIndex(index);
        }
        public void Initialize(InforMotorbike inforMotorbike, BaseController baseController, ETeam eTeam)
        {
            this.inforMotorbike = inforMotorbike;
            this.baseController = baseController;
            this.eTeam = eTeam;
            this.baseMotor.Initialize(this);
            this.baseCharacter.Initialize(this);
            this.baseController.Initialized(this);
            this.soundMotorbike.Initialize(this);
            this.miniMapPlayer.Initialize(this);
            this.inforRacing.Initialize(this);
            GetCurrentCheckPoint();
            //transform.LookAt(GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex));
            if (eTeam == ETeam.Player)
            {
                CameraManager.Instance.SetFollowCamera(posCamera, this);
            }
            InitAction();
            isFall = false;
            autoRotation.transform.localPosition = new Vector3(0, 3, 0);
            autoRotation.transform.localEulerAngles = new Vector3(90, 0, 0);
            //autoRotation.enabled = false;
        }
        public void InitSpawn(DB_Character db_Character, DB_Motorbike dbMotorBike)
        {
            this.dbMotorbike = dbMotorBike;
            this.dbCharacter = db_Character;
            var db_Motor = DataManager.Instance.motorSO.GetDBMotor(dbMotorBike.idMotor);
            var prefabs = ResourcesManager.Instance.LoadMotor(db_Motor.idVisualMotor);
            baseMotor.visualMotor = Instantiate(prefabs, parentVisualMotor);
            //baseMotor.visualMotor.transform.localPosition = Vector3.zero;
            baseMotor.transform.localScale = Vector3.one * 0.53f;
            baseCharacter.InitSpawn(db_Character);
            baseMotor.InitSpawn();
            eState = EStateMotorbike.Start;
        }
        public void InitStartRace()
        {
            round = 0;
            currentIndex = 0;
            countDead = 0;
            listIndex.Add(0);
        }
        private void OnAdsReInitialize()
        {
            if(eTeam == ETeam.Player && eState != EStateMotorbike.Finish)
            {
                AdsWrapperManager.Instance.ShowInter(KeyAds.OnCollisionWall, ReInitialize);
            }
            else
            {
                ReInitialize();
            }
    
        }
        public void ReInitialize()
        {
            if (eState != EStateMotorbike.Finish)
            {
                eState = EStateMotorbike.None;
                if (eTeam == ETeam.Player)
                {
                    GameManager.Instance.gameCoordinator.ShowHideUIController(true);
                }
            }
            Initialize(inforMotorbike, baseController, eTeam);
            Transform transPoint =  GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex);
            transform.position = transPoint.position;
            transform.rotation = transPoint.rotation;
    
        }
        private void InitAction()
        {
            baseMotor.ActionCollisionWall = OnVisualCharacterCollisionWall;
        }
        public void GetCurrentCheckPoint()
        {
            if (isFall) return;
    
            // Lấy danh sách các Collider gần nhất
            List<Collider> nearbyColliders = GetListCol(1);
            if (nearbyColliders == null || nearbyColliders.Count == 0) return;
    
            // Sắp xếp các Collider theo khoảng cách đến điểm WavingPoint
            nearbyColliders.Sort((c1, c2) =>
                DistanceForWavingPoint(c1.transform).CompareTo(DistanceForWavingPoint(c2.transform))
            );
    
            // Duyệt qua các Collider và xử lý điểm WavingPoint hợp lệ đầu tiên
            foreach (var collider in nearbyColliders)
            {
                WavingPoint wavingPoint = collider.GetComponent<WavingPoint>();
                if (wavingPoint == null) continue;
                var allWayPoint = GameManager.Instance.gameCoordinator.wavingPointGizmos.allWavePoint;
                int waveIndex = wavingPoint.indexPoint;
                bool isLastWavePoint = (waveIndex >= allWayPoint.Length*4/5);
                // Nếu đây là điểm cuối cùng và danh sách chỉ số chưa có đủ 3 phần tử
                if (isLastWavePoint && listIndex.Count < allWayPoint.Length*3/5) return;
    
                // Cập nhật chỉ số điểm hiện tại
                currentIndex = waveIndex;
    
                // Thêm chỉ số mới vào danh sách nếu chưa tồn tại
                if (!listIndex.Contains(waveIndex))
                {
                    listIndex.Add(waveIndex);
                }
    
                // Kết thúc xử lý sau khi tìm thấy điểm WavingPoint hợp lệ đầu tiên
                return;
            }
        }
        private List<Collider> GetListCol(int factor)
        {
            var col = Physics.OverlapSphere(transform.position, radiusCheckPoint * factor, layerCheckPoint);
            if (col != null && col.Length > 0)
            {
                List<Collider> listCol = col.ToList();
                listCol.RemoveAll(collider => DistanceForWavingPoint(collider.transform) <= 0);
                if (listCol.Count == 0)
                {
                    factor++;
                    return null;
                }
                return listCol;
            }
            return null;
        }
        public float DistanceForWavingPoint(Transform posWavingPoint)
        {
            Vector3 directPos = posWavingPoint.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, directPos);
            if (dotProduct < 0)
            {
                return -Vector3.Distance(transform.position, posWavingPoint.position);
            }
            else
            {
                return Vector3.Distance(transform.position, posWavingPoint.position);
            }
        }
        public float DistanceForWavingPoint()
        {
            Transform posWavingPoint = GameManager.Instance.gameCoordinator.wavingPointGizmos.allWavePoint[currentIndex].transform;
            Vector3 directPos = posWavingPoint.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, directPos);
            if (dotProduct < 0)
            {
                return -Vector3.Distance(transform.position, posWavingPoint.position);
            }
            else
            {
                return Vector3.Distance(transform.position, posWavingPoint.position);
            }
        }
        public void OnFinishLine()
        {
            if(listIndex.Count < GameManager.Instance.gameCoordinator.wavingPointGizmos.allWavePoint.Length*4/5)
            {
                return;
            }
            else
            {
                round++;
                if(GameManager.Instance.gameCoordinator.db_Level.level != 0)
                {
                    currentIndex = 0;
                    listIndex.Clear();
                }
    
    
                GameManager.Instance.gameCoordinator.OnMemberPassFinishLine(this);
    
            }
    
        }
    
        public void OnFinishRace()
        {
            this.timePlay = GameManager.Instance.gameCoordinator.timePlay;
            eState = EStateMotorbike.Finish;
            if (eTeam == ETeam.AI)
            {
                OnFinishRaceAI();
            }
            else if(eTeam == ETeam.Player)
            {
                OnFinishRacePlayer();
            }
        }
    
        private void OnFinishRaceAI()
        {
            //Destroy(baseController);
            //GameUtil.Instance.WaitAndDo(this,1, StopWinGame);
        }
        private void OnFinishRacePlayer()
        {
            Destroy(baseController);
            baseController = gameObject.AddComponent<VehicleAI>();
            baseController.Initialized(this);
    
            GameUtil.Instance.WaitAndDo(this,1, StopWinGame);
        }
        private void StopWinGame()
        {
            //Brake();
        }
    
        public int countDead = 0;
        private void OnVisualCharacterCollisionWall (Vector3 velocity)
        {
            if(eState != EStateMotorbike.Finish)
            {
                if (eTeam == ETeam.Player)
                {
                    GameManager.Instance.gameCoordinator.ShowHideUIController(false);
                }
            }
    
            baseCharacter.OnCollisionWall(velocity);
            UnVerticle();
            UnHorizontal();
            isFall = true;
            autoRotation.enabled = true;
            countDead++;
            GameUtil.Instance.WaitAndDo(this,2f, OnAdsReInitialize);
        }
        public void StartRace()
        {
            eState = EStateMotorbike.None;
            IsStartRace = true;
        }
        private void Update()
        {
            if(!IsStartRace)
            {
                return;
            }
            UpdateController();
            GetCurrentCheckPoint();
        }
        public float GetDistanceFromTarget()
        {
            Transform target = GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex + 1);
            return Vector3.Distance(transform.position, target.position);
        }
    
        private bool IsStartRace = false;
    
        #region Controller
        public int valueVerticle { get; set; } = 0;
        public int valueHorizontal { get; set; } = 0;
        private void FixedUpdateController()
        {
            if (baseController != null)
            {
                baseController.FixedUpdateController();
            }
        }
        private void UpdateController()
        {
            if(baseController != null)
            {
                baseController.UpdateController();
            }
        }
        public void MoveLeft()
        {
            baseMotor.MoveLeft();
            baseCharacter.MoveLeft();
            valueHorizontal = -1;
        }
        public void MoveRight()
        {
            baseMotor.MoveRight();
            baseCharacter.MoveRight();
            valueHorizontal = 1;
        }
        public void MoveUp()
        {
            baseMotor.MoveUp();
            baseCharacter.MoveUp();
            valueVerticle = 1;
        }
        public void Brake()
        {
            baseMotor.Brake();
            baseCharacter.Brake();
            valueVerticle = -1;
        }
        public void UnHorizontal()
        {
            baseMotor.UnHorizontal();
            baseCharacter.UnHorizontal();
            valueHorizontal = 0;
        }
        public void UnVerticle()
        {
            baseMotor.UnVerticle();
            baseCharacter.UnVerticle();
            valueVerticle = 0;
        }
        public void MoveVisual(Vector3 velocity)
        {
            baseMotor.OnVisualMove(velocity);
        }
        public void MoveSteerVisual(int steerInput, float currentSpeed)
        {
            baseMotor.OnVisualTilt(steerInput, currentSpeed);
            baseCharacter.OnVisualTilt(steerInput);
        }
        #endregion
    
        private void OnDestroy()
        {
            if(GameUtil.Instance != null)
            {
                GameUtil.Instance.StopAllCoroutinesForBehaviour(this);
            }
        }
    
    #if UNITY_EDITOR
        public bool isDrawRadius = false;
        private void OnDrawGizmos()
        {
            if (!isDrawRadius)
            {
                return;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,radiusCheckPoint);
        }
    #endif
    }
    
    public enum ETeam
    {
        None =0,
        Player = 1,
        AI = 2,
    }
    
    public enum EStateMotorbike
    {
        None = 0,
        Start = 1,
        Finish = 2,
    }
}
