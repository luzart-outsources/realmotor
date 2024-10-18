using UnityEngine;

public class RTSCam : Singleton<RTSCam>
{
	public bool dragged;

	private float orbitSpeedX;

	private float orbitSpeedY = 4f;

	public float rotXSpeedModifier = 5.4f;

	public float rotYSpeedModifier = 5.4f;

	public float minRotX = -20f;

	public float maxRotX = 70f;

	public float panSpeedModifier = 1f;

	public bool dfDragFlag;
	public bool isDrag = false;

	private void Start()
	{
	}

	private void OnEnable()
	{
		Gesture.onDraggingE += OnDragging;
	}

	private void OnDisable()
	{
		Gesture.onDraggingE -= OnDragging;
	}

	private void Update()
	{
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        float y = eulerAngles.y;

        Quaternion lhs = Quaternion.Euler(0f, y, 0f) * Quaternion.Euler(0f, orbitSpeedY * Time.deltaTime, 0f);

        base.transform.parent.rotation = lhs;

        if (dragged)
        {
            orbitSpeedY *= 1f - Time.deltaTime * 1.8f;
        }
    }

	private void OnDragging(DragInfo dragInfo)
	{
        if (!isDrag) return;

        if (!(dragInfo.pos.y > (float)Screen.height * 0.9f) && !(dragInfo.pos.y < (float)Screen.height * 0.35f))
        {
            dragged = true;
            if (dragInfo.type != 2 && !dfDragFlag)
            {
                orbitSpeedY = -dragInfo.delta.x * rotYSpeedModifier;  // Horizontal drag
                orbitSpeedX = 0f;  // Disable vertical rotation
            }
        }
    }
}
