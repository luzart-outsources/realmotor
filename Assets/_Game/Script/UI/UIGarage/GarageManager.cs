using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GarageManager : MonoBehaviour
{
    [Space, Header("Motor")]
    public GameObject obMotor;
    public Transform parentSpawn;
    public MotorVisual _motorCache;

    [Space, Header("Character")]
    public GameObject obCharacter;
    public CharacterVisual characterVisual;
    public Animator animator;
    public static Action<GarageManager> ActionInitGarage = null;

    [Space, Header("Camera")]
    [SerializeField]
    private Camera cameraMain;
    [SerializeField]
    private Transform transformCameraGarage;
    [SerializeField]
    private Transform transformCameraHeader;
    [SerializeField]
    private Transform transformCameraBody;

    private void Start()
    {
        ActionInitGarage?.Invoke(this);
    }
    public void SetActiveMotorCharacter(bool active)
    {
        parentSpawn.gameObject.SetActive(active);
    }
    public void SpawnMotorVisual(int idMotor)
    {
        RemoveMotorCache();
        var motorVisual = ResourcesManager.Instance.LoadMotor(idMotor);
        _motorCache = Instantiate(motorVisual, parentSpawn);
        _motorCache.transform.localPosition = Vector3.zero;
    }
    private void RemoveMotorCache()
    {
        if(_motorCache != null)
        {
            DestroyImmediate(_motorCache.gameObject);
            _motorCache = null;
        }

    }
    public void ChangeIdHelmet(int id)
    {
        DB_Character character = new DB_Character();
        character.idHelmet = id;
        character.idClothes = DataManager.Instance.GameData.curCharacter.idClothes;
        characterVisual.InitDBCharacter(character);
    }
    public void ChangeIdBody(int id)
    {
        DB_Character character = new DB_Character();
        character.idHelmet = DataManager.Instance.GameData.curCharacter.idHelmet;
        character.idClothes = id;
        characterVisual.InitDBCharacter(character);
    }
    public void SetMyCharacter()
    {
        characterVisual.InitDBCharacter(DataManager.Instance.GameData.curCharacter);
    }

    public void ChangeCameraMotor()
    {
        cameraMain.transform.DOMove(transformCameraGarage.position, 1f);
        cameraMain.transform.DORotateQuaternion(transformCameraGarage.rotation, 1f);
    }
    public void ChangeCameraHeader()
    {
        animator.Play("OrcIdle");
        cameraMain.transform.DOMove(transformCameraHeader.position, 1f);
        cameraMain.transform.DORotateQuaternion(transformCameraHeader.rotation, 1f);
    }
    public void ChangeCameraBody()
    {
        animator.Play("Standard Idle");
        cameraMain.transform.DOMove(transformCameraBody.position, 1f);
        cameraMain.transform.DORotateQuaternion(transformCameraBody.rotation, 1f);
    }
    public void SetActiveMotor(bool status)
    {
        obMotor.SetActive(status);
    }
    public void SetActiveCharacter(bool status)
    {
        obCharacter.SetActive(status);
    }
    public void OnInScreenUIGarage()
    {
        SetActiveCharacter(false);
        SetActiveMotor(true);
    }
    public void OnInScreenUIRacer()
    {
        SetActiveCharacter(true);
        SetActiveMotor(false);
    }
}
