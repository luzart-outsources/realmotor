using UnityEngine;

public class DragInfo
{
	public int type = -1;

	public Vector2 pos;

	public Vector2 delta;

	public DragInfo(int t, Vector2 p, Vector2 dir)
	{
		type = t;
		pos = p;
		delta = dir;
	}
}
