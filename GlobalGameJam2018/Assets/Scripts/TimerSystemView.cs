using Finegamedesign.Utils;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystemView : SystemView<TimerSystem>
    {
        [SerializeField]
        private Animator m_Animator;

        private void OnEnable()
        {
            System.animator = m_Animator;
        }
    }
}
