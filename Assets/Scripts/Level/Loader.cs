using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public sealed class Loader : MonoBehaviour
    {
        static readonly Loader instance = new Loader();

        const string AtmoshericPhenomenaConfigName =
            @"Text/Level/AtmosphericPhenomena/config";

        public static Loader Instance { get => instance; }
        static Loader() { }
        Loader() { }

        public void ConfigureAtmosphericPhenomena()
        {
            var serializedData = Resources.Load<TextAsset>(
                AtmoshericPhenomenaConfigName);

            var deserializedData = AtmoshericPhenomenaConfigReader.
                Deserialize<AtmosphericPhenomenonJson>(serializedData.text);


            for (int i = 0; i < deserializedData.Length; i++)
            {
                var phenomenon = FindObjectOfType<Generator>().
                    AtmosphericPhenomenaPool[i].
                    GetComponent<AtmosphericPhenomenon>();
                
                phenomenon.DirectionalSpeed = new Vector2(
                    deserializedData[i].DirectionalSpeed[0],
                    deserializedData[i].DirectionalSpeed[1]);

                phenomenon.LiftRatio = deserializedData[i].LiftRatio;
                phenomenon.RelativeProbabilityPercentage = deserializedData[i].
                    RelativeProbabilityPercentage;
            }
        }
    }

    internal static class AtmoshericPhenomenaConfigReader
    {
        public static T[] Deserialize<T>(string serializedData)
        {
            try
            {
                return JsonUtility.
                    FromJson<AtmoshericPhenomenaConfigContent<T>>(
                    serializedData).AtmosphericPhenomena;
            }
            catch (Exception ex)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                return null;
            }
        }
    }

    [Serializable]
    internal class AtmoshericPhenomenaConfigContent<T>
    {
        public T[] AtmosphericPhenomena;
    }
}
