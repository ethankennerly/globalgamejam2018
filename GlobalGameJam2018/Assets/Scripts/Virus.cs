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

        private int m_Count = 1;
        private int m_Min = 0;
        private int m_Max = 4;

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
                value = Mathf.Clamp(value, m_Min, m_Max);
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

        private void Start()
        {
            onCountChanged(this, 0, m_Count);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (onTrigger != null)
            {
                onTrigger(this, other);
            }
        }
    }
}
