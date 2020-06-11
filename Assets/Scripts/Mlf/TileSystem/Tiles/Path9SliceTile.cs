using System;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Mlf.TileSystem.Tiles
{

    [Serializable]
    [CreateAssetMenu(fileName = "New Path9Slice Tile", menuName = "Mlf/Tiles/Path9Slice Tile")]
    public class Path9SliceTile : TileBase, IPathBaseTile
    {
       
        [SerializeField]
        public Sprite[] m_Sprites;

        [SerializeField] private PathData _pathData;
        public PathData pathData { get => _pathData; set => _pathData = value; }

        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {

                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                    if (TileValue(tileMap, position))
                        tileMap.RefreshTile(position);
                }
        }

        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            UpdateTile(location, tileMap, ref tileData);
        }

        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;

            int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;
            Debug.Log("INT:::: " + mask);
            int index = GetIndex((byte)mask);
            Debug.Log("Index::: " + index);
            if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
            {
                tileData.sprite = m_Sprites[index];
                //tileData.transform = GetTransform((byte)mask);
                tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
                tileData.colliderType = Tile.ColliderType.None;
            }
        }

        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        private int GetIndex(byte mask)
        {
          Debug.Log("BYTE:::: " + mask);
           switch (mask)
            {
                case 0: return 0;
                case 1: return 2;
                case 2: return 0;
                case 3: return 6;
                case 4: return 0;
                case 5: return 0;
                case 6: return 1;
                case 7: return 4;
                case 8: return 0;
                case 9: return 8;
                case 10: return 0;
                case 11: return 7;
                case 12: return 3;
                case 13: return 5;
                case 14: return 2;
                case 15: return 0;
            }
            return -1;
            /*
            switch (mask)
            {
                case 0: return 0;
                case 3: return 6;
                case 6: return 1;
                case 9: return 8;
                case 12: return 3;
                case 1:
                case 2:
                case 4:
                case 5:
                case 10:
                case 8: 
                case 7: return 4;
                case 11: return 7;
                case 13: return 5;
                case 14: return 2;
                case 15: return 0;
            }
            return -1;
            */
        }

        private Matrix4x4 GetTransform(byte mask)
        {
            switch (mask)
            {
                case 9:
                case 10:
                case 7:
                case 2:
                case 8:
                    return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
                case 3: 
                case 14:
                    return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
                case 6: 
                case 13:
                    return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);
            }
            return Matrix4x4.identity;
        }
    }
}
