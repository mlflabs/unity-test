using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
//using UnityEngine.Tilemaps;
using UnityEngine;

namespace Mlf.TileSystem.Tiles
{
    public class PathRuleTile<T> : PathRuleTile
    {
        /// <summary>
        /// Returns the Neighbor Rule Class type for this Rule Tile.
        /// </summary>
        public sealed override Type m_NeighborType => typeof(T);
    }

    /// <summary>
    /// Generic visual tile for creating different tilesets like terrain, pipeline, random or animated tiles.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New PathRule Tile", menuName = "Mlf/Tiles/PathRule Tile")]
    public class PathRuleTile : UnityEngine.Tilemaps.TileBase, IPathBaseTile
    {
      [SerializeField] private PathData _pathData;
      public PathData pathData { get => _pathData; set => _pathData = value; }

        
      public virtual Type m_NeighborType => typeof(PathTilingRule.Neighbor);
      public Sprite m_DefaultSprite;
      public GameObject m_DefaultGameObject;
      public UnityEngine.Tilemaps.Tile.ColliderType m_DefaultColliderType = 
        UnityEngine.Tilemaps.Tile.ColliderType.Sprite;
      public virtual int m_RotationAngle => 90;
      public int m_RotationCount => 360 / m_RotationAngle;

      [Serializable]
      public class TilingRuleOutput
      {
            public int m_Id;
            public Sprite[] m_Sprites = new Sprite[1];
            public GameObject m_GameObject;
            public float m_AnimationSpeed = 1f;
            public float m_PerlinScale = 0.5f;
            public OutputSprite m_Output = OutputSprite.Single;
            public UnityEngine.Tilemaps.Tile.ColliderType m_ColliderType = 
              UnityEngine.Tilemaps.Tile.ColliderType.Sprite;
            public Transform m_RandomTransform;

            public class Neighbor
            {
                public const int This = 1;
                public const int NotThis = 2;
            }

            public enum Transform
            {
                Fixed,
                Rotated,
                MirrorX,
                MirrorY,
                MirrorXY
            }
            public enum OutputSprite
            {
                Single,
                Random,
                Animation
            }
        }

        [Serializable]
        public class PathTilingRule : TilingRuleOutput
        {
            public List<int> m_Neighbors = new List<int>();
            public List<Vector3Int> m_NeighborPositions = new List<Vector3Int>()
            {
                new Vector3Int(-1, 1, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, -1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(1, -1, 0),
            };
            public Transform m_RuleTransform;

            public Dictionary<Vector3Int, int> GetNeighbors()
            {
                Dictionary<Vector3Int, int> dict = new Dictionary<Vector3Int, int>();

                for (int i = 0; i < m_Neighbors.Count && i < m_NeighborPositions.Count; i++)
                    dict.Add(m_NeighborPositions[i], m_Neighbors[i]);

                return dict;
            }

            public void ApplyNeighbors(Dictionary<Vector3Int, int> dict)
            {
                m_NeighborPositions = dict.Keys.ToList();
                m_Neighbors = dict.Values.ToList();
            }

            public BoundsInt GetBounds()
            {
                BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.one);
                foreach (var neighbor in GetNeighbors())
                {
                    bounds.xMin = Mathf.Min(bounds.xMin, neighbor.Key.x);
                    bounds.yMin = Mathf.Min(bounds.yMin, neighbor.Key.y);
                    bounds.xMax = Mathf.Max(bounds.xMax, neighbor.Key.x + 1);
                    bounds.yMax = Mathf.Max(bounds.yMax, neighbor.Key.y + 1);
                }
                return bounds;
            }
        }

        public class DontOverride : Attribute { }

        [HideInInspector] public List<PathTilingRule> m_TilingRules = new List<PathRuleTile.PathTilingRule>();

        public HashSet<Vector3Int> neighborPositions
        {
            get
            {
                if (m_NeighborPositions.Count == 0)
                    UpdateNeighborPositions();

                return m_NeighborPositions;
            }
        }

        private HashSet<Vector3Int> m_NeighborPositions = new HashSet<Vector3Int>();

        public void UpdateNeighborPositions()
        {
            m_CacheTilemapsNeighborPositions.Clear();

            HashSet<Vector3Int> positions = m_NeighborPositions;
            positions.Clear();

            foreach (PathTilingRule rule in m_TilingRules)
            {
                foreach (var neighbor in rule.GetNeighbors())
                {
                    Vector3Int position = neighbor.Key;
                    positions.Add(position);

                    // Check rule against rotations of 0, 90, 180, 270
                    if (rule.m_RuleTransform == PathTilingRule.Transform.Rotated)
                    {
                        for (int angle = m_RotationAngle; angle < 360; angle += m_RotationAngle)
                        {
                            positions.Add(GetRotatedPosition(position, angle));
                        }
                    }
                    // Check rule against x-axis, y-axis mirror
                    else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorXY)
                    {
                        positions.Add(GetMirroredPosition(position, true, true));
                        positions.Add(GetMirroredPosition(position, true, false));
                        positions.Add(GetMirroredPosition(position, false, true));
                    }
                    // Check rule against x-axis mirror
                    else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorX)
                    {
                        positions.Add(GetMirroredPosition(position, true, false));
                    }
                    // Check rule against y-axis mirror
                    else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorY)
                    {
                        positions.Add(GetMirroredPosition(position, false, true));
                    }
                }
            }
        }

        public override bool StartUp(Vector3Int location, UnityEngine.Tilemaps.ITilemap tilemap, GameObject instantiatedGameObject)
        {
            if (instantiatedGameObject != null)
            {
                UnityEngine.Tilemaps.Tilemap tmpMap = 
                  tilemap.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                Matrix4x4 orientMatrix = tmpMap.orientationMatrix;

                var iden = Matrix4x4.identity;
                Vector3 gameObjectTranslation = new Vector3();
                Quaternion gameObjectRotation = new Quaternion();
                Vector3 gameObjectScale = new Vector3();

                bool ruleMatched = false;
                foreach (PathTilingRule rule in m_TilingRules)
                {
                    Matrix4x4 transform = iden;
                    if (RuleMatches(rule, location, tilemap, ref transform))
                    {
                        transform = orientMatrix * transform;

                        // Converts the tile's translation, rotation, & scale matrix to values to be used by the instantiated Game Object
                        gameObjectTranslation = new Vector3(transform.m03, transform.m13, transform.m23);
                        gameObjectRotation = Quaternion.LookRotation(new Vector3(transform.m02, transform.m12, transform.m22), new Vector3(transform.m01, transform.m11, transform.m21));
                        gameObjectScale = transform.lossyScale;

                        ruleMatched = true;
                        break;
                    }
                }
                if (!ruleMatched)
                {
                    // Fallback to just using the orientMatrix for the translation, rotation, & scale values.
                    gameObjectTranslation = new Vector3(orientMatrix.m03, orientMatrix.m13, orientMatrix.m23);
                    gameObjectRotation = Quaternion.LookRotation(new Vector3(orientMatrix.m02, orientMatrix.m12, orientMatrix.m22), new Vector3(orientMatrix.m01, orientMatrix.m11, orientMatrix.m21));
                    gameObjectScale = orientMatrix.lossyScale;
                }

                instantiatedGameObject.transform.localPosition = gameObjectTranslation + tmpMap.CellToLocalInterpolated(location + tmpMap.tileAnchor);
                instantiatedGameObject.transform.localRotation = gameObjectRotation;
                instantiatedGameObject.transform.localScale = gameObjectScale;
            }

            return true;
        }
        public override void GetTileData(Vector3Int position, 
          UnityEngine.Tilemaps.ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
        {
            var iden = Matrix4x4.identity;

            tileData.sprite = m_DefaultSprite;
            tileData.gameObject = m_DefaultGameObject;
            tileData.colliderType = m_DefaultColliderType;
            tileData.flags = UnityEngine.Tilemaps.TileFlags.LockTransform;
            tileData.transform = iden;

            foreach (PathTilingRule rule in m_TilingRules)
            {
                Matrix4x4 transform = iden;
                if (RuleMatches(rule, position, tilemap, ref transform))
                {
                    switch (rule.m_Output)
                    {
                        case PathTilingRule.OutputSprite.Single:
                        case PathTilingRule.OutputSprite.Animation:
                            tileData.sprite = rule.m_Sprites[0];
                            break;
                        case PathTilingRule.OutputSprite.Random:
                            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, rule.m_PerlinScale, 100000f) * rule.m_Sprites.Length), 0, rule.m_Sprites.Length - 1);
                            tileData.sprite = rule.m_Sprites[index];
                            if (rule.m_RandomTransform != PathTilingRule.Transform.Fixed)
                                transform = ApplyRandomTransform(rule.m_RandomTransform, transform, rule.m_PerlinScale, position);
                            break;
                    }
                    tileData.transform = transform;
                    tileData.gameObject = rule.m_GameObject;
                    tileData.colliderType = rule.m_ColliderType;
                    break;
                }
            }
        }

        public static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }

        static Dictionary<UnityEngine.Tilemaps.Tilemap, KeyValuePair<HashSet<UnityEngine.Tilemaps.TileBase>, HashSet<Vector3Int>>> m_CacheTilemapsNeighborPositions = 
          new Dictionary<UnityEngine.Tilemaps.Tilemap, KeyValuePair<HashSet<UnityEngine.Tilemaps.TileBase>, HashSet<Vector3Int>>>();
        static UnityEngine.Tilemaps.TileBase[] m_AllocatedUsedTileArr = 
          new UnityEngine.Tilemaps.TileBase[0];

        static bool IsTilemapUsedTilesChange(UnityEngine.Tilemaps.Tilemap tilemap)
        {
            if (!m_CacheTilemapsNeighborPositions.ContainsKey(tilemap))
                return true;

            var oldUsedTiles = m_CacheTilemapsNeighborPositions[tilemap].Key;
            int newUsedTilesCount = tilemap.GetUsedTilesCount();

            if (newUsedTilesCount != oldUsedTiles.Count)
                return true;

            if (m_AllocatedUsedTileArr.Length < newUsedTilesCount)
                Array.Resize(ref m_AllocatedUsedTileArr, newUsedTilesCount);

            tilemap.GetUsedTilesNonAlloc(m_AllocatedUsedTileArr);

            for (int i = 0; i < newUsedTilesCount; i++)
            {
                UnityEngine.Tilemaps.TileBase newUsedTile = m_AllocatedUsedTileArr[i];
                if (!oldUsedTiles.Contains(newUsedTile))
                    return true;
            }

            return false;
        }
        static void CachingTilemapNeighborPositions(UnityEngine.Tilemaps.Tilemap tilemap)
        {
            int usedTileCount = tilemap.GetUsedTilesCount();
            HashSet<UnityEngine.Tilemaps.TileBase> usedTiles 
              = new HashSet<UnityEngine.Tilemaps.TileBase>();
            HashSet<Vector3Int> neighborPositions = new HashSet<Vector3Int>();

            if (m_AllocatedUsedTileArr.Length < usedTileCount)
                Array.Resize(ref m_AllocatedUsedTileArr, usedTileCount);

            tilemap.GetUsedTilesNonAlloc(m_AllocatedUsedTileArr);

            for (int i = 0; i < usedTileCount; i++)
            {
                UnityEngine.Tilemaps.TileBase tile = m_AllocatedUsedTileArr[i];
                usedTiles.Add(tile);
                PathRuleTile ruleTile = null;

                if (tile is RuleTile)
                    ruleTile = tile as PathRuleTile;
                else if (tile is PathRuleOverrideTile)
                    ruleTile = (tile as PathRuleOverrideTile).m_Tile;

                if (ruleTile)
                    foreach (Vector3Int neighborPosition in ruleTile.neighborPositions)
                        neighborPositions.Add(neighborPosition);
            }

            m_CacheTilemapsNeighborPositions[tilemap] = 
              new KeyValuePair<HashSet<UnityEngine.Tilemaps.TileBase>, HashSet<Vector3Int>>(usedTiles, neighborPositions);
        }
        static void ReleaseDestroyedTilemapCacheData()
        {
            m_CacheTilemapsNeighborPositions = m_CacheTilemapsNeighborPositions
                .Where(data => data.Key != null)
                .ToDictionary(data => data.Key, data => data.Value);
        }

        public override bool GetTileAnimationData(
          Vector3Int position, 
          UnityEngine.Tilemaps.ITilemap tilemap, 
          ref UnityEngine.Tilemaps.TileAnimationData tileAnimationData)
        {
            var iden = Matrix4x4.identity;
            foreach (PathTilingRule rule in m_TilingRules)
            {
                if (rule.m_Output == PathTilingRule.OutputSprite.Animation)
                {
                    Matrix4x4 transform = iden;
                    if (RuleMatches(rule, position, tilemap, ref transform))
                    {
                        tileAnimationData.animatedSprites = rule.m_Sprites;
                        tileAnimationData.animationSpeed = rule.m_AnimationSpeed;
                        return true;
                    }
                }
            }
            return false;
        }
        public override void RefreshTile(Vector3Int location, UnityEngine.Tilemaps.ITilemap tilemap)
        {
            base.RefreshTile(location, tilemap);

            UnityEngine.Tilemaps.Tilemap tilemap_2 = tilemap.GetComponent<UnityEngine.Tilemaps.Tilemap>();

            ReleaseDestroyedTilemapCacheData(); // Prevent memory leak

            if (IsTilemapUsedTilesChange(tilemap_2))
                CachingTilemapNeighborPositions(tilemap_2);

            HashSet<Vector3Int> neighborPositions = m_CacheTilemapsNeighborPositions[tilemap_2].Value;
            foreach (Vector3Int offset in neighborPositions)
            {
                Vector3Int position = GetOffsetPositionReverse(location, offset);
                UnityEngine.Tilemaps.TileBase tile = tilemap_2.GetTile(position);
                PathRuleTile ruleTile = null;

                if (tile is PathRuleTile)
                    ruleTile = tile as PathRuleTile;
                else if (tile is PathRuleOverrideTile)
                    ruleTile = (tile as PathRuleOverrideTile).m_Tile;

                if (ruleTile)
                    if (ruleTile.neighborPositions.Contains(offset))
                        base.RefreshTile(position, tilemap);
            }
        }

        public virtual bool RuleMatches(
          PathTilingRule rule, 
          Vector3Int position, 
          UnityEngine.Tilemaps.ITilemap tilemap, 
          ref Matrix4x4 transform)
        {
            if (RuleMatches(rule, position, tilemap, 0))
            {
                transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
                return true;
            }

            // Check rule against rotations of 0, 90, 180, 270
            if (rule.m_RuleTransform == PathTilingRule.Transform.Rotated)
            {
                for (int angle = m_RotationAngle; angle < 360; angle += m_RotationAngle)
                {
                    if (RuleMatches(rule, position, tilemap, angle))
                    {
                        transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one);
                        return true;
                    }
                }
            }
            // Check rule against x-axis, y-axis mirror
            else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorXY)
            {
                if (RuleMatches(rule, position, tilemap, true, true))
                {
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, -1f, 1f));
                    return true;
                }
                if (RuleMatches(rule, position, tilemap, true, false))
                {
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
                    return true;
                }
                if (RuleMatches(rule, position, tilemap, false, true))
                {
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
                    return true;
                }
            }
            // Check rule against x-axis mirror
            else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorX)
            {
                if (RuleMatches(rule, position, tilemap, true, false))
                {
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
                    return true;
                }
            }
            // Check rule against y-axis mirror
            else if (rule.m_RuleTransform == PathTilingRule.Transform.MirrorY)
            {
                if (RuleMatches(rule, position, tilemap, false, true))
                {
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
                    return true;
                }
            }

            return false;
        }
        public virtual Matrix4x4 ApplyRandomTransform(PathTilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
        {
            float perlin = GetPerlinValue(position, perlinScale, 200000f);
            switch (type)
            {
                case PathTilingRule.Transform.MirrorXY:
                    return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Math.Abs(perlin - 0.5) > 0.25 ? 1f : -1f, perlin < 0.5 ? 1f : -1f, 1f));
                case PathTilingRule.Transform.MirrorX:
                    return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(perlin < 0.5 ? 1f : -1f, 1f, 1f));
                case PathTilingRule.Transform.MirrorY:
                    return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, perlin < 0.5 ? 1f : -1f, 1f));
                case PathTilingRule.Transform.Rotated:
                    int angle = Mathf.Clamp(Mathf.FloorToInt(perlin * m_RotationCount), 0, m_RotationCount - 1) * m_RotationAngle;
                    return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one);
            }
            return original;
        }

        public FieldInfo[] GetCustomFields(bool isOverrideInstance)
        {
            return this.GetType().GetFields()
                .Where(field => typeof(PathRuleTile).GetField(field.Name) == null)
                .Where(field => !field.IsDefined(typeof(HideInInspector)))
                .Where(field => !isOverrideInstance || !field.IsDefined(typeof(PathRuleTile.DontOverride)))
                .ToArray();
        }
        public virtual bool RuleMatch(int neighbor, UnityEngine.Tilemaps.TileBase other)
        {
            if (other is PathRuleOverrideTile)
                other = (other as PathRuleOverrideTile).m_InstanceTile;

            switch (neighbor)
            {
                case PathTilingRule.Neighbor.This: return other == this;
                case PathTilingRule.Neighbor.NotThis: return other != this;
            }
            return true;
        }
        public bool RuleMatches(PathTilingRule rule, Vector3Int position, UnityEngine.Tilemaps.ITilemap tilemap, int angle)
        {
            for (int i = 0; i < rule.m_Neighbors.Count && i < rule.m_NeighborPositions.Count; i++)
            {
                int neighbor = rule.m_Neighbors[i];
                Vector3Int positionOffset = GetRotatedPosition(rule.m_NeighborPositions[i], angle);
                UnityEngine.Tilemaps.TileBase other = tilemap.GetTile(GetOffsetPosition(position, positionOffset));
                if (!RuleMatch(neighbor, other))
                {
                    return false;
                }
            }
            return true;
        }

        public bool RuleMatches(
          PathTilingRule rule, 
          Vector3Int position, 
          UnityEngine.Tilemaps.ITilemap tilemap, bool mirrorX, bool mirrorY)
        {
            for (int i = 0; i < rule.m_Neighbors.Count && i < rule.m_NeighborPositions.Count; i++)
            {
                int neighbor = rule.m_Neighbors[i];
                Vector3Int positionOffset = GetMirroredPosition(rule.m_NeighborPositions[i], mirrorX, mirrorY);
                UnityEngine.Tilemaps.TileBase other = tilemap.GetTile(GetOffsetPosition(position, positionOffset));
                if (!RuleMatch(neighbor, other))
                {
                    return false;
                }
            }
            return true;
        }
        public virtual Vector3Int GetRotatedPosition(Vector3Int position, int rotation)
        {
            switch (rotation)
            {
                case 0:
                    return position;
                case 90:
                    return new Vector3Int(position.y, -position.x, 0);
                case 180:
                    return new Vector3Int(-position.x, -position.y, 0);
                case 270:
                    return new Vector3Int(-position.y, position.x, 0);
            }
            return position;
        }

        public virtual Vector3Int GetMirroredPosition(Vector3Int position, bool mirrorX, bool mirrorY)
        {
            if (mirrorX)
                position.x *= -1;
            if (mirrorY)
                position.y *= -1;
            return position;
        }

        public virtual Vector3Int GetOffsetPosition(Vector3Int location, Vector3Int offset)
        {
            return location + offset;
        }

        public virtual Vector3Int GetOffsetPositionReverse(Vector3Int position, Vector3Int offset)
        {
            return position - offset;
        }
    }
}
