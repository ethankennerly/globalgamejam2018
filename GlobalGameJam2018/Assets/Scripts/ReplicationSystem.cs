using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public class ReplicationSystem : System<ReplicationSystem>
    {
        private readonly List<Virus> m_Viruses = new List<Virus>();

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
            m_Viruses.Remove(virus);
        }

        private void Update(float deltaTime)
        {
            foreach (Virus virus in m_Viruses)
            {
                if (virus.host == null)
                {
                    continue;
                }
                virus.timeRemaining -= deltaTime;
                if (virus.timeRemaining <= 0f)
                {
                    virus.count.value ++;
                    virus.timeRemaining += virus.incrementPeriod;
                }
            }
        }
    }
}
