using System.Collections;
using System.Collections.Generic;
using Mlf.Utils;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    Mesh mesh;
    
    void Start()
    {
        mesh =  UtilsMesh.test(1);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
