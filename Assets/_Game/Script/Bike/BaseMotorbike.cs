using MoreMountains.HighroadEngine;
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
    public int currentIndex = 0;
    [SerializeField]
    private float radiusCheckPoint = 10f;
    [SerializeField]
    private LayerMask layerCheckPoint;
    //[SerializeField]
    //private MiniMapPlayer miniMapPlayer;
    public string strMyName;
    public ETeam eTeam;
    [SerializeField]
    private Transform parentVisualMotor;
    public DB_Motorbike dbMotorbike;
    private DB_Character dbCharacter;

    public SoundMotorbike soundMotorbike;
    public List<int> listIndex = new List<int>();
    public EStateMotorbike eState;

    public Transform parentCam;

    public int round = 0;
    public float Speed => baseMotor.Speed;
    public bool isFall = false;

    public float timePlay;

    public Vector3 velocity
    {
        get
        {
            return transform.InverseTransformDirection(transform.forward * Speed);
        }
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
        eState = EStateMotorbike.None;
        GetCurrentCheckPoint();
        //transform.LookAt(GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex));
        if(eTeam == ETeam.Player)
        {
            CameraManager.Instance.SetFollowCamera(this.gameObject);
        }
        InitAction();
        isFall = false;
    }
    public void InitSpawn(DB_Character db_Character, DB_Motorbike dbMotorBike)
    {
        this.dbMotorbike = dbMotorBike;
        this.dbCharacter = db_Character;
        var prefabs = ResourcesManager.Instance.LoadMotor(dbMotorbike.idMotor);
        baseMotor.visualMotor = Instantiate(prefabs, parentVisualMotor);
        baseMotor.visualMotor.transform.localPosition = Vector3.zero;
        baseMotor.transform.localScale = Vector3.one*0.8f;
        baseCharacter.InitSpawn(db_Character);
        baseMotor.InitSpawn();
    }
    public void InitStartRace()
    {
        round = 0;
        currentIndex = 0;
        listIndex.Add(0);
    }
    public void ReInitialize()
    {
        if(eState == EStateMotorbike.Finish)
        {
            return;
        }
        Initialize(inforMotorbike, baseController, eTeam);
        Transform transPoint =  GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex);
        transform.position = transPoint.position;
        transform.rotation = transPoint.rotation;
        if (eTeam == ETeam.Player)
        {
            GameManager.Instance.gameCoordinator.ShowHideUIController(true);
        }
    }
    private void InitAction()
    {
        baseMotor.ActionCollisionWall = OnVisualCharacterCollisionWall;
    }
    public void GetCurrentCheckPoint()
    {
        if (isFall)
        {
            return;
        }
        if(eState == EStateMotorbike.Finish)
        {
            return;
        }
        List<Collider> listCol = GetListCol(1);
        if(listCol !=null && listCol.Count > 0)
        {
            listCol.Sort(
                (collider1, collider2)
                            => DistanceForWavingPoint(collider1.transform).CompareTo(DistanceForWavingPoint(collider2.transform))
                            );
            foreach (var c in listCol)
            {
                var checkPointCol = c.GetComponent<WavingPoint>();
                if (checkPointCol != null)
                {
                    int currentIndex = checkPointCol.indexPoint;
                    if (currentIndex == GameManager.Instance.gameCoordinator.wavingPointGizmos.allWavePoint.Length - 1)
                    {
                        if (listIndex.Count >=3)
                        {
                            this.currentIndex = currentIndex;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        this.currentIndex = currentIndex;
                    }
                    if (!listIndex.Contains(currentIndex))
                    {
                        listIndex.Add(currentIndex);
                    }
                    return;

                }
            }
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
            currentIndex = 0;
            listIndex.Clear();

            GameManager.Instance.gameCoordinator.OnPassFinishLine(this);

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
        Destroy(baseController);
        GameUtil.Instance.WaitAndDo(1, StopWinGame);
    }
    private void OnFinishRacePlayer()
    {
        Destroy(baseController);
        baseController = gameObject.AddComponent<VehicleAI>();
        baseController.Initialized(this);

        GameUtil.Instance.WaitAndDo(1, StopWinGame);
    }
    private void StopWinGame()
    {
        Brake();
    }

    private void OnVisualCharacterCollisionWall (Vector3 velocity)
    {
        if(eTeam == ETeam.Player)
        {
            GameManager.Instance.gameCoordinator.ShowHideUIController(false);
        }
        baseCharacter.OnCollisionWall(velocity);
        UnVerticle();
        UnHorizontal();
        isFall = true; 
        GameUtil.Instance.WaitAndDo(2f, ReInitialize);
    }
    public void StartRace()
    {
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
    }
    public void MoveRight()
    {
        baseMotor.MoveRight();
        baseCharacter.MoveRight();
    }
    public void MoveUp()
    {
        baseMotor.MoveUp();
        baseCharacter.MoveUp();
    }
    public void Brake()
    {
        baseMotor.Brake();
        baseCharacter.Brake();
    }
    public void UnHorizontal()
    {
        baseMotor.UnHorizontal();
        baseCharacter.UnHorizontal();
    }
    public void UnVerticle()
    {
        baseMotor.UnVerticle();
        baseCharacter.UnVerticle();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radiusCheckPoint);
    }
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