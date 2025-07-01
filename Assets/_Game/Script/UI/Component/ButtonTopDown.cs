namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    
    public class ButtonTopDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button btn;
        public Action OnClickDown;
        public Action OnClickUp;
        public Image[] ims;
        public Sprite[] spriteDowns;
        public Sprite[] spriteUps;
    
        public void AddListener(Action clickDown, Action clickUp)
        {
            this.OnClickDown = clickDown;
            this.OnClickUp = clickUp;   
        }
        private void SetSprite(Image im, Sprite sp)
        {
            if(im != null)
            {
                im.sprite = sp;
            }
        }
        private bool isUp = false;
        private void SetAllSprite(Image[] ims, Sprite[] sps)
        {
            if(ims != null)
            {
                for(int i = 0; i < ims.Length; i++)
                {
                    SetSprite(ims[i], sps[i]);
                }
            }
        }
    
        public void ForcePointUp()
        {
            if (isUp)
            {
                return;
            }
            isUp = true;
            OnClickUp?.Invoke();
            SetAllSprite(ims, spriteUps);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isUp = false;
            OnClickDown?.Invoke();
            SetAllSprite(ims, spriteDowns);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isUp)
            {
                return;
            }
            isUp = true;
            OnClickUp?.Invoke();
            SetAllSprite(ims, spriteUps);
        }
    }
}
