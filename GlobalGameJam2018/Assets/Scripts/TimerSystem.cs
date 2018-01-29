using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Finegamedesign.Virus
{
    public sealed class TimerSystem
    {
        public static event Action<string> onSceneChanged;
        public static event Action onTimerStarted;
        public static event Action onTimerEnded;

        private string[] m_SceneNames;

        private int m_SceneIndex = 0;

        private bool m_IsGameOver = false;

        private bool m_IsVerbose = false;

        public TimerSystem()
        {
            StartButton.onClick += StartTimer;
            RestartButton.onClick += NextScene;
            ReplicationSystem.onAllDied += EndTimer;
            TimerSystemView.onSceneNamesEnabled += SetSceneNames;
            ClickPointSystem.disabledDuration = 1f;
            m_IsGameOver = false;
            SetGamePlaying(false);
        }

        ~TimerSystem()
        {
            StartButton.onClick -= StartTimer;
            RestartButton.onClick += NextScene;
            ReplicationSystem.onAllDied -= EndTimer;
            TimerSystemView.onSceneNamesEnabled += SetSceneNames;
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
            SetGamePlaying(true);
            if (onTimerStarted == null)
            {
                return;
            }
            onTimerStarted();
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
            if (onTimerEnded == null)
            {
                return;
            }
            onTimerEnded();
        }

        private void SetGamePlaying(bool isGamePlaying)
        {
            DeltaTimeSystem.paused = !isGamePlaying;
        }

        private void NextScene(RestartButton button = null)
        {
            if (m_IsVerbose)
            {
                Debug.Log("TimerSystems.NextScene");
            }
            m_IsGameOver = false;
            SetGamePlaying(false);
            if (m_SceneNames == null || m_SceneNames.Length == 0)
            {
                LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            ++m_SceneIndex;
            if (m_SceneIndex >= m_SceneNames.Length)
            {
                m_SceneIndex = 0;
            }
            string sceneName = m_SceneNames[m_SceneIndex];
            LoadScene(sceneName);
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SetSceneName(sceneName);
        }

        private void SetSceneNames(string[] sceneNames)
        {
            m_SceneNames = sceneNames;
            UpdateSceneName();
        }

        private void UpdateSceneName()
        {
            SetSceneName(SceneManager.GetActiveScene().name);
        }

        private void SetSceneName(string sceneName)
        {
            if (onSceneChanged == null)
            {
                return;
            }
            onSceneChanged(sceneName);
        }
    }
}
