using System;
using UnityEngine;

namespace Level
{
    public class AtmosphericPhenomenon : MonoBehaviour
    {
        public float LiftRatio { get; private set; }
        public sbyte RelativeProbabilityPercentage { get; private set; }
        public Vector2 DirectionalSpeed { get; private set; }

        bool Initialized { get; set; } = false;

        void FixedUpdate()
        {
            if (Initialized)
            {
                transform.Translate(DirectionalSpeed * Time.deltaTime);
            }
        }

        public void Initialize(float liftRatio,
                               sbyte relativeProbabilityPercentage,
                               Vector2 directionalSpeed)
        {
            LiftRatio = liftRatio;
            RelativeProbabilityPercentage = relativeProbabilityPercentage;
            DirectionalSpeed = directionalSpeed;
            Initialized = true;
        }
    }

    [Serializable]
    public class AtmosphericPhenomenonJson
    {
        public float[] DirectionalSpeed;
        public float LiftRatio;
        public sbyte RelativeProbabilityPercentage;
        public string Name;
    }
}
