using Finegamedesign.Utils;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystemView : SystemView<TimerSystem>
    {
        [SerializeField]
        private EndScreen m_EndScreen;

        private void OnEnable()
        {
            System.SetEndScreen(m_EndScreen);
        }
    }
}
