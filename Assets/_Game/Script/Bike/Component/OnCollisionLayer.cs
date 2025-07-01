namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class OnCollisionLayer : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private LayerCheck layerCheck;
        public Action<ResultOnCollisionLayer> actionOnCollisionLayer;
        public enum CollisionType { Enter = 0, Stay = 1, Exit =2 } ;
        public CollisionType collisionType;
        private BaseMotorbike _baseMotorbike = null;
        private BaseMotorbike baseMotorbike
        {
            get
            {
                if(_baseMotorbike == null)
                {
                    _baseMotorbike = GetComponent<BaseMotorbike>();
                }
                return _baseMotorbike;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collisionType == CollisionType.Enter)
                OnCollision(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collisionType == CollisionType.Stay)
                OnCollision(collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            if(collisionType == CollisionType.Exit) 
            OnCollision(collision);
        }
        protected virtual void OnCollision(Collision collision)
        {
            int layer = collision.gameObject.layer;
            if (GameUtil.IsLayerInLayerMask(layer, layerMask))
            {
                ResultOnCollisionLayer result = new ResultOnCollisionLayer();
                result.layerCheck = layerCheck;
                result.collision = collision;
                actionOnCollisionLayer?.Invoke(result);
            }
        }
    }
    public enum LayerCheck
    {
        None = 0,
        Wall = 1,
        Bike =2,
    }
    [System.Serializable]
    public struct ResultOnCollisionLayer
    {
        public LayerCheck layerCheck;
        public Collision collision;
    }
}
