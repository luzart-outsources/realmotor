using UnityEngine;

public class ChargedInfo
{
	public float percent;

	public Vector2 pos;

	public Vector2 pos1;

	public Vector2 pos2;

	public ChargedInfo(Vector2 p, float val)
	{
		pos = p;
		percent = val;
	}

	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2)
	{
		pos = p;
		percent = val;
		pos1 = p1;
		pos2 = p2;
	}
}
