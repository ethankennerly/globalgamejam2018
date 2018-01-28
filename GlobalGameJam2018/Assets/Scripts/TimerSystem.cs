using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystem : System<TimerSystem>
    {
        public static event Action onStartTimer;

        public Animator animator { get; set; }
        public string[] sceneNames { get; set; }

        private int m_SceneIndex = 0;

        private bool m_IsGameOver = false;

        private bool m_IsVerbose = false;

        public TimerSystem()
        {
            StartButton.onClick += StartTimer;
            RestartButton.onClick += NextScene;
            ReplicationSystem.onAllDied += EndTimer;
            ClickPointSystem.disabledDuration = 1f;
            m_IsGameOver = false;
            SetGamePlaying(false);
        }

        ~TimerSystem()
        {
            StartButton.onClick -= StartTimer;
            RestartButton.onClick += NextScene;
            ReplicationSystem.onAllDied -= EndTimer;
        }

        // Guards if game over then restart.
        // Otherwise, a race may register a tap
        // if start button is accidentally attached to the end screen.
        private void StartTimer(StartButton button)
        {
            if (m_IsGameOver)
            {
                NextScene();
            }
            if (m_IsVerbose)
            {
                Debug.Log("TimerSystems.TimerSystems.StartTimer");
            }
            if (animator != null)
            {
                animator.Play("begin");
            }
            SetGamePlaying(true);
            if (onStartTimer == null)
            {
                return;
            }
            onStartTimer();
        }

        private void EndTimer()
        {
            if (m_IsGameOver)
            {
                return;
            }
            if (m_IsVerbose)
            {
                Debug.Log("TimerSystems.EndTimer");
            }
            m_IsGameOver = true;
            SetGamePlaying(false);
            if (animator == null)
            {
                return;
            }
            animator.Play("end");
        }

        private void NextScene(RestartButton button = null)
        {
            if (m_IsVerbose)
            {
                Debug.Log("TimerSystems.NextScene");
            }
            m_IsGameOver = false;
            SetGamePlaying(false);
            ++m_SceneIndex;
            if (m_SceneIndex >= sceneNames.Length)
            {
                m_SceneIndex = 0;
            }
            SceneManager.LoadScene(sceneNames[m_SceneIndex]);
            m_IsGameOver = false;
            SetGamePlaying(false);
        }

        private void SetGamePlaying(bool isGamePlaying)
        {
            DeltaTimeSystem.paused = !isGamePlaying;
        }
    }
}
