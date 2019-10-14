using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using CustomSaber;

namespace SaberForge
{
    class CreateQuad
    {

        //creates a demo quad for displaying trail textures
        public static GameObject CreateTrailQuad(Transform startPosMarker, Transform endPosMarker, CustomTrail trail, Color col)
        {
            GameObject newQuad = new GameObject();

            Mesh quadMesh = new Mesh();

            var vertices = new Vector3[4]

           {
                startPosMarker.position,
                endPosMarker.position,
                startPosMarker.position + new Vector3(0, -0.5f,-0.5f),
                endPosMarker.position + new Vector3(0, -0.5f,-0.5f)
           };

            quadMesh.vertices = vertices;

            var tris = new int[6]
             {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
            };

            quadMesh.triangles = tris;

            var normals = new Vector3[4]
            {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
            };

            quadMesh.normals = normals;

            var uv = new Vector2[4]
        {
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

            var vertexColors = new Color[4]
            {
                col, col,  col, col
            };

            quadMesh.colors = vertexColors;

            quadMesh.uv = uv;

            newQuad.AddComponent<MeshFilter>().mesh = quadMesh;
            newQuad.AddComponent<MeshRenderer>().material = trail.TrailMaterial;
            newQuad.transform.SetParent(startPosMarker.parent);

            return newQuad;
        }
    }
}
