using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Finegamedesign.Utils
{
    public static class ClickPointSystem
    {
        public static event Action<Vector3> onWorld;
        public static event Action<float, float> onWorldXY;
        public static event Action<float, float> onViewportXY;
        public static event Action<float, float> onAxisXY;

        public static event Action<Vector3> onCollisionEnter;
        public static event Action<Vector2> onCollisionEnter2D;

        private static float s_DisabledDuration = 0.25f;
        public static float disabledDuration
        {
            get { return s_DisabledDuration; }
            set { s_DisabledDuration = value; }
        }
        private static float s_UpdateTime = -1.0f;
        private static float s_ClickTime = -1.0f;

        private static Vector2 s_OverlapPoint = new Vector2();

        private static Vector3 s_RaycastHit = new Vector3();

        private static Vector3 s_Click = new Vector3();
        private static Vector3 s_Viewport = new Vector3();
        private static Vector2 s_Axis = new Vector2();

        private static bool s_IsVerbose = true;

        // Caches time to ignore multiple calls per frame.
        //
        // Ignores if over UI object.
        // Otherwise, a tap on a button is also reacted as a tap in viewport.
        // For example, in Deadly Diver, a viewport tap moves the diver.
        public static void Update()
        {
            float time = Time.time;
            if (s_UpdateTime == time)
            {
                return;
            }
            s_UpdateTime = time;
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Raycast();
            OverlapPoint();
            Screen();
            Viewport();
        }

        // Returns if currently enabled, else already disabled.
        // Because of click-point update racing between other updates,
        // each caller disables individually.
        public static bool DisableTemporarily()
        {
            bool enabled = s_ClickTime < 0f;
            float time = Time.time;
            if (!enabled)
            {
                float enabledTime = s_ClickTime + s_DisabledDuration;
                enabled = time >= enabledTime;
            }
            if (enabled)
            {
                s_ClickTime = time;
            }
            if (s_IsVerbose)
            {
                Debug.Log("ClickPointSystem.enabled: " + enabled
                    + " since " + s_ClickTime + " for " + s_DisabledDuration);
            }
            return enabled;
        }

        private static bool Raycast()
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                return false;
            }
            s_RaycastHit = hit.point;
            if (onCollisionEnter != null)
            {
                onCollisionEnter(s_RaycastHit);
            }
            if (s_IsVerbose)
            {
                Debug.Log("ClickPoint.RayCast: " + s_RaycastHit);
            }
            return true;
        }

        private static bool OverlapPoint()
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            s_OverlapPoint.x = point.x;
            s_OverlapPoint.y = point.y;
            if (Physics2D.OverlapPoint(s_OverlapPoint) == null)
            {
                return false;
            }
            if (onCollisionEnter2D != null)
            {
                onCollisionEnter2D(s_OverlapPoint);
            }
            if (s_IsVerbose)
            {
                Debug.Log("ClickPoint.OverlapPoint: " + s_OverlapPoint);
            }
            return true;
        }

        private static bool Viewport()
        {
            s_Viewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (onViewportXY != null)
            {
                onViewportXY(s_Viewport.x, s_Viewport.y);
            }
            if (onAxisXY != null)
            {
                s_Axis.x = s_Viewport.x - 0.5f;
                s_Axis.y = s_Viewport.y - 0.5f;
                s_Axis.Normalize();
                onAxisXY(s_Axis.x, s_Axis.y);
            }
            if (s_IsVerbose)
            {
                Debug.Log("ClickPoint.Viewport: " + s_Viewport);
            }
            return true;
        }

        private static bool Screen()
        {
            s_Click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (onWorld != null)
            {
                onWorld(s_Click);
            }
            if (onWorldXY != null)
            {
                onWorldXY(s_Click.x, s_Click.y);
            }
            if (s_IsVerbose)
            {
                Debug.Log("ClickPoint.Screen: " + s_Click);
            }
            return true;
        }
    }
}
