using UnityEngine;

namespace Finegamedesign.Utils
{
    public class SystemView<T> : MonoBehaviour
        where T : new()
    {
        [SerializeField]
        private T m_System = System<T>.Instance;

        public T System
        {
            get { return m_System; }
        }
    }
}
