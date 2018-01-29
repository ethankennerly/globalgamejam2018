using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class TransmissionSystem : System<TransmissionSystem>
    {
        public TransmissionSystem()
        {
            Virus.onTrigger += TryTransmit;
            ClickSpawnSystem.onSpawnAtCollider += TryTransmitObject;
        }

        ~TransmissionSystem()
        {
            Virus.onTrigger -= TryTransmit;
            ClickSpawnSystem.onSpawnAtCollider -= TryTransmitObject;
        }

        private static void TryTransmitObject(GameObject cloneObject, Collider2D potentialHost)
        {
            Virus virus = cloneObject.GetComponent<Virus>();
            TransmissionSystem.TryTransmit(virus, potentialHost);
        }

        // Only subtracts from original virus when branching.
        private static void TryTransmit(Virus virus, Collider2D potentialHost)
        {
            if (virus == null)
            {
                return;
            }
            if (virus.host != null && virus.count < 2)
            {
                return;
            }
            MobileTile mobile = potentialHost.gameObject.GetComponent<MobileTile>();
            if (mobile == null)
            {
                return;
            }
            Virus potentialHostVirus = potentialHost.gameObject.GetComponentInChildren<Virus>();
            if (potentialHostVirus == null)
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
                cloneObject.transform.SetParent(potentialHost.gameObject.transform);
                cloneObject.transform.localPosition = new Vector3(0f, 0f, -0.01f);
                return;
            }
            if (virus.count == potentialHostVirus.count)
            {
                return;
            }
            if (virus.count < potentialHostVirus.count)
            {
                Virus swap = virus;
                virus = potentialHostVirus;
                potentialHostVirus = swap;
            }
            virus.count--;
            potentialHostVirus.count++;
        }
    }
}
