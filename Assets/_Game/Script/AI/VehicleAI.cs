﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Luzart;

namespace MoreMountains.HighroadEngine
{

	public class VehicleAI : BaseController 
	{
		/// If Active, AI controls the vehicle
		public bool Active;

		[Header("AI configuration")]
		[Range(0,10)]
		/// The time in seconds before the car is reset to the last checkpoint it's met. If the value is zero, the car will remain where it is.
		public float TimeBeforeStuck = 5f;
		[Range(5,30)]
		/// when this distance is reached, AI goes to next waypoint
		public int MinimalDistanceToNextWaypoint = 10; 

		[Range(0f, 1f)]
		/// the maximum Gas Pedal Amount 
		public float FullThrottle = 1f; 

		[Range(0f, 1f)]
		/// the minimum Gas Pedal Amount
		public float SmallThrottle = 0.3f; 

		public bool OverrideSteringSpeed = false;
		public int SteeringSpeed = 300;

		// Constants used by the AI engine
		// Feel free to edit these values. Just be sure to test thoroughly the new AI vehicle driving behaviour
		public float _largeAngleDistance = 90f; // When angle between front of the vehicle and target waypoint are distant 
        public  float _smallAngleDistance = 30f;  // When angle between front of the vehicle and target waypoint are near
        public float _minimalSpeedForBrakes = 0.1f; // When vehicle is at least at this speed, AI can use brakes
        public float _maximalDistanceStuck = 0.5f; // Distance to consider vehicle stuck

		public List<Vector3> _AIWaypoints;
		protected int _currentWaypoint;
		protected float _direction = 0f;
		protected float _acceleration = 0f;
		protected Vector3 _targetWaypoint;
		protected float _targetAngleAbsolute;
		protected int _newDirection;
		protected float _stuckTime = 0f;
		protected Vector3 _lastPosition;
        private LayerMask layerFinishLine;
        private BaseMotorbike motorPlayer;
        private GameCoordinator gameCoordinator
        {
            get
            {
                return GameManager.Instance.gameCoordinator;
            }
        }
        private void Start()
        {
            int layer = LayerMask.NameToLayer("FinishLine");
            layerFinishLine = 1 << layer;
        }
        /// <summary>
        /// Initialization
        /// </summary>

        public override void Initialized(BaseMotorbike baseMotorbike)
        {
            base.Initialized(baseMotorbike);
            List<Vector3> ais = gameCoordinator.wavingPointGizmos.GetAllWavePoint();
            int length = ais.Count;
            for (int i = 0; i < length; i++)
            {
                var item = ais[i];
                ais[i] = new Vector3(item.x + Random.Range(-3, 3), item.y, item.z + Random.Range(-3, 3));
            }
            _AIWaypoints = ais;
            baseMotorBike.GetCurrentCheckPoint();
            _currentWaypoint = baseMotorbike.currentIndex;
            _targetWaypoint = _AIWaypoints[_currentWaypoint];
            motorPlayer = gameCoordinator.myMotorbike;
        }

        /// <summary>
        /// At LateUpdate, we apply ou AI logic
        /// </summary>
        public override void UpdateController()
		{
            if (!IsInit)
            {
                return;
            }
            // if the AI can't control this vehicle, we do nothing and exit
            //if (!_controller.IsPlaying || !Active)
            //{
            //	return;
            //}

            switch (baseMotorBike.eState)
            {
                case EStateMotorbike.None:
                    {
                        OnAIMoveNormal();
                        break;
                    }
                case EStateMotorbike.Finish:
                    {
                        OnAIMoveNormal();
                        break;
                    }
            }

			
        }
        private bool IsCanMove = true;
        private void OnAIMoveNormal()
        {
            if(DataManager.Instance.CurrentLevel >= 10)
            {
                Move();
            }
            else
            {
                ClaimMove();
            }
        }
        private void ClaimMove()
        {
            if (IsAIMovementFinishLine() && DataManager.Instance.CurrentLevel <= 5)
            {
                UnVerticle();
                UnHorizontal();
                baseMotorBike.Speed = Mathf.Clamp(baseMotorBike.Speed, 15f, Time.deltaTime);
                if (baseMotorBike.Speed <= 5)
                {
                    baseMotorBike.Speed = 10f;
                }

                return;
            }
            if (IsAIMovementFarPlayer())
            {
                UnVerticle();
                UnHorizontal();
                IsCanMove = false;
                return;
            }
            if (IsAIMovementNearPlayer())
            {
                IsCanMove = true;
            }
            if (IsCanMove)
            {
                Move();
            }
        }
        private void Move()
        {
            EvaluateNextWaypoint();

            EvaluateDirection();

            CalculateValues();

            // we update controller inputs
            if (_acceleration > 0)
            {
                MoveUp();
            }
            else
            {
                Brake();
            }
            if (IsDontControll)
            {
                return;
            }
            if (_direction > 0f)
            {
                MoveRight();
            }
            else if (_direction < 0f)
            {
                MoveLeft();
            }
            else
            {
                UnHorizontal();
            }
        }
        private float radiusFinishLine = 250f;
        private bool IsAIMovementFinishLine()
        {
            if (baseMotorBike != null && baseMotorBike.eTeam == ETeam.AI)
            {
                bool isCollisionFinishLine = false;
                int maxPoint = gameCoordinator.wavingPointGizmos.allWavePoint.Length - 1;
                if(gameCoordinator.db_Level.level == 0)
                {
                    maxPoint = 287;
                }
                float distance = gameCoordinator.DisFromPlayer(baseMotorBike, maxPoint);
                if (distance <= radiusFinishLine)
                {
                    isCollisionFinishLine = true;
                }
                if (isCollisionFinishLine && IsRoundFinal)
                {
                    return true;
                }
            }
            return false;
        }
        public float disFarMotor = 100f;
        public bool IsAIMovementFarPlayer()
        {
            if (baseMotorBike != null && baseMotorBike.eTeam == ETeam.AI)
            {
                float distance = gameCoordinator.DisFrom2Player(baseMotorBike, motorPlayer);
                if (distance >= disFarMotor)
                {
                    return true;
                }
            }
            return false;
        }
        public float disNearMotor = -20f;
        public bool IsAIMovementNearPlayer()
        {
            if (baseMotorBike != null && baseMotorBike.eTeam == ETeam.AI)
            {
                float distance = gameCoordinator.DisFrom2Player(baseMotorBike, motorPlayer);
                if (distance <= disNearMotor)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsRoundFinal
        {
            get
            {
                if(baseMotorBike == null)
                {
                    return false;
                }
                if(baseMotorBike.round >= gameCoordinator.db_Level.lapRequire - 1&& baseMotorBike.listIndex.Count >= gameCoordinator.wavingPointGizmos.allWavePoint.Length*0.6f)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }



        //private void OnAIMoveFinish()
        //{
        //    if (IsStuck())
        //    {
        //        //_controller.Respawn();
        //        return;
        //    }

        //    EvaluateNextWaypoint();

        //    EvaluateDirection();

        //    CalculateValues();

        //    if (IsDontControll)
        //    {
        //        return;
        //    }

        //    if (_direction > 0f)
        //    {
        //        MoveRight();
        //    }
        //    else if (_direction < 0f)
        //    {
        //        MoveLeft();
        //    }
        //    else
        //    {
        //        UnHorizontal();
        //    }
        //}


        /// <summary>
        /// Reset next AI waypoint to closest
        /// </summary>
        protected virtual void ResetAIWaypointToClosest()
        {
            int indexChoosen = -1;
            float localMinimumDistance = float.MaxValue;
            for (int i = 0; i < _AIWaypoints.Count; i++)
            {
                Vector3 heading = _AIWaypoints[i] - transform.position;
                float facing = Vector3.Dot(heading, transform.forward);

                if (facing > 0)                    
                {
                    float distance = Vector3.Distance(_AIWaypoints[i], transform.position);
                    if (distance < localMinimumDistance)
                    {
                        localMinimumDistance = distance;
                        indexChoosen = i;
                    }
                }
            }
            _currentWaypoint = indexChoosen;
            _targetWaypoint = _AIWaypoints[indexChoosen];
        }


        /// <summary>
        /// If car stays in the same place for too long, we respawn to the last checkpoint
        /// </summary>
        protected virtual bool IsStuck()
		{
			if (TimeBeforeStuck > 0) 
			{
				if (Vector3.Distance(_lastPosition,transform.position) < _maximalDistanceStuck)
				{
					if (_stuckTime == 0f)
					{
						_stuckTime = Time.time;
					}
				}
				else 
				{
					_lastPosition = transform.position;
					_stuckTime = 0;
				}

				if ((_stuckTime > 0f) && (Time.time - _stuckTime) > TimeBeforeStuck) 
				{
					_stuckTime = 0;
					return true;
				}

				return false;
			}

			return false;
		}

		/// <summary>
		/// we determine if the current waypoint is still correct
		/// </summary>
		protected virtual void EvaluateNextWaypoint()
		{
			var distanceToWaypoint = PlaneDistanceToWaypoints();
			// if we are close enough to the current waypoint, we switch to the next one
			if (distanceToWaypoint < MinimalDistanceToNextWaypoint)
			{
				_currentWaypoint++;
				// after one lap, we go back to checkpoint 1
				if (_currentWaypoint == _AIWaypoints.Count)
				{
					_currentWaypoint = 0;
				}
				// we set the new target waypoint
				_targetWaypoint = _AIWaypoints[_currentWaypoint];
			}
		}

        Vector3 targetVector;
        private bool IsDontControll = false;
        /// <summary>
        /// Determine direction towards the waypoint
        /// </summary>
        protected virtual void EvaluateDirection()
		{
			// In case of SolidController, dans les les loopings et les déplacements "non classiques", on désactive la direction et laisse
			// foncer la voiture tout droit
			//if (_solidController != null)
			//{
			//	if (Mathf.Clamp(_solidController.HorizontalAngle, 0.9f, 1.1f) != _solidController.HorizontalAngle)
			//	{
			//		return;
			//	}
			//}
			// we compute the target vector between the vehicle and the next waypoint on a plane (without Y axis)
			targetVector = _targetWaypoint - transform.position;
			targetVector.y = 0;
            targetVector = targetVector.normalized;
			Vector3 transformForwardPlane = transform.forward;
			transformForwardPlane.y = 0;
			// then we measure the angle from vehicle forward to target Vector
			_targetAngleAbsolute = Vector3.Angle(transformForwardPlane, targetVector);

            // we also compute the cross product in order to find out if the angle is positive 
            Vector3 cross = Vector3.Cross(transformForwardPlane, targetVector);
			// this value indicates if the vehicle has to move towards the left or right
            int newDir = cross.y >= 0 ? 1 : -1;
            float turn = baseMotorBike.inforMotorbike.handling * Time.deltaTime;


            _newDirection = 0;

            if (turn > _targetAngleAbsolute)
            {
                transform.forward = targetVector.normalized;
                //transform.Rotate(0, _targetAngleAbsolute * newDir, 0);
                IsDontControll = true;
                return;
            }
            IsDontControll = false;
            if (cross.y < 0)
            {
                _newDirection = -1;
            }
            else if (cross.y > 0)
            {
                _newDirection = 1;
            }

		}

		/// <summary>
		/// Applies controls to move vehicle towards the waypoint
		/// </summary>
		protected virtual void CalculateValues()
		{
			// now, we apply _direction & _acceleration values 
			// if the vehicle is looking towards the opposite direction ?
			if (_targetAngleAbsolute > _largeAngleDistance)
			{
				// we steer to the proper direction
				_direction = -_newDirection;
				// if we have enough speed, we brake to rotate faster
				if (baseMotorBike.Speed > baseMotorBike.inforMotorbike.maxSpeed* _minimalSpeedForBrakes)
				{
					_acceleration = -FullThrottle;
				}
				else
				{
					// otherwise we accelerate slowly
					_acceleration = SmallThrottle;
				}
				// else if the vehicle is not pointing towards the waypoint but also not too far ? 
			}
			else if (_targetAngleAbsolute > _smallAngleDistance)
			{
				// we steer to the proper direction
				_direction = _newDirection;
				// we acceleration slowly
				_acceleration = SmallThrottle;
			}
			else
			{
				// if the vehicle is facing the waypoint, we switch to full speed
				_direction = 0f;
				_acceleration = FullThrottle;
			}
		}

		/// <summary>
		/// Returns the Plane distance between the next waypoint and the vehicle
		/// </summary>
		/// <returns>The distance to the next waypoint.</returns>
		protected virtual float PlaneDistanceToWaypoints()
		{
			Vector2 target = new Vector2(_targetWaypoint.x, _targetWaypoint.z);
			Vector2 position = new Vector2(transform.position.x, transform.position.z);

			return Vector2.Distance(target, position);
		}	

		/// <summary>
		/// On DrawGizmos, we draw a line between the vehicle and its target
		/// </summary>
		public virtual void OnDrawGizmos() 
		{
			#if UNITY_EDITOR

			// we draw a line between the vehicle & target waypoint
			if (_AIWaypoints != null && (_AIWaypoints.Count >= (_currentWaypoint + 1)))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, _AIWaypoints[_currentWaypoint]);
			}
            if(baseMotorBike!=null && baseMotorBike.eTeam == ETeam.Player)
            {
                int length = _AIWaypoints.Count;
                for (int i = 0; i < length; i++)
                {
                    int next = (i + 1) % length;
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(_AIWaypoints[i], _AIWaypoints[next]);
                }
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusFinishLine);
#endif
        }

	}

}
