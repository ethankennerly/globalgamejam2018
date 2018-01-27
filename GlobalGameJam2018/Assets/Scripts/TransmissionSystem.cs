using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
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
                virus.count.value--;
                GameObject cloneObject;
                Virus clone;
                if (virus.count.value > 0)
                {
                    cloneObject = GameObject.Instantiate(virus.gameObject);
                    clone = cloneObject.GetComponent<Virus>();
                }
                else
                {
                    cloneObject = virus.gameObject;
                    clone = virus;
                }
                clone.host = mobile;
                cloneObject.transform.SetParent(other.gameObject.transform);
                cloneObject.transform.localPosition = Vector3.zero;
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
        }
    }
}
