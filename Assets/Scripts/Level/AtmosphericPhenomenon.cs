using System;
using UnityEngine;

namespace Level
{
    public class AtmosphericPhenomenon : MonoBehaviour
    {
        public float LiftRatio { get; set; }
        public sbyte RelativeProbabilityPercentage { get; set; }
        public Vector2 DirectionalSpeed { get; set; }

        void FixedUpdate()
        {
            if (DirectionalSpeed != Vector2.zero)
            {
                transform.Translate(DirectionalSpeed * Time.deltaTime);
            }
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
