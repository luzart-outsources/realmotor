using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Vector3 m_localScale = Vector3.one;
    private float valueScale = 0.9f;
    private float timeScale = 0.1f;

    private Coroutine corIEScale = null;
    private IEnumerator IEScale()
    {
        float time = 0;
        //float delta = (scale - 1) / (timeScale / Time.deltaTime);
        while (time <= timeScale && transform.localScale.x >= m_localScale.x * valueScale)
        {
            yield return null;
            time += Time.deltaTime;
            transform.localScale = ((valueScale - 1) * time / timeScale + 1) * m_localScale;
        }
        transform.localScale = m_localScale * valueScale;
    }
    private Coroutine corIEDeScale = null;
    private IEnumerator IEDeScale()
    {
        float time = timeScale;
        //float delta = (scale - 1) / (timeScale / Time.deltaTime);
        while (time <= timeScale && transform.localScale.x < m_localScale.x)
        {
            yield return null;
            time -= Time.deltaTime;
            transform.localScale = ((valueScale - 1) * time / timeScale + 1) * m_localScale;
        }
        transform.localScale = m_localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (corIEScale != null)
        {
            StopCoroutine(corIEScale);
        }
        if (corIEDeScale != null)
        {
            StopCoroutine(corIEDeScale);
            transform.localScale = m_localScale;
        }
        corIEScale = StartCoroutine(IEScale());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (corIEDeScale != null)
        {
            StopCoroutine(corIEDeScale);
        }
        if (corIEScale != null)
        {
            StopCoroutine(corIEScale);
        }
        corIEDeScale = StartCoroutine(IEDeScale());

    }
    private void OnDisable()
    {
        if (corIEDeScale != null)
        {
            StopCoroutine(corIEDeScale);
        }
        if (corIEScale != null)
        {
            StopCoroutine(corIEScale);
        }
        transform.localScale = m_localScale;
    }
}
