using System;
using UnityEngine;

namespace Level
{
    class AtmosphericPhenomenonConfig : MonoBehaviour
    {
        public float LiftRatio { get; private set; }
        public int RelativeProbabilityPercentage { get; private set; }
        public Vector2 DirectionalSpeed { get; private set; }

        void FixedUpdate()
        {
            transform.Translate(DirectionalSpeed * Time.deltaTime);
        }

        public void Initialize(float liftRatio,
                               int relativeProbabilityPercentage,
                               Vector2 directionalSpeed)
        {
            LiftRatio = liftRatio;
            RelativeProbabilityPercentage = relativeProbabilityPercentage;
            DirectionalSpeed = directionalSpeed;
        }
    }

    [Serializable]
    class AtmosphericPhenomenonJson
    {
        public float[] DirectionalSpeed;
        public float LiftRatio;
        public int RelativeProbabilityPercentage;
        public string Name;
    }
}
