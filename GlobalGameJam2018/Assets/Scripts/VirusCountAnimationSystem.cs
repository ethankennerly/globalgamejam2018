using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public class VirusCountAnimationSystem : System<VirusCountAnimationSystem>
    {
        public VirusCountAnimationSystem()
        {
            Virus.onCountChanged += AnimateCount;
        }

        ~VirusCountAnimationSystem()
        {
            Virus.onCountChanged -= AnimateCount;
        }

        // When updating animation duration, also update duration in virus data.
        // To auto-sync animation duration, could sync speed to timer.
        // Or resync partial progress every frame.
        //
        // TODO: Synchronize host animation if any.
        private void AnimateCount(Virus virus, int previousCount, int currentCount)
        {
            if (virus.animator == null)
            {
                return;
            }
            string state = "count_" + currentCount;
            virus.animator.Play(state, -1, 0f);
        }
    }
}
