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

        public TimerSystem()
        {
            StartButton.onClick += StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied += EndTimer;
            ClickPointSystem.disabledDuration = 1f;
        }

        ~TimerSystem()
        {
            StartButton.onClick -= StartTimer;
            RestartButton.onClick += Restart;
            ReplicationSystem.onAllDied -= EndTimer;
        }

        private void StartTimer(StartButton button)
        {
            if (animator != null)
            {
                animator.Play("begin");
            }
            if (onStartTimer == null)
            {
                return;
            }
            onStartTimer();
        }

        private void EndTimer()
        {
            if (animator == null)
            {
                return;
            }
            animator.Play("end");
        }

        private void Restart(RestartButton button)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
