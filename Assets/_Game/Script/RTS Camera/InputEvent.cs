using UnityEngine;

public class InputEvent
{
	public _InputType inputType;

	public _InputState inputState;

	public Vector2 pos = new Vector3(-999f, -999f);

	public InputEvent()
	{
	}

	public InputEvent(Vector3 p, _InputType type, _InputState state)
	{
		pos = p;
		inputType = type;
		inputState = state;
	}
}
