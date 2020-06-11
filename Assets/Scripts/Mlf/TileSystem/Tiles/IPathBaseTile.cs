using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mlf.TileSystem.Tiles
{

  public interface IPathBaseTile  {

    PathData pathData { get; set; }
  }
}
