using System;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Mlf.TileSystem.Tiles
{

    [Serializable]
    [CreateAssetMenu(fileName = "New PathSimple Tile", menuName = "Mlf/Tiles/PathSimple Tile")]
    public class PathSimpleTile : TileBase
    {
       
        [SerializeField]
        public Sprite m_Sprite;

        [SerializeField] private PathData _pathData;
        public PathData pathData { get => _pathData; set => _pathData = value; }
       

        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {

                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                    TileBase tile = tileMap.GetTile(position);
                    if (tile != null)
                        tileMap.RefreshTile(position);
                }
        }

        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            UpdateTile(location, tileMap, ref tileData);
        }


        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {

          tileData.sprite = m_Sprite;
          //tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
          
        }
    }
}
