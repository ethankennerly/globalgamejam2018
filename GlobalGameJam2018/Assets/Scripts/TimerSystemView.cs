using Finegamedesign.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystemView : SystemView<TimerSystem>
    {
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private string[] m_SceneNames;
        [SerializeField]
        private Text m_SceneNameText;

        private void OnEnable()
        {
            TimerSystem.onSceneChanged += SetSceneName;
            System.animator = m_Animator;
            System.sceneNames = m_SceneNames;
            System.UpdateSceneName();
        }

        private void OnDisable()
        {
            TimerSystem.onSceneChanged -= SetSceneName;
        }

        private void SetSceneName(string sceneName)
        {
            if (m_SceneNameText == null)
            {
                return;
            }
            m_SceneNameText.text = sceneName.Replace("_", " ");
        }
    }
}
