using System;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class DeltaTimeSystem : MonoBehaviour
    {
        public static event Action<float> onDeltaTime;

        public void Update()
        {
            if (onDeltaTime == null)
            {
                return;
            }
            onDeltaTime(Time.deltaTime);
        }
    }
}
