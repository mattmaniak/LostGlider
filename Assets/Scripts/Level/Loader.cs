using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Level
{
    internal sealed class Loader : ScriptableObject
    {
#region Singleton
        static readonly Loader instance = new Loader();
        internal static Loader Instance { get => instance; }
        
        static Loader() { }
        Loader() { }
#endregion

#region Constants
        const int groundChunksMin = 3;
        const string AtmoshericPhenomenaConfigName =
            @"Text/Level/AtmosphericPhenomena/config";

        const string AtmosphericPhenomenaPoolName = "AtmosphericPhenomenaPool";
        const string GroundChunksPoolName = "GroundChunksPool";
#endregion

        internal float CameraHalfWidthInWorld { get; set; }
        internal float CameraWidthInWorld
        {
            get => CameraHalfWidthInWorld * 2.0f;
        }
        internal float GroundChunkHalfWidth { get => GroundChunkWidth / 2.0f; }
        internal float GroundChunkWidth { get; set; }

#region Parents
        GameObject AtmosphericPhenomenaParent { get; set; }
        GameObject GroundChunksParent { get; set; }
#endregion

#region Prefabs
        GameObject AtmosphericPhenomenonPrefab { get; set; }
        GameObject GroundChunkPrefab { get; set; }
#endregion

        public void ConfigureAtmosphericPhenomena()
        {
            var serializedData = Resources.Load<TextAsset>(
                AtmoshericPhenomenaConfigName);

            var deserializedData = AtmoshericPhenomenaConfigReader.
                Deserialize<AtmosphericPhenomenonJson>(serializedData.text);

            for (int i = 0; i < deserializedData.Length; i++)
            {
                var phenomenon =
                    Generator.Instance.AtmosphericPhenomenaPool[i].
                    GetComponent<AtmosphericPhenomenon>();
                
                phenomenon.Initialize(deserializedData[i].LiftRatio,
                    deserializedData[i].RelativeProbabilityPercentage,
                    new Vector2(deserializedData[i].DirectionalSpeed[0],
                                deserializedData[i].DirectionalSpeed[1]));
            }
        }

        public void InitializeAtmosphericPhenomenaPool()
        {
            const string spritesPath = @"Sprites/Level/AtmosphericPhenomena/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");
            int initialStreamIndex = Random.Range(0, spritesNames.Length);

            Generator.Instance.InitialAtmosphericPhenomenon = true;

            AtmosphericPhenomenonPrefab = Resources.
                Load<GameObject>(@"Prefabs/Level/AtmosphericPhenomenon");
            AtmosphericPhenomenaParent = new GameObject(
                AtmosphericPhenomenaPoolName);

            foreach (var name in spritesNames)
            {
                var spriteName = Path.GetFileNameWithoutExtension(name);

                try
                {
                    Generator.Instance.AtmosphericPhenomenaPool.Add(
                        CreateObjectFromPrefab(AtmosphericPhenomenonPrefab,
                        Path.Combine(spritesPath, spriteName)));

                    if (Generator.Instance.AtmosphericPhenomenaPool[Generator.
                        Instance.AtmosphericPhenomenaPool.Count - 1].
                        GetComponent<SpriteRenderer>().sprite.name.
                        Contains("cumulonimbus"))
                    {
                        Generator.Instance.AtmosphericPhenomenaPool[Generator.
                            Instance.AtmosphericPhenomenaPool.Count - 1].
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
                Generator.Instance.AtmosphericPhenomenaPool[Generator.Instance.AtmosphericPhenomenaPool.Count - 1].
                    transform.parent = AtmosphericPhenomenaParent.transform;

                var atmosphericPhenomenon = Generator.Instance.
                    AtmosphericPhenomenaPool
                    [Generator.Instance.AtmosphericPhenomenaPool.Count - 1].
                    GetComponent<AtmosphericPhenomenon>();
            }
        }

        public void InitializeGroundChunksPool()
        {
            const string spriteNamePrefix = @"ground_chunk_";
            const string spritesPath = @"Sprites/Level/GroundChunks/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");

            GroundChunkPrefab = Resources.
                Load<GameObject>(@"Prefabs/Level/GroundChunk");
            GroundChunksParent = new GameObject(GroundChunksPoolName);

            Generator.Instance.CurrentGroundChunkIndex = Random.Range(
                0, spritesNames.Length);
            
            Generator.Instance.NextGroundChunkTransitionX = Generator.Instance.
                CameraLeftEdgeInWorldX;
            
            Generator.Instance.InitialGroundChunk = true;

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
                    Generator.Instance.GroundChunksPool.Add(
                        CreateObjectFromPrefab(GroundChunkPrefab,
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
                Generator.Instance.GroundChunksPool[i].transform.parent =
                    GroundChunksParent.transform;

                if (i == Generator.Instance.CurrentGroundChunkIndex)
                {
                    var groundChunk = Generator.Instance.GroundChunksPool[i];

                    GroundChunkWidth = groundChunk.
                        GetComponent<SpriteRenderer>().sprite.bounds.size.x;

                    groundChunk.transform.position = new Vector2(
                        GroundChunkHalfWidth - CameraHalfWidthInWorld
                        + Camera.main.transform.localPosition.x
                        + FindObjectOfType<Player>().transform.position.x,
                        Generator.Instance.CenterObjectVertically(groundChunk));
                }
            }
        }

        GameObject CreateObjectFromPrefab(in GameObject sourcePrefab,
                                          string basename)
        {
            BoxCollider2D goBoxCollider;
            GameObject go;
            Sprite goSprite;
            SpriteRenderer goSpriteRenderer;

            go = Instantiate(sourcePrefab);

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
            go.transform.position = Generator.Instance.graveyardPosition;        
            goBoxCollider = go.GetComponent<BoxCollider2D>();
            goSpriteRenderer = go.GetComponent<SpriteRenderer>();

            goSpriteRenderer.sprite = goSprite;
            goBoxCollider.size = goSpriteRenderer.sprite.bounds.size;
            goBoxCollider.offset = goSpriteRenderer.sprite.bounds.center;

            return go;
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
