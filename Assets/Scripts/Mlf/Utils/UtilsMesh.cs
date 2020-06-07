using UnityEngine;
using Mlf.Grid;
using System.Drawing;

namespace Mlf.Utils {

  public static class UtilsMesh
  {
    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
      vertices = new Vector3[4 * quadCount];
      uvs = new Vector2[4 * quadCount];
      triangles = new int[6 * quadCount];
    }



    //public static void AddToMeshArrays(Vector3[] vertices, Vector2 uvs, int[] triangles,
    //                                   int index, Vector3 position, int quadSize, Vector2 gridValueUv, ....)
    //

    public static Mesh GenerateTileGridMesh(Grid<int> grid, Mesh mesh,
                int MAX_VALUE = 100, int MIN_VALUE = 0) {

          if(mesh == null) mesh = new Mesh();

          // create our arrays
          Vector3[] vertices = new Vector3[4 * grid.Width * grid.Height];
          Vector2[] uv = new Vector2[4 * grid.Width * grid.Height];
          int[] triangles = new int[6 * grid.Width * grid.Height];

          for (int ti=0, vi=0, y = 0; y < grid.Height; y++)
          {
              for (int x = 0; x < grid.Width; x++, ti += 6, vi +=4)
              {
           
                  vertices[vi] = new Vector3(x, y);
                  vertices[vi + 1] = new Vector3(x, y+1);
                  vertices[vi + 2] = new Vector3(x+1, y+1);
                  vertices[vi + 3] = new Vector3(x+1, y);

                  uv[vi] = new Vector2((float)grid.GetGridObject(x, y) / MAX_VALUE, MIN_VALUE);
                  uv[vi + 1] = new Vector2((float)grid.GetGridObject(x, y) / MAX_VALUE, MIN_VALUE);
                  uv[vi + 2] = new Vector2((float)grid.GetGridObject(x, y) / MAX_VALUE, MIN_VALUE);
                  uv[vi + 3] = new Vector2((float)grid.GetGridObject(x, y) / MAX_VALUE, MIN_VALUE);

                  triangles[ti] = vi;
                  triangles[ti + 1] = triangles[ti + 4] = vi + 1;
                  triangles[ti + 5] = vi + 2;
                  triangles[ti + 2] = triangles[ti + 3] = vi + 3;



              }
          }
          mesh.vertices = vertices;
          mesh.uv = uv;
          mesh.triangles = triangles;

          return mesh;
      }



      public static Mesh GenerateTileGridMesh(Grid<bool> grid, Mesh mesh) {

          if (mesh == null) mesh = new Mesh();

          // create our arrays
          Vector3[] vertices = new Vector3[4 * grid.Width * grid.Height];
          Vector2[] uv = new Vector2[4 * grid.Width * grid.Height];
          int[] triangles = new int[6 * grid.Width * grid.Height];

          for (int ti = 0, vi = 0, y = 0; y < grid.Height; y++)
          {
              for (int x = 0; x < grid.Width; x++, ti += 6, vi += 4)
              {

                  vertices[vi] = new Vector3(x, y);
                  vertices[vi + 1] = new Vector3(x, y + 1);
                  vertices[vi + 2] = new Vector3(x + 1, y + 1);
                  vertices[vi + 3] = new Vector3(x + 1, y);

                  uv[vi] = new Vector2(0f, 0f);
                  uv[vi + 1] = new Vector2(0f, 0f);
                  uv[vi + 2] = new Vector2(0f, 0f);
                  uv[vi + 3] = new Vector2(0f, 0f);

                  triangles[ti] = vi;
                  triangles[ti + 1] = triangles[ti + 4] = vi + 1;
                  triangles[ti + 5] = vi + 2;
                  triangles[ti + 2] = triangles[ti + 3] = vi + 3;



              }
          }



          mesh.vertices = vertices;
          mesh.uv = uv;
          mesh.triangles = triangles;

          return mesh;
      }



      public static Mesh testsimple() {
        Vector3[] vertices = new Vector3[3];
        Vector2[] uvs = new Vector2[3];
        int [] triangles = new int[3];

        vertices[0] = new Vector3(0,0);
        vertices[1] = new Vector3(0,100);
        vertices[2] = new Vector3(100,100);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        uvs[0] = new Vector2(0,0);
        uvs[0] = new Vector2(0,1);
        uvs[0] = new Vector2(1,1);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;

      }

     public static Mesh test(int quadCount) {
        Vector3[] vertices = new Vector3[4 * quadCount];
        Vector2[] uvs = new Vector2[4 * quadCount];
        int [] triangles = new int[6 * quadCount];
        
        vertices[0] = new Vector3(0,0);
        vertices[1] = new Vector3(0,100);
        vertices[2] = new Vector3(100,100);
        vertices[3] = new Vector3(100,0);

        uvs[0] = new Vector2(0,0);
        uvs[1] = new Vector2(0,1);
        uvs[2] = new Vector2(1,1);
        uvs[3] = new Vector2(1,0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;

      }

    }

}

