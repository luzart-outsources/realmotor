using UnityEngine;

[RequireComponent(typeof(General))]
public class Gesture : MonoBehaviour
{
	public delegate void ShortTapHandler(Vector2 pos);

	public delegate void LongTapHandler(Vector2 pos);

	public delegate void DoubleTapHandler(Vector2 pos);

	public delegate void ChargingHandler(ChargedInfo cInfo);

	public delegate void ChargeEndHandler(ChargedInfo cInfo);

	public delegate void DFShortTapHandler(Vector2 pos);

	public delegate void DFLongTapHandler(Vector2 pos);

	public delegate void DFDoubleTapHandler(Vector2 pos);

	public delegate void DFChargingHandler(ChargedInfo cInfo);

	public delegate void DFChargeEndHandler(ChargedInfo cInfo);

	public delegate void DraggingHandler(DragInfo dragInfo);

	public delegate void DualFDragHandler(DragInfo dragInfo);

	public delegate void DraggingEndHandler(Vector2 pos);

	public delegate void DualFDraggingEndHandler(Vector2 pos);

	public delegate void SwipeHandler(SwipeInfo sw);

	public delegate void PinchHandler(float val);

	public delegate void RotateHandler(float val);

	public delegate void TouchDownHandler(Vector2 pos);

	public delegate void TouchUpHandler(Vector2 pos);

	public delegate void TouchHandler(Vector2 pos);

	public delegate void Mouse1DownHandler(Vector2 pos);

	public delegate void Mouse1UpHandler(Vector2 pos);

	public delegate void Mouse1Handler(Vector2 pos);

	public delegate void Mouse2DownHandler(Vector2 pos);

	public delegate void Mouse2UpHandler(Vector2 pos);

	public delegate void Mouse2Handler(Vector2 pos);

	public static Gesture gesture;

	public static event ShortTapHandler onShortTapE;

	public static event LongTapHandler onLongTapE;

	public static event DoubleTapHandler onDoubleTapE;

	public static event ChargingHandler onChargingE;

	public static event ChargeEndHandler onChargeEndE;

	public static event DFShortTapHandler onDFShortTapE;

	public static event DFLongTapHandler onDFLongTapE;

	public static event DFDoubleTapHandler onDFDoubleTapE;

	public static event DFChargingHandler onDFChargingE;

	public static event DFChargeEndHandler onDFChargeEndE;

	public static event DraggingHandler onDraggingE;

	public static event DualFDragHandler onDualFDraggingE;

	public static event DraggingEndHandler onDraggingEndE;

	public static event DualFDraggingEndHandler onDualFDraggingEndE;

	public static event SwipeHandler onSwipeE;

	public static event PinchHandler onPinchE;

	public static event RotateHandler onRotateE;

	public static event TouchDownHandler onTouchDownE;

	public static event TouchUpHandler onTouchUpE;

	public static event TouchHandler onTouchE;

	public static event Mouse1DownHandler onMouse1DownE;

	public static event Mouse1UpHandler onMouse1UpE;

	public static event Mouse1Handler onMouse1E;

	public static event Mouse2DownHandler onMouse2DownE;

	public static event Mouse2UpHandler onMouse2UpE;

	public static event Mouse2Handler onMouse2E;

	private void Awake()
	{
		gesture = this;
	}

	public static void ShortTap(Vector2 pos)
	{
		if (Gesture.onShortTapE != null)
		{
			Gesture.onShortTapE(pos);
		}
	}

	public static void LongTap(Vector2 pos)
	{
		if (Gesture.onLongTapE != null)
		{
			Gesture.onLongTapE(pos);
		}
	}

	public static void DoubleTap(Vector2 pos)
	{
		if (Gesture.onDoubleTapE != null)
		{
			Gesture.onDoubleTapE(pos);
		}
	}

	public static void Charging(ChargedInfo cInfo)
	{
		if (Gesture.onChargingE != null)
		{
			Gesture.onChargingE(cInfo);
		}
	}

	public static void ChargeEnd(ChargedInfo cInfo)
	{
		if (Gesture.onChargeEndE != null)
		{
			Gesture.onChargeEndE(cInfo);
		}
	}

	public static void DFShortTap(Vector2 pos)
	{
		if (Gesture.onDFShortTapE != null)
		{
			Gesture.onDFShortTapE(pos);
		}
	}

	public static void DFLongTap(Vector2 pos)
	{
		if (Gesture.onDFLongTapE != null)
		{
			Gesture.onDFLongTapE(pos);
		}
	}

	public static void DFDoubleTap(Vector2 pos)
	{
		if (Gesture.onDFDoubleTapE != null)
		{
			Gesture.onDFDoubleTapE(pos);
		}
	}

	public static void DFCharging(ChargedInfo cInfo)
	{
		if (Gesture.onDFChargingE != null)
		{
			Gesture.onDFChargingE(cInfo);
		}
	}

	public static void DFChargeEnd(ChargedInfo cInfo)
	{
		if (Gesture.onDFChargeEndE != null)
		{
			Gesture.onDFChargeEndE(cInfo);
		}
	}

	public static void Dragging(DragInfo dragInfo)
	{
		if (Gesture.onDraggingE != null)
		{
			Gesture.onDraggingE(dragInfo);
		}
	}

	public static void DualFingerDragging(DragInfo dragInfo)
	{
		if (Gesture.onDualFDraggingE != null)
		{
			Gesture.onDualFDraggingE(dragInfo);
		}
	}

	public static void DraggingEnd(Vector2 pos)
	{
		if (Gesture.onDraggingEndE != null)
		{
			Gesture.onDraggingEndE(pos);
		}
	}

	public static void DualFingerDraggingEnd(Vector2 pos)
	{
		if (Gesture.onDualFDraggingEndE != null)
		{
			Gesture.onDualFDraggingEndE(pos);
		}
	}

	public static void Swipe(SwipeInfo sw)
	{
		if (Gesture.onSwipeE != null)
		{
			Gesture.onSwipeE(sw);
		}
	}

	public static void Pinch(float val)
	{
		if (Gesture.onPinchE != null)
		{
			Gesture.onPinchE(val);
		}
	}

	public static void Rotate(float val)
	{
		if (Gesture.onRotateE != null)
		{
			Gesture.onRotateE(val);
		}
	}

	public static void OnTouchDown(Vector2 pos)
	{
		if (Gesture.onTouchDownE != null)
		{
			Gesture.onTouchDownE(pos);
		}
	}

	public static void OnTouchUp(Vector2 pos)
	{
		if (Gesture.onTouchUpE != null)
		{
			Gesture.onTouchUpE(pos);
		}
	}

	public static void OnTouch(Vector2 pos)
	{
		if (Gesture.onTouchE != null)
		{
			Gesture.onTouchE(pos);
		}
	}

	public static void OnMouse1Down(Vector2 pos)
	{
		if (Gesture.onMouse1DownE != null)
		{
			Gesture.onMouse1DownE(pos);
		}
	}

	public static void OnMouse1Up(Vector2 pos)
	{
		if (Gesture.onMouse1UpE != null)
		{
			Gesture.onMouse1UpE(pos);
		}
	}

	public static void OnMouse1(Vector2 pos)
	{
		if (Gesture.onMouse1E != null)
		{
			Gesture.onMouse1E(pos);
		}
	}

	public static void OnMouse2Down(Vector2 pos)
	{
		if (Gesture.onMouse2DownE != null)
		{
			Gesture.onMouse2DownE(pos);
		}
	}

	public static void OnMouse2Up(Vector2 pos)
	{
		if (Gesture.onMouse2UpE != null)
		{
			Gesture.onMouse2UpE(pos);
		}
	}

	public static void OnMouse2(Vector2 pos)
	{
		if (Gesture.onMouse2E != null)
		{
			Gesture.onMouse2E(pos);
		}
	}

	public static float VectorToAngle(Vector2 dir)
	{
		if (dir.x == 0f)
		{
			if (dir.y > 0f)
			{
				return 90f;
			}
			if (dir.y < 0f)
			{
				return 270f;
			}
			return 0f;
		}
		if (dir.y == 0f)
		{
			if (dir.x > 0f)
			{
				return 0f;
			}
			if (dir.x < 0f)
			{
				return 180f;
			}
			return 0f;
		}
		float num = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
		float num2 = Mathf.Asin(dir.y / num) * 57.29578f;
		if (dir.y > 0f)
		{
			if (dir.x < 0f)
			{
				num2 = 180f - num2;
			}
		}
		else
		{
			if (dir.x > 0f)
			{
				num2 = 360f + num2;
			}
			if (dir.x < 0f)
			{
				num2 = 180f - num2;
			}
		}
		return num2;
	}
}
