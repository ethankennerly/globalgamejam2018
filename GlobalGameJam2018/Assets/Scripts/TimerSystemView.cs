using Finegamedesign.Utils;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystemView : SystemView<TimerSystem>
    {
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private string[] m_SceneNames;

        private void OnEnable()
        {
            System.animator = m_Animator;
            System.sceneNames = m_SceneNames;
        }
    }
}
