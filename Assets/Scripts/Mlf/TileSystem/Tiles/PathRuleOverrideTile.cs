﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

namespace Mlf.TileSystem.Tiles {

    [MovedFrom(true, "UnityEngine")]
    [Serializable]
    [CreateAssetMenu(fileName = "New PathRule Override Tile", menuName = "Mlf/Tiles/PathRule Override Tile", order = 359)]
    public class PathRuleOverrideTile : TileBase
    {
        [Serializable]
        public class TileSpritePair
        {
            public Sprite m_OriginalSprite;
            public Sprite m_OverrideSprite;
        }

        [Serializable]
        public class TileGameObjectPair
        {
            public GameObject m_OriginalGameObject;
            public GameObject m_OverrideGameObject;
        }
        public Sprite this[Sprite originalSprite]
        {
            get
            {
                foreach (TileSpritePair spritePair in m_Sprites)
                {
                    if (spritePair.m_OriginalSprite == originalSprite)
                    {
                        return spritePair.m_OverrideSprite;
                    }
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    m_Sprites = m_Sprites.Where(spritePair => spritePair.m_OriginalSprite != originalSprite).ToList();
                }
                else
                {
                    foreach (TileSpritePair spritePair in m_Sprites)
                    {
                        if (spritePair.m_OriginalSprite == originalSprite)
                        {
                            spritePair.m_OverrideSprite = value;
                            return;
                        }
                    }
                    m_Sprites.Add(new TileSpritePair()
                    {
                        m_OriginalSprite = originalSprite,
                        m_OverrideSprite = value,
                    });
                }
            }
        }
        public GameObject this[GameObject originalGameObject]
        {
            get
            {
                foreach (TileGameObjectPair gameObjectPair in m_GameObjects)
                {
                    if (gameObjectPair.m_OriginalGameObject == originalGameObject)
                    {
                        return gameObjectPair.m_OverrideGameObject;
                    }
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    m_GameObjects = m_GameObjects.Where(gameObjectPair => gameObjectPair.m_OriginalGameObject != originalGameObject).ToList();
                }
                else
                {
                    foreach (TileGameObjectPair gameObjectPair in m_GameObjects)
                    {
                        if (gameObjectPair.m_OriginalGameObject == originalGameObject)
                        {
                            gameObjectPair.m_OverrideGameObject = value;
                            return;
                        }
                    }
                    m_GameObjects.Add(new TileGameObjectPair()
                    {
                        m_OriginalGameObject = originalGameObject,
                        m_OverrideGameObject = value,
                    });
                }
            }
        }


        public PathRuleTile m_Tile;
        public List<TileSpritePair> m_Sprites = new List<TileSpritePair>();
        public List<TileGameObjectPair> m_GameObjects = new List<TileGameObjectPair>();

        [HideInInspector] public PathRuleTile m_InstanceTile;
        public void ApplyOverrides(IList<KeyValuePair<Sprite, Sprite>> overrides)
        {
            if (overrides == null)
                throw new System.ArgumentNullException("overrides");

            for (int i = 0; i < overrides.Count; i++)
                this[overrides[i].Key] = overrides[i].Value;
        }
        public void ApplyOverrides(IList<KeyValuePair<GameObject, GameObject>> overrides)
        {
            if (overrides == null)
                throw new System.ArgumentNullException("overrides");

            for (int i = 0; i < overrides.Count; i++)
                this[overrides[i].Key] = overrides[i].Value;
        }
        public void GetOverrides(List<KeyValuePair<Sprite, Sprite>> overrides, ref int validCount)
        {
            if (overrides == null)
                throw new System.ArgumentNullException("overrides");

            overrides.Clear();

            List<Sprite> originalSprites = new List<Sprite>();

            if (m_Tile)
            {
                if (m_Tile.m_DefaultSprite)
                    originalSprites.Add(m_Tile.m_DefaultSprite);

                foreach (PathRuleTile.PathTilingRule rule in m_Tile.m_TilingRules)
                    foreach (Sprite sprite in rule.m_Sprites)
                        if (sprite && !originalSprites.Contains(sprite))
                            originalSprites.Add(sprite);
            }

            validCount = originalSprites.Count;

            foreach (var pair in m_Sprites)
                if (!originalSprites.Contains(pair.m_OriginalSprite))
                    originalSprites.Add(pair.m_OriginalSprite);

            foreach (Sprite sprite in originalSprites)
                overrides.Add(new KeyValuePair<Sprite, Sprite>(sprite, this[sprite]));
        }
        public void GetOverrides(List<KeyValuePair<GameObject, GameObject>> overrides, ref int validCount)
        {
            if (overrides == null)
                throw new System.ArgumentNullException("overrides");

            overrides.Clear();

            List<GameObject> originalGameObjects = new List<GameObject>();

            if (m_Tile)
            {
                if (m_Tile.m_DefaultGameObject)
                    originalGameObjects.Add(m_Tile.m_DefaultGameObject);

                foreach (PathRuleTile.PathTilingRule rule in m_Tile.m_TilingRules)
                    if (rule.m_GameObject && !originalGameObjects.Contains(rule.m_GameObject))
                        originalGameObjects.Add(rule.m_GameObject);
            }

            validCount = originalGameObjects.Count;

            foreach (var pair in m_GameObjects)
                if (!originalGameObjects.Contains(pair.m_OriginalGameObject))
                    originalGameObjects.Add(pair.m_OriginalGameObject);

            foreach (GameObject gameObject in originalGameObjects)
                overrides.Add(new KeyValuePair<GameObject, GameObject>(gameObject, this[gameObject]));
        }

        public virtual void Override()
        {
            if (!m_Tile || !m_InstanceTile)
                return;

            PrepareOverride();

            var tile = m_InstanceTile;

            tile.m_DefaultSprite = this[tile.m_DefaultSprite] ?? tile.m_DefaultSprite;
            tile.m_DefaultGameObject = this[tile.m_DefaultGameObject] ?? tile.m_DefaultGameObject;

            foreach (var rule in tile.m_TilingRules)
            {
                for (int i = 0; i < rule.m_Sprites.Length; i++)
                {
                    Sprite sprite = rule.m_Sprites[i];
                    rule.m_Sprites[i] = this[sprite] ?? sprite;
                }

                rule.m_GameObject = this[rule.m_GameObject] ?? rule.m_GameObject;
            }
        }

        public void PrepareOverride()
        {
            var customData = m_InstanceTile.GetCustomFields(true)
                .ToDictionary(field => field, field => field.GetValue(m_InstanceTile));

            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(m_Tile), m_InstanceTile);

            foreach (var kvp in customData)
                kvp.Key.SetValue(m_InstanceTile, kvp.Value);
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            if (!m_InstanceTile)
                return false;
            return m_InstanceTile.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (!m_InstanceTile)
                return;
            m_InstanceTile.GetTileData(position, tilemap, ref tileData);
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            if (!m_InstanceTile)
                return;
            m_InstanceTile.RefreshTile(position, tilemap);
        }
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (!m_InstanceTile)
                return true;
            return m_InstanceTile.StartUp(position, tilemap, go);
        }
    }
}
