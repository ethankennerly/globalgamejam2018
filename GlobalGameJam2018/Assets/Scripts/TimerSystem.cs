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

        private bool m_IsGameOver = false;

        private bool m_IsVerbose = true;

        public TimerSystem()
        {
            StartButton.onClick += StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied += EndTimer;
            ClickPointSystem.disabledDuration = 1f;
            m_IsGameOver = false;
            SetGamePlaying(false);
        }

        ~TimerSystem()
        {
            StartButton.onClick -= StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied -= EndTimer;
        }

        // Guards if game over then restart.
        // Otherwise, an animation race may register a tap on start button from end screen.
        private void StartTimer(StartButton button)
        {
            if (m_IsGameOver)
            {
                Restart();
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

        private void Restart(RestartButton button = null)
        {
            if (m_IsVerbose)
            {
                Debug.Log("TimerSystems.Restart");
            }
            m_IsGameOver = false;
            SetGamePlaying(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            m_IsGameOver = false;
            SetGamePlaying(false);
        }

        private void SetGamePlaying(bool isGamePlaying)
        {
            DeltaTimeSystem.paused = !isGamePlaying;
        }
    }
}
