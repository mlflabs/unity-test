using System;
using Mlf.TileSystem.Tiles;
using UnityEditor;
using UnityEngine;

namespace Mlf.TileSystem
{
    [CustomEditor(typeof(Path9SliceTile), true)]
    [CanEditMultipleObjects]
    public class Path9SliceEditor : Editor
    {
      private Path9SliceTile tile { get { return (target as Path9SliceTile); } }

        public void OnEnable()
        {
            if (tile.m_Sprites == null || tile.m_Sprites.Length != 9)
            {
                tile.m_Sprites = new Sprite[9];
                EditorUtility.SetDirty(tile);
            }
        }

        public override void OnInspectorGUI() {
          base.OnInspectorGUI();

          EditorGUILayout.LabelField("Place the 9 sprites as indicated");
          EditorGUILayout.Space();

          tile.m_Sprites[0] = (Sprite) EditorGUILayout.ObjectField("0 Center", tile.m_Sprites[0], typeof(Sprite), false, null);
          tile.m_Sprites[1] = (Sprite) EditorGUILayout.ObjectField("1 Top Left", tile.m_Sprites[1], typeof(Sprite), false, null);
          tile.m_Sprites[2] = (Sprite) EditorGUILayout.ObjectField("2 Top Center", tile.m_Sprites[2], typeof(Sprite), false, null);
          tile.m_Sprites[3] = (Sprite) EditorGUILayout.ObjectField("3 Top Right", tile.m_Sprites[3], typeof(Sprite), false, null);
          tile.m_Sprites[4] = (Sprite) EditorGUILayout.ObjectField("4 Left Middle", tile.m_Sprites[4], typeof(Sprite), false, null);
          tile.m_Sprites[5] = (Sprite) EditorGUILayout.ObjectField("5 Right Middle", tile.m_Sprites[5], typeof(Sprite), false, null);
          tile.m_Sprites[6] = (Sprite) EditorGUILayout.ObjectField("6 Bottom Left", tile.m_Sprites[6], typeof(Sprite), false, null);
          tile.m_Sprites[7] = (Sprite) EditorGUILayout.ObjectField("7 Bottom Center", tile.m_Sprites[7], typeof(Sprite), false, null);
          tile.m_Sprites[8] = (Sprite) EditorGUILayout.ObjectField("8 Bottom Right", tile.m_Sprites[8], typeof(Sprite), false, null);
            

        }

    }

}