using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    [Serializable]
    public class TransmissionSystem : System<TransmissionSystem>
    {
        public TransmissionSystem()
        {
            Virus.onTrigger += TryTransmit;
        }

        ~TransmissionSystem()
        {
            Virus.onTrigger -= TryTransmit;
        }

        private void TryTransmit(Virus virus, Collider2D other)
        {
            if (virus.host != null && virus.count.value < 1)
            {
                return;
            }
            MobileTile mobile = other.gameObject.GetComponent<MobileTile>();
            if (mobile == null)
            {
                return;
            }
            Virus otherVirus = other.gameObject.GetComponentInChildren<Virus>();
            if (otherVirus == null)
            {
                if (virus.count.value > 0)
                {
                    return;
                }
                virus.host = mobile;
                virus.gameObject.transform.SetParent(other.gameObject.transform);
                virus.gameObject.transform.localPosition = Vector3.zero;
                return;
            }
            if (virus.count.value == otherVirus.count.value)
            {
                return;
            }
            if (virus.count.value < otherVirus.count.value)
            {
                Virus swap = virus;
                virus = otherVirus;
                otherVirus = swap;
            }
            virus.count.value--;
            otherVirus.count.value++;
            return;
        }
    }
}
