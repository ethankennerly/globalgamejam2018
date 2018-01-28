using System;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class DeltaTimeSystem : MonoBehaviour
    {
        public static event Action<float> onDeltaTime;

        private static bool s_Paused;

        public static bool paused
        {
            get { return s_Paused; }
            set { s_Paused = value; }
        }

        public void Update()
        {
            if (onDeltaTime == null)
            {
                return;
            }
            float time = s_Paused ? 0f : Time.deltaTime;
            onDeltaTime(time);
        }
    }
}
