using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class VirusLink : MonoBehaviour
    {
        public Virus virus;

        private void Update()
        {
            SetPosition(virus, transform);
        }

        private static void SetPosition(Component follower, Transform target)
        {
            if (follower == null
                || follower.transform == null
                || target == null)
            {
                return;
            }
            follower.transform.position = target.position;
        }
    }
}
