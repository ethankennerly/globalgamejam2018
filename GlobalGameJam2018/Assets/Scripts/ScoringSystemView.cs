using Finegamedesign.Tiles;
using Finegamedesign.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Finegamedesign.Virus
{
    public sealed class ScoringSystemView : MonoBehaviour
    {
        [SerializeField]
        private TextMesh[] m_TimeTextMeshes;
        [SerializeField]
        private Text[] m_TimeTexts;

        [SerializeField]
        private Text[] m_NumInfectedTexts;
        [SerializeField]
        private TextMesh[] m_NumInfectedTextMeshes;

        private int m_NumInfected = 0;
        private int m_NumPossibleToInfect = 0;

        private float m_Time = 0f;

        private void OnEnable()
        {
            VirusCountAnimationSystem.onFatal += AddInfected;
            DeltaTimeSystem.onDeltaTime += AddTime;
            m_NumPossibleToInfect = GameObject.FindObjectsOfType(typeof(MobileTile)).Length;
            UpdateInfectedText();
            UpdateTimeText();
        }

        private void OnDisable()
        {
            VirusCountAnimationSystem.onFatal -= AddInfected;
            DeltaTimeSystem.onDeltaTime -= AddTime;
        }

        private void AddInfected()
        {
            ++m_NumInfected;
            UpdateInfectedText();
        }

        private void AddTime(float deltaTime)
        {
            m_Time += deltaTime;
            UpdateTimeText();
        }

        private void UpdateInfectedText()
        {
            string text = m_NumInfected.ToString()
                + " of " + m_NumPossibleToInfect.ToString();
            UpdateTexts(m_NumInfectedTexts, m_NumInfectedTextMeshes, text);
        }

        private void UpdateTexts(Text[] uiTexts, TextMesh[] textMeshes, string text)
        {
            foreach (Text uiText in uiTexts)
            {
                uiText.text = text;
            }
            foreach (TextMesh mesh in textMeshes)
            {
                mesh.text = text;
            }
        }

        private void UpdateTimeText()
        {
            int seconds = (int)m_Time;
            UpdateTexts(m_TimeTexts, m_TimeTextMeshes, seconds.ToString());
        }
    }
}
