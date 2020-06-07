using UnityEngine;

namespace Mlf.Grid.Tilemap {
  
  public class MapGenerator : MonoBehaviour {
    
    public Texture2D map;

    void Start () {
      GenerateMap();
    }


    void GenerateMap() {
      for(int x = 0; x< map.width; x++) {
        for(int y = 0; y<map.height; y ++) {
          GenerateTile(x, y);
        }
      }
    }

    void GenerateTile(int x, int y) {
      Color pixelColor = map.GetPixel(x, y);

      if(pixelColor.a == 0) {
        //transparent
      }
      else {
        Debug.Log(pixelColor);
      }
    }
  }
}