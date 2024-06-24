using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class BaseMotorbike : MonoBehaviour
{
    [SerializeField]
    private BaseMotor baseMotor;
    [SerializeField]
    private BaseCharacter baseCharacter;
    private BaseController baseController;
    public InforMotorbike inforMotorbike;
    public int currentIndex = 0;
    [SerializeField]
    private float radiusCheckPoint = 10f;
    [SerializeField]
    private LayerMask layerCheckPoint;
    public ETeam eTeam;
    [SerializeField]
    private Transform parentVisualMotor;
    private DB_Motorbike dbMotorbike;
    private DB_Character dbCharacter;

    public int round = 0;
    public float Speed => baseMotor.Speed;

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

        GetCurrentCheckPoint();
        transform.LookAt(GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex));
        if(eTeam == ETeam.Player)
        {
            CameraManager.Instance.SetFollowCamera(this.gameObject);
        }
        InitAction();
    }
    public void InitSpawn(DB_Character db_Character, DB_Motorbike dbMotorBike)
    {
        this.dbMotorbike = dbMotorBike;
        this.dbCharacter = db_Character;
        var prefabs = ResourcesManager.Instance.LoadMotor(dbMotorbike.idMotor);
        baseMotor.visualMotor = Instantiate(prefabs, parentVisualMotor);
        baseMotor.visualMotor.transform.localPosition = Vector3.zero;
        baseCharacter.InitSpawn(db_Character);
        baseMotor.InitSpawn();
    }
    public void InitStartRace()
    {
        round = 0;
    }
    public void ReInitialize()
    {
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
        var col = Physics.OverlapSphere(transform.position, radiusCheckPoint, layerCheckPoint);
        if (col != null && col.Length>0)
        {
            List<Collider> listCol = col.ToList();
            listCol.Sort(
                (collider1, collider2) 
                => Vector3.Distance(transform.position, collider1.transform.position).CompareTo(Vector3.Distance(transform.position, collider2.transform.position))
                );
            foreach (var c in listCol)
            {
                var checkPointCol = c.GetComponent<WavingPoint>();
                currentIndex = checkPointCol.indexPoint;
                return;
            }
        }
    }
    public void OnFinishLine()
    {
        round++;
        GameManager.Instance.gameCoordinator.OnPassFinishLine(this);
    }

    public void OnFinishRace()
    {
        if(eTeam == ETeam.AI)
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
        Debug.Log($"FinishRace AI {name}");
    }
    private void OnFinishRacePlayer()
    {
        Debug.Log($"FinishRace Player {name}");
    }

    private void OnVisualCharacterCollisionWall (Vector3 velocity)
    {
        baseCharacter.OnCollisionWall(velocity);
        GameUtil.Instance.WaitAndDo(2f, ReInitialize);
    }
    private void FixedUpdate()
    {
        FixedUpdateController();
        GetCurrentCheckPoint();
    }
    private void Update()
    {
        UpdateController();
    }
    public float GetDistanceFromTarget()
    {
        Transform target = GameManager.Instance.gameCoordinator.wavingPointGizmos.GetTransformIndex(currentIndex + 1);
        return Vector3.Distance(transform.position, target.position);
    }



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