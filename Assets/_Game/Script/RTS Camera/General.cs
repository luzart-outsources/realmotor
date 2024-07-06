using UnityEngine;

public class General : MonoBehaviour
{
	private Vector2 lastPos;

	private bool dragging;

	private bool draggingInitiated;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount > 0)
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.phase == TouchPhase.Began)
				{
					Gesture.OnTouchDown(touch.position);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					Gesture.OnTouchUp(touch.position);
				}
				else
				{
					Gesture.OnTouch(touch.position);
				}
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			Gesture.OnMouse1Down(UnityEngine.Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Gesture.OnMouse1Up(UnityEngine.Input.mousePosition);
		}
		else if (Input.GetMouseButton(0))
		{
			Gesture.OnMouse1(UnityEngine.Input.mousePosition);
		}
		if (Input.GetMouseButtonDown(1))
		{
			Gesture.OnMouse2Down(UnityEngine.Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(1))
		{
			Gesture.OnMouse2Up(UnityEngine.Input.mousePosition);
		}
		else if (Input.GetMouseButton(1))
		{
			Gesture.OnMouse2(UnityEngine.Input.mousePosition);
		}
		InputEvent inputEvent = new InputEvent();
		if (UnityEngine.Input.touchCount == 1)
		{
			Touch touch2 = Input.touches[0];
			inputEvent = new InputEvent(state: (touch2.phase == TouchPhase.Began) ? _InputState.Down : ((touch2.phase == TouchPhase.Ended) ? _InputState.Up : _InputState.On), p: touch2.position, type: _InputType.Touch);
		}
		else if (UnityEngine.Input.touchCount == 0)
		{
			if (Input.GetMouseButtonDown(0))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse1, _InputState.Down);
			}
			else if (Input.GetMouseButton(0))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse1, _InputState.On);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse1, _InputState.Up);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse2, _InputState.Down);
			}
			else if (Input.GetMouseButton(1))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse2, _InputState.On);
			}
			else if (Input.GetMouseButtonUp(1))
			{
				inputEvent = new InputEvent(UnityEngine.Input.mousePosition, _InputType.Mouse2, _InputState.Up);
			}
		}
		if (inputEvent.inputType != 0)
		{
			if (inputEvent.inputState == _InputState.Down)
			{
				lastPos = inputEvent.pos;
				draggingInitiated = true;
			}
			if (inputEvent.inputState == _InputState.On)
			{
				Vector2 pos = inputEvent.pos;
				if (!draggingInitiated)
				{
					draggingInitiated = true;
				}
				else
				{
					Vector2 dir = pos - lastPos;
					if (Mathf.Abs(dir.magnitude) > 0f)
					{
						dragging = true;
						int t = 0;
						if (inputEvent.inputType == _InputType.Mouse1)
						{
							t = 1;
						}
						else if (inputEvent.inputType == _InputType.Mouse2)
						{
							t = 2;
						}
						DragInfo dragInfo = new DragInfo(t, pos, dir);
						Gesture.Dragging(dragInfo);
					}
					else
					{
						draggingInitiated = false;
					}
				}
				lastPos = inputEvent.pos;
			}
			if (inputEvent.inputState == _InputState.Up)
			{
				if (dragging)
				{
					dragging = false;
					Gesture.DraggingEnd(inputEvent.pos);
				}
				if (draggingInitiated)
				{
					draggingInitiated = false;
				}
			}
		}
		else
		{
			if (dragging)
			{
				dragging = false;
				Gesture.DraggingEnd(inputEvent.pos);
			}
			if (draggingInitiated)
			{
				draggingInitiated = false;
			}
		}
	}
}
