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
        //
        // Virus link follows the host.
        // Does not add virus as child, because Unity merges child triggers with parent.
        // https://answers.unity.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html
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
            VirusLink potentialHostVirusLink = potentialHost.gameObject.GetComponent<VirusLink>();
            if (potentialHostVirusLink == null)
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
                potentialHost.gameObject.AddComponent<VirusLink>();
                potentialHostVirusLink = potentialHost.gameObject.GetComponent<VirusLink>();
                potentialHostVirusLink.virus = clone;
                cloneObject.transform.position = potentialHost.transform.position;
                return;
            }
            Virus potentialHostVirus = potentialHostVirusLink.virus;
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
