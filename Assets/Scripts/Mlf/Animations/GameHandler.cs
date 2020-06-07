using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

namespace Mlf.Sm {

    public class GameHandler : MonoBehaviour {

        public Material material;
        
        private Vector2[] headDownUV;
        private Vector2[] headUpUV;
        private Vector2[] headLeftUV;
        private Vector2[] headRightUV;
        
        private Vector2[] bodyDownUV;
        private Vector2[] bodyUpUV;
        private Vector2[] bodyLeftUV;
        private Vector2[] bodyRightUV;
        
        private Vector2[] swordUV;
        private Vector2[] shieldUV;
        
        private Vector2[] handUV;
        private Vector2[] footUV;


        private void Start() {
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector3(0, 1);
            vertices[1] = new Vector3(1, 1);
            vertices[2] = new Vector3(0, 0);
            vertices[3] = new Vector3(1, 0);

            headDownUV = GetUVRectangleFromPixels(0, 384, 128, 128, 512, 512);
            headUpUV = GetUVRectangleFromPixels(256, 384, 128, 128, 512, 512);
            headLeftUV = GetUVRectangleFromPixels(256, 384, -128, 128, 512, 512);
            headRightUV = GetUVRectangleFromPixels(128, 384, 128, 128, 512, 512);
            
            bodyDownUV = GetUVRectangleFromPixels(0, 256, 128, 128, 512, 512);
            bodyUpUV = GetUVRectangleFromPixels(256, 256, 128, 128, 512, 512);
            bodyLeftUV = GetUVRectangleFromPixels(256, 256, -128, 128, 512, 512);
            bodyRightUV = GetUVRectangleFromPixels(128, 256, 128, 128, 512, 512);
            
            swordUV = GetUVRectangleFromPixels(0, 128, 128, 128, 512, 512);
            shieldUV = GetUVRectangleFromPixels(128, 128, 128, 128, 512, 512);
            
            handUV = GetUVRectangleFromPixels(384, 448, 64, 64, 512, 512);
            footUV = GetUVRectangleFromPixels(448, 448, 64, 64, 512, 512);

            ApplyUVToUVArray(headLeftUV, ref uv);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;

            Mesh mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
            gameObject.transform.localScale = new Vector3(30, 30, 1);

            gameObject.GetComponent<MeshFilter>().mesh = mesh;

            gameObject.GetComponent<MeshRenderer>().material = material;
            
            
            CMDebug.ButtonUI(new Vector2(-500, 310), "Head Down",   () => { ApplyUVToUVArray(headDownUV,    ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-500, 250), "Head Up",     () => { ApplyUVToUVArray(headUpUV,      ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-500, 190), "Head Left",   () => { ApplyUVToUVArray(headLeftUV,    ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-500, 130), "Head Right",  () => { ApplyUVToUVArray(headRightUV,   ref uv); mesh.uv = uv; });
            
            CMDebug.ButtonUI(new Vector2(-350, 310), "Body Down",   () => { ApplyUVToUVArray(bodyDownUV,    ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-350, 250), "Body Up",     () => { ApplyUVToUVArray(bodyUpUV,      ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-350, 190), "Body Left",   () => { ApplyUVToUVArray(bodyLeftUV,    ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-350, 130), "Body Right",  () => { ApplyUVToUVArray(bodyRightUV,   ref uv); mesh.uv = uv; });
            
            CMDebug.ButtonUI(new Vector2(-500, 50),  "Sword",       () => { ApplyUVToUVArray(swordUV,       ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-500, -10), "Shield",      () => { ApplyUVToUVArray(shieldUV,      ref uv); mesh.uv = uv; });

            CMDebug.ButtonUI(new Vector2(-350, 50),  "Hand",        () => { ApplyUVToUVArray(handUV,        ref uv); mesh.uv = uv; });
            CMDebug.ButtonUI(new Vector2(-350, -10), "Foot",        () => { ApplyUVToUVArray(footUV,        ref uv); mesh.uv = uv; });
        }

        private Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight) {
            return new Vector2((float)x / textureWidth, (float)y / textureHeight);
        }

        private Vector2[] GetUVRectangleFromPixels(int x, int y, int width, int height, int textureWidth, int textureHeight) {
            /* 0, 1
             * 1, 1
             * 0, 0
             * 1, 0
             * */
            return new Vector2[] { 
                ConvertPixelsToUVCoordinates(x, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x, y, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y, textureWidth, textureHeight)
            };
        }

        private void ApplyUVToUVArray(Vector2[] uv, ref Vector2[] mainUV) {
            if (uv == null || uv.Length < 4 || mainUV == null || mainUV.Length < 4) throw new System.Exception();
            mainUV[0] = uv[0];
            mainUV[1] = uv[1];
            mainUV[2] = uv[2];
            mainUV[3] = uv[3];
        }
    }

}