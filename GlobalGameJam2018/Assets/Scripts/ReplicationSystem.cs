using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class ReplicationSystem : System<ReplicationSystem>
    {
        public static event Action onAllDied;

        private readonly List<Virus> m_Viruses = new List<Virus>();
        private readonly List<Virus> m_Removing = new List<Virus>();

        private int m_MaxViruses = 0;

        public ReplicationSystem()
        {
            Virus.onEnable += OnEnable;
            Virus.onDisable += OnDisable;
            DeltaTimeSystem.onDeltaTime += Update;
            TimerSystem.onStartTimer += Reset;
        }

        ~ReplicationSystem()
        {
            Virus.onEnable -= OnEnable;
            Virus.onDisable -= OnDisable;
            DeltaTimeSystem.onDeltaTime -= Update;
            TimerSystem.onStartTimer -= Reset;
        }

        private void Reset()
        {
            m_MaxViruses = 0;
        }

        private void OnEnable(Virus virus)
        {
            if (m_Viruses.Contains(virus))
            {
                return;
            }
            m_Viruses.Add(virus);
        }

        private void OnDisable(Virus virus)
        {
            if (m_Removing.Contains(virus))
            {
                return;
            }
            m_Removing.Add(virus);
        }

        private void Update(float deltaTime)
        {
            RemoveDead();
            foreach (Virus virus in m_Viruses)
            {
                if (virus == null || virus.host == null || virus.isDead)
                {
                    continue;
                }
                virus.timeRemaining -= deltaTime;
                if (virus.timeRemaining <= 0f)
                {
                    virus.count++;
                    virus.timeRemaining += virus.incrementPeriod;
                }
            }
        }

        private void RemoveDead()
        {
            foreach (Virus virus in m_Viruses)
            {
                if (virus == null || !virus.isDead)
                {
                    continue;
                }
                if (m_Removing.Contains(virus))
                {
                    continue;
                }
                m_Removing.Add(virus);
            }
            int previousViruses = m_Viruses.Count;
            foreach (Virus virus in m_Removing)
            {
                m_Viruses.Remove(virus);
            }
            m_Removing.Clear();
            int numViruses = m_Viruses.Count;
            if (numViruses > m_MaxViruses)
            {
                m_MaxViruses = numViruses;
            }
            else if (m_MaxViruses > 0 && numViruses == 0)
            {
                if (onAllDied != null)
                {
                    onAllDied();
                }
                Reset();
            }
        }
    }
}
