namespace Luzart
{
    using DG.Tweening;
    using JetBrains.Annotations;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    public class CountdownUI : MonoBehaviour
    {
        public int count;
        public int targetCount;
        [ReadOnly]
        private int currentCount;    
        public Action onDone;
        public TMP_Text txt;
        public bool IsInitOnStart = false;
    
        public Transform countdownImage;
        public Sprite[] countdownSprites;
        private void Start()
        {
            if (IsInitOnStart)
            {
                StartCountDown();
            }
        }
        public void InitCountDown(int firstCount, int targetCount, Action onDone)
        {
            this.count = firstCount; 
            this.targetCount = targetCount;
            currentCount = firstCount;
            this.onDone = onDone;
            SetText(currentCount.ToString());
            //SetImage(currentCount);
            StartCountDown();
    
        }
        private Coroutine corIECountdown = null;
        private void StartCountDown()
        {
            if (corIECountdown != null)
            {
                StopCoroutine(corIECountdown);
            }
            corIECountdown = StartCoroutine(IECountdown());
        }
        private IEnumerator IECountdown()
        {
            WaitForSeconds wait = new WaitForSeconds(1);
            while(currentCount > targetCount)
            {
                yield return wait;
                currentCount--;
                SetText(currentCount.ToString());
                //SetImage(currentCount);
                if (currentCount == targetCount)
                {
                    onDone?.Invoke();
                }
            }
        }
        private Tween tw;
        private void SetText(string str)
        {
            txt.text = str;
            tw?.Kill();
            tw = txt.transform.DOPunchScale(Vector3.one * 1.1f - Vector3.one, 0.5f, 1, 1);
        }
        private void SetImage(int count)
        {
            if (count > 0 && count <= countdownSprites.Length)
            {
                countdownImage.GetComponent<Image>().sprite = countdownSprites[count - 1];
                tw?.Kill();
                tw = countdownImage.DOPunchScale(Vector3.one * 1.1f - Vector3.one, 0.5f, 1, 1);
            }
        }
        private void OnDisable()
        {
            if (corIECountdown != null)
            {
                StopCoroutine(corIECountdown);
            }
        }
    }
}
