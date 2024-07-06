using UnityEngine;

public class SwipeInfo
{
	public Vector2 startPoint;

	public Vector2 endPoint;

	public Vector2 direction;

	public float angle;

	public float duration;

	public float speed;

	public SwipeInfo(Vector2 p1, Vector2 p2, Vector2 dir, float startT)
	{
		startPoint = p1;
		endPoint = p2;
		direction = dir;
		angle = Gesture.VectorToAngle(dir);
		duration = Time.time - startT;
		speed = dir.magnitude / duration;
	}
}
