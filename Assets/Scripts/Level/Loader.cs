using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Level
{
    sealed class Loader : MonoBehaviour
    {
#region Constants
        public readonly Vector2 graveyardPosition = new Vector2(-100.0f, 0.0f);
        const int groundChunksMin = 3;
        const string AtmoshericPhenomenaConfigName =
            @"Text/Level/AtmosphericPhenomena/config";

        const string AtmosphericPhenomenaPoolName = "AtmosphericPhenomenaPool";
        const string GroundChunksPoolName = "GroundChunksPool";
#endregion

#region Prefabs
        [SerializeField]
        GameObject atmosphericPhenomenonPrefab;

        [SerializeField]
        GameObject groundChunkPrefab;
#endregion

        Generator generator;

        public float CameraHalfWidthInWorld { get; set; }
        public float CameraWidthInWorld
        {
            get => CameraHalfWidthInWorld * 2.0f;
        }
        public float GroundChunkHalfWidth { get => GroundChunkWidth / 2.0f; }
        public float GroundChunkWidth { get; set; }

        public List<GameObject> AtmosphericPhenomenaPool { get; set; } =
            new List<GameObject>();

        public List<GameObject> GroundChunksPool { get; set; } =
            new List<GameObject>();

#region Parents
        GameObject AtmosphericPhenomenaParent { get; set; }
        GameObject GroundChunksParent { get; set; }
#endregion

        void Awake()
        {
            CameraHalfWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
                Screen.width, 0.0f, 0.0f)).x;
            try
            {
                InitializeAtmosphericPhenomenaPool();
                InitializeGroundChunksPool();
                ConfigureAtmosphericPhenomena();
            }
            catch (System.Exception ex)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                Utils.UnityQuit.Quit(1);
            }
        }

        void Start()
        {
            generator = gameObject.AddComponent<Generator>();
        }

        public float CenterObjectVertically(in GameObject go) =>
            go.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

        GameObject CreateObjectFromPrefab(in GameObject prefab, string basename)
        {
            BoxCollider2D goBoxCollider;
            GameObject go;
            Sprite goSprite;
            SpriteRenderer goSpriteRenderer;

            go = Instantiate(prefab);

            if ((goSprite = Resources.Load<Sprite>(basename)) == null)
            {
                string errMsg = GetType().Name
                    + " initialization aborted. Unable to load: " + basename;

                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(errMsg);
                }
                throw new FileNotFoundException(errMsg);
            }
            go.transform.position = graveyardPosition;        
            goBoxCollider = go.GetComponent<BoxCollider2D>();
            goSpriteRenderer = go.GetComponent<SpriteRenderer>();

            goSpriteRenderer.sprite = goSprite;
            goBoxCollider.size = goSpriteRenderer.sprite.bounds.size;
            goBoxCollider.offset = goSpriteRenderer.sprite.bounds.center;

            return go;
        }

        void ConfigureAtmosphericPhenomena()
        {
            var serializedData = Resources.Load<TextAsset>(
                AtmoshericPhenomenaConfigName);

            var deserializedData = AtmoshericPhenomenaConfigReader.
                Deserialize<AtmosphericPhenomenonJson>(serializedData.text);

            for (int i = 0; i < deserializedData.Length; i++)
            {
                var phenomenon = AtmosphericPhenomenaPool[i].
                    GetComponent<AtmosphericPhenomenonConfig>();
                
                phenomenon.Initialize(deserializedData[i].LiftRatio,
                    deserializedData[i].RelativeProbabilityPercentage,
                    new Vector2(deserializedData[i].DirectionalSpeed[0],
                                deserializedData[i].DirectionalSpeed[1]));
            }
        }

        void InitializeAtmosphericPhenomenaPool()
        {
            const string spritesPath = @"Sprites/Level/AtmosphericPhenomena/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");
            int initialStreamIndex = Random.Range(0, spritesNames.Length);

            AtmosphericPhenomenaParent = new GameObject(
                AtmosphericPhenomenaPoolName);

            foreach (var name in spritesNames)
            {
                var spriteName = Path.GetFileNameWithoutExtension(name);

                try
                {
                    AtmosphericPhenomenaPool.Add(
                        CreateObjectFromPrefab(atmosphericPhenomenonPrefab,
                        Path.Combine(spritesPath, spriteName)));

                    if (AtmosphericPhenomenaPool[
                        AtmosphericPhenomenaPool.Count - 1].
                        GetComponent<SpriteRenderer>().sprite.name.
                        Contains("cumulonimbus"))
                    {
                        AtmosphericPhenomenaPool[
                            AtmosphericPhenomenaPool.Count - 1].
                            GetComponent<BoxCollider2D>().isTrigger = false;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    if (DebugUtils.GlobalEnabler.activated)
                    {
                        Debug.Log(ex);
                    }
                    Utils.UnityQuit.Quit(1);
                }            
                AtmosphericPhenomenaPool[AtmosphericPhenomenaPool.Count - 1].
                    transform.parent = AtmosphericPhenomenaParent.transform;

                var atmosphericPhenomenon = AtmosphericPhenomenaPool
                    [AtmosphericPhenomenaPool.Count - 1].
                    GetComponent<AtmosphericPhenomenonConfig>();
            }
        }

        void InitializeGroundChunksPool()
        {
            const string spriteNamePrefix = @"ground_chunk_";
            const string spritesPath = @"Sprites/Level/GroundChunks/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");

            GroundChunksParent = new GameObject(GroundChunksPoolName);

            if (spritesNames.Length < groundChunksMin)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(GetType().Name + " initialization aborted. "
                        + $"At least {groundChunksMin} grounds needed.");
                }
                Utils.UnityQuit.Quit(1);
            }
            for (int i = 0; i < spritesNames.Length; i++)
            {
                try
                {
                    GroundChunksPool.Add(
                        CreateObjectFromPrefab(groundChunkPrefab,
                        Path.Combine(spritesPath, spriteNamePrefix) + i));
                }
                catch (FileNotFoundException ex)
                {
                    if (DebugUtils.GlobalEnabler.activated)
                    {
                        Debug.Log(ex);
                    }
                    Utils.UnityQuit.Quit(1);
                }
                GroundChunksPool[i].transform.parent =
                    GroundChunksParent.transform;
            }
        }
    }

    internal static class AtmoshericPhenomenaConfigReader
    {
        public static T[] Deserialize<T>(string serialized)
        {
            try
            {
                return JsonUtility.
                    FromJson<AtmoshericPhenomenaConfigContent<T>>(serialized).
                    AtmosphericPhenomena;
            }
            catch (System.Exception ex)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                return null;
            }
        }
    }

    [System.Serializable]
    internal class AtmoshericPhenomenaConfigContent<T>
    {
        public T[] AtmosphericPhenomena;
    }
}
