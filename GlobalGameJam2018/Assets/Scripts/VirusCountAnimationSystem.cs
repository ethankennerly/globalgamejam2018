using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public class VirusCountAnimationSystem : System<VirusCountAnimationSystem>
    {
        public static event Action onFatal;

        private readonly HashSet<MobileTile> m_Fatalities = new HashSet<MobileTile>();

        public VirusCountAnimationSystem()
        {
            Virus.onCountChanged += OnCountChanged;
        }

        ~VirusCountAnimationSystem()
        {
            Virus.onCountChanged -= OnCountChanged;
        }

        private void OnCountChanged(Virus virus, int previousCount, int currentCount)
        {
            if (DestroyIfFatal(virus))
            {
                return;
            }
            AnimateCount(virus.animator, currentCount);
        }

        private bool DestroyIfFatal(Virus virus)
        {
            if (!virus.isFatal)
            {
                return false;
            }
            GameObject hostObject = virus.host.gameObject;
            if (hostObject == null)
            {
                return true;
            }
            MobileTile mobile = hostObject.GetComponent<MobileTile>();
            if (mobile != null)
            {
                mobile.velocity = new Vector2();
            }
            if (!virus.isDead)
            {
                if (m_Fatalities.Contains(mobile))
                {
                    return false;
                }
                m_Fatalities.Add(mobile);
                if (onFatal != null)
                {
                    onFatal();
                }
                return false;
            }
            hostObject.SetActive(false);
            virus.gameObject.SetActive(false);
            return true;
        }

        // When updating animation duration, also update duration in virus data.
        // To auto-sync animation duration, could sync speed to timer.
        // Or resync partial progress every frame.
        //
        // TODO: Synchronize host animation if any.
        private void AnimateCount(Animator animator, int currentCount)
        {
            if (animator == null)
            {
                return;
            }
            string state = "count_" + currentCount;
            animator.Play(state, -1, 0f);
        }
    }
}
