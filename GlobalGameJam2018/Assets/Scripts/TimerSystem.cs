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

        private EndScreen m_EndScreen;

        private bool isGameOver = false;

        public TimerSystem()
        {
            StartButton.onClick += StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied += EndTimer;
            EndScreen.onEnable += SetEndScreen;
        }

        ~TimerSystem()
        {
            StartButton.onClick -= StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied -= EndTimer;
            EndScreen.onEnable -= SetEndScreen;
        }

        public void SetEndScreen(EndScreen endScreen)
        {
            m_EndScreen = endScreen;
            endScreen.gameObject.SetActive(isGameOver);
        }

        private void StartTimer(StartButton button)
        {
            button.gameObject.SetActive(false);
            m_EndScreen.gameObject.SetActive(false);
            if (onStartTimer == null)
            {
                return;
            }
            onStartTimer();
        }

        private void EndTimer()
        {
            if (m_EndScreen == null)
            {
                return;
            }
            isGameOver = true;
            m_EndScreen.gameObject.SetActive(true);
        }

        private void Restart(RestartButton button)
        {
            Debug.Log("Restart");
            isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
