using Finegamedesign.Utils;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class VirusCountAnimationSystemView : SystemView<VirusCountAnimationSystem>
    {
        [SerializeField]
        private AudioArray m_AudioArray;

        private void OnEnable()
        {
            if (m_AudioArray == null)
            {
                return;
            }
            VirusCountAnimationSystem.onFatal += m_AudioArray.PlayNext;
        }

        private void OnDisable()
        {
            if (m_AudioArray == null)
            {
                return;
            }
            VirusCountAnimationSystem.onFatal -= m_AudioArray.PlayNext;
        }
    }
}
