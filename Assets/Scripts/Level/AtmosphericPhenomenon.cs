using UnityEngine;

namespace Level
{
    public class AtmosphericPhenomenon : MonoBehaviour
    {
        public float LiftRatio { get; set; }
        public Vector2 DirectionalSpeed { get; set; }

        void FixedUpdate()
        {
            if (DirectionalSpeed != Vector2.zero)
            {
                transform.Translate(DirectionalSpeed * Time.deltaTime);
            }
        }
    }    
}
