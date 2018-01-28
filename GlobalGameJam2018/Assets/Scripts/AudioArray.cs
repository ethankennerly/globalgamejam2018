using System;
using UnityEngine;

namespace Finegamedesign.Utils
{
    [Serializable]
    public sealed class AudioArray
    {
        [SerializeField]
        private AudioSource m_Source;
        [SerializeField]
        private AudioClip[] m_Clips;
        [Range(0f, 8f)]
        [SerializeField]
        private float m_VolumeScale = 1f;

        private int m_ClipIndex = 0;

        public void PlayNext()
        {
            AudioClip clip = m_Clips[m_ClipIndex];
            ++m_ClipIndex;
            if (m_ClipIndex >= m_Clips.Length)
            {
                m_ClipIndex = 0;
            }
            m_Source.PlayOneShot(clip, m_VolumeScale);
        }
    }
}
