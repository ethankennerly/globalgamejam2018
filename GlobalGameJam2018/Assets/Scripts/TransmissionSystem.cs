using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class TransmissionSystem
    {
        public TransmissionSystem()
        {
            Virus.onTrigger += TryTransmit;
        }

        ~TransmissionSystem()
        {
            Virus.onTrigger -= TryTransmit;
        }

        // Only subtracts from original virus when branching.
        private void TryTransmit(Virus virus, Collider2D other)
        {
            if (virus.host != null && virus.count < 2)
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
                GameObject cloneObject;
                Virus clone;
                if (virus.count > 1)
                {
                    virus.count--;
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
                cloneObject.transform.localPosition = new Vector3(0f, 0f, -0.01f);
                return;
            }
            if (virus.count == otherVirus.count)
            {
                return;
            }
            if (virus.count < otherVirus.count)
            {
                Virus swap = virus;
                virus = otherVirus;
                otherVirus = swap;
            }
            virus.count--;
            otherVirus.count++;
        }
    }
}
