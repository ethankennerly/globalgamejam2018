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
            if (virus.isFatal)
            {
                return;
            }
            if (virus.host != null && virus.count < 1)
            {
                return;
            }
            MobileTile mobile = other.gameObject.GetComponent<MobileTile>();
            if (mobile == null)
            {
                return;
            }
            Virus otherVirus = other.gameObject.GetComponentInChildren<Virus>();
            if (otherVirus != null && otherVirus.isFatal)
            {
                return;
            }
            if (otherVirus == null)
            {
                virus.count--;
                GameObject cloneObject;
                Virus clone;
                if (virus.count > 0)
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
