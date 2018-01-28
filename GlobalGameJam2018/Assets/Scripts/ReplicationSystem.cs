using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public class ReplicationSystem : System<ReplicationSystem>
    {
        private readonly List<Virus> m_Viruses = new List<Virus>();
        private readonly List<Virus> m_Removing = new List<Virus>();

        public ReplicationSystem()
        {
            Virus.onEnable += OnEnable;
            Virus.onDisable += OnDisable;
            DeltaTimeSystem.onDeltaTime += Update;
        }

        ~ReplicationSystem()
        {
            Virus.onEnable -= OnEnable;
            Virus.onDisable -= OnDisable;
            DeltaTimeSystem.onDeltaTime -= Update;
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
            foreach (Virus virus in m_Removing)
            {
                m_Viruses.Remove(virus);
            }
            m_Removing.Clear();
            foreach (Virus virus in m_Viruses)
            {
                if (!virus || virus == null || virus.host == null || virus.isDead)
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
    }
}
