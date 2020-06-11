

namespace Mlf.TileSystem {
  

  [System.Serializable] public struct PathData {

    public bool isWalkable;
    public bool canBuild;



  }


  public static class PahtDataModifiers {


    public static PathData getBasicPathData() {
      return new PathData {
        isWalkable = true,
      };
    }

  }

}