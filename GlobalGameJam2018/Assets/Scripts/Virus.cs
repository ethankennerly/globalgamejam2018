using System;
using Finegamedesign.Utils;
using Finegamedesign.Tiles;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class Virus : AEnableBehaviour<Virus>
    {
        public static event Action<Virus, Collider2D> onTrigger;
        public static event Action<Virus, int, int> onCountChanged;

        private int m_Count = 0;

        public int count
        {
            get
            {
                return m_Count;
            }
            set
            {
                if (m_Count == value)
                {
                    return;
                }
                int previousCount = m_Count;
                m_Count = value;
                if (onCountChanged == null)
                {
                    return;
                }
                onCountChanged(this, previousCount, value);
            }
        }

        public float incrementPeriod = 1f;
        public float timeRemaining = 1f;
        public MobileTile host;
        public Animator animator;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (onTrigger != null)
            {
                onTrigger(this, other);
            }
        }
    }
}
