using Finegamedesign.Tiles;
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
                Debug.Log("Fatal but no host");
                return true;
            }
            MobileTile mobile = hostObject.GetComponent<MobileTile>();
            if (mobile != null)
            {
                mobile.velocity = new Vector2();
            }
            Rigidbody2D body = hostObject.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                GameObject.Destroy(body);
            }
            Collider2D collider = hostObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                GameObject.Destroy(collider);
            }
            if (!virus.isDead)
            {
                Debug.Log("Not dead yet");
                return false;
            }
            Debug.Log("Destroy host and virus.");
            // GameObject.Destroy(hostObject);
            // GameObject.Destroy(virus.gameObject);
            // virus.host = null;
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
