using Finegamedesign.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystemView : SystemView<TimerSystem>
    {
        public static event Action<string[]> onSceneNamesEnabled;

        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private string[] m_SceneNames;

        [SerializeField]
        private Text m_SceneNameText;

        private void OnEnable()
        {
            TimerSystem.onSceneChanged += SetSceneName;
            TimerSystem.onTimerStarted += AnimateStartLevel;
            TimerSystem.onTimerEnded += AnimateEndLevel;
            if (onSceneNamesEnabled == null)
            {
                return;
            }
            onSceneNamesEnabled(m_SceneNames);
        }

        private void OnDisable()
        {
            TimerSystem.onSceneChanged -= SetSceneName;
            TimerSystem.onTimerStarted -= AnimateStartLevel;
            TimerSystem.onTimerEnded -= AnimateEndLevel;
        }

        private void SetSceneName(string sceneName)
        {
            if (m_SceneNameText == null)
            {
                return;
            }
            m_SceneNameText.text = sceneName.Replace("_", " ");
        }

        private void AnimateStartLevel()
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.Play("begin");
        }

        private void AnimateEndLevel()
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.Play("end");
        }
    }
}
