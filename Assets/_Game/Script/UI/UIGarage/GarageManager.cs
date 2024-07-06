using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GarageManager : MonoBehaviour
{
    public Transform parentSpawn;
    public MotorVisual motor;
    public CharacterVisual characterVisual;
    public Animator animator;
    public static Action<GarageManager> ActionInitGarage = null;
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
        motor = Instantiate(motorVisual, parentSpawn);
        motor.transform.localPosition = Vector3.zero;
    }
    private void RemoveMotorCache()
    {
        if(motor != null)
        {
            DestroyImmediate(motor.gameObject);
            motor = null;
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
}
