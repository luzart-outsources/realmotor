namespace Luzart
{
    using DG.Tweening;
    using DynamicShadowProjector;
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
        public float valueRotateCurrentMotor = 293f;
        public Transform parentSpawn;
        public Transform parentSpawnMotor;
        public MotorVisual _motorCache;
        public DrawTargetObject softShadowProjector;
    
        [Space, Header("Character")]
        public GameObject obCharacter;
        public CharacterVisual characterVisual;
        public Animator animator;
        public static Action<GarageManager> ActionInitGarage = null;
    
        [Space, Header("Camera")]
        [SerializeField]
        private Camera cameraMain;
        [SerializeField]
        private Transform transformFirstCameraGarage;
        [SerializeField]
        private Transform transformCameraGarage;
        [SerializeField]
        private Transform transformFirstCameraHeader;
        [SerializeField]
        private Transform transformCameraHeader;
        [SerializeField]
        private Transform transformCameraBody;
        private Tween twMovePos;
        private Tween twMoveRos;
    
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
            var db_Motor = DataManager.Instance.motorSO.GetDBMotor(idMotor);
            var motorVisual = ResourcesManager.Instance.LoadMotor(db_Motor.idVisualMotor);
            _motorCache = Instantiate(motorVisual, parentSpawnMotor);
            _motorCache.transform.localPosition = Vector3.zero;
            softShadowProjector.SetCommandBufferDirty();
        }
    
        public void RotateCurrentMotor()
        {
            obMotor.transform.DORotate(new Vector3(0, valueRotateCurrentMotor, 0), 0);
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
        public void ChangeCameraHome()
        {
            twMoveRos?.Kill();
            twMovePos?.Kill();
            cameraMain.transform.rotation = transformCameraGarage.transform.rotation;
            cameraMain.transform.position = transformCameraGarage.transform.position;
            twMovePos = cameraMain.transform.DOMove(transformFirstCameraGarage.position, 0.3f);
            twMoveRos = cameraMain.transform.DORotateQuaternion(transformFirstCameraGarage.rotation, 0.3f);
        }
        public void ChangeCameraMotor()
        {
            twMoveRos?.Kill();
            twMovePos?.Kill();
            cameraMain.transform.rotation = transformFirstCameraGarage.transform.rotation;
            cameraMain.transform.position = transformFirstCameraGarage.transform.position;
            twMovePos = cameraMain.transform.DOMove(transformCameraGarage.position, 0.3f);
            twMoveRos = cameraMain.transform.DORotateQuaternion(transformCameraGarage.rotation, 0.3f);
        }
        public void ChangeCameraHeader()
        {
            twMoveRos?.Kill();
            twMovePos?.Kill();
            animator.Play("OrcIdle");
            //cameraMain.transform.rotation = transformCameraGarage.transform.rotation;
            //cameraMain.transform.position = transformCameraGarage.transform.position;
            twMovePos = cameraMain.transform.DOMove(transformCameraHeader.position, 0.3f);
            twMoveRos = cameraMain.transform.DORotateQuaternion(transformCameraHeader.rotation, 0.3f);
        }
        public void ChangeCameraBody()
        {
            twMoveRos?.Kill();
            twMovePos?.Kill();
            animator.Play("Standard Idle");
            twMovePos = cameraMain.transform.DOMove(transformCameraBody.position, 0.3f);
            twMoveRos = cameraMain.transform.DORotateQuaternion(transformCameraBody.rotation, 0.3f);
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
}
