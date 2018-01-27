using System;
using Finegamedesign.Utils;
using Finegamedesign.Tiles;
using UnityEngine;

namespace Finegamedesign.Virus
{
    public sealed class Virus : AEnableBehaviour<Virus>
    {
        public static event Action<Virus, Collider2D> onTrigger;
        public Observable<int> count = new Observable<int>();
        public float incrementPeriod = 1f;
        public float timeRemaining = 1f;
        public MobileTile host;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (onTrigger != null)
            {
                onTrigger(this, other);
            }
        }
    }
}
