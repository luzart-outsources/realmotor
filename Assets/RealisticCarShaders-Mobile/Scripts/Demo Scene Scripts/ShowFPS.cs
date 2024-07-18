//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2022 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//
using UnityEngine;
using System.Collections;

namespace SkrilStudio
{
    public class ShowFPS : MonoBehaviour
    {
        float deltaTime = 0.0f;
        // set text anchor
        public enum FPSTextPosition { UpperLeft, UpperRight, BottomLeft, BottomRight }
        public FPSTextPosition textAnchor = new FPSTextPosition();
        // set text color
        public enum FPSColor { Black, White, Red, Blue, Yellow }
        public FPSColor fpsTextColor = new FPSColor();
        // text sizze
        public bool autoFontSize = true;
        [Header("Font Size is ignored if Auto Font Size is enabled.")]
        public int fontSize = 20;
        // set text color
        public enum FPSType { All, FPS, Millisec }
        public FPSType fpsType = new FPSType();

        void LateUpdate()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h);
            // anchor upper left
            if (textAnchor == FPSTextPosition.UpperLeft)
                style.alignment = TextAnchor.UpperLeft;
            // anchor upper right
            if (textAnchor == FPSTextPosition.UpperRight)
                style.alignment = TextAnchor.UpperRight;
            // anchor bottom left
            if (textAnchor == FPSTextPosition.BottomLeft)
                style.alignment = TextAnchor.LowerLeft;
            // anchor bottom left
            if (textAnchor == FPSTextPosition.BottomRight)
                style.alignment = TextAnchor.LowerRight;
            // font size
            if (autoFontSize)
            {
                style.fontSize = h * 2 / 60;
            }
            else
            {
                style.fontSize = fontSize;
            }
            // text color
            if (fpsTextColor == FPSColor.Black)
                style.normal.textColor = Color.black;
            if (fpsTextColor == FPSColor.White)
                style.normal.textColor = Color.white;
            if (fpsTextColor == FPSColor.Red)
                style.normal.textColor = Color.red;
            if (fpsTextColor == FPSColor.Blue)
                style.normal.textColor = Color.blue;
            if (fpsTextColor == FPSColor.Yellow)
                style.normal.textColor = Color.yellow;
            //
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            // text format
            if (fpsType == FPSType.All)
            {
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(rect, text, style);
            }
            if (fpsType == FPSType.FPS)
            {
                string text = string.Format("{1:0.} fps", msec, fps);
                GUI.Label(rect, text, style);
            }
            if (fpsType == FPSType.Millisec)
            {
                string text = string.Format("{0:0.0} ms", msec, fps);
                GUI.Label(rect, text, style);
            }
        }
    }
}