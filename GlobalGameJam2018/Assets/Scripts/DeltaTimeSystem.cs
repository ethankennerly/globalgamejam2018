using System;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class DeltaTimeSystem : MonoBehaviour
    {
        public static event Action<float> OnDeltaTime;

        public void Update()
        {
            if (OnDeltaTime == null)
            {
                return;
            }
            OnDeltaTime(Time.deltaTime);
        }
    }
}
