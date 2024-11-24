using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IMDM227
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [DisallowMultipleComponent]
    public class MeshLine4
    {
        public Mesh Mesh { get; private set; }
        public TextMeshPro tmp;

        public Color32 Color32
        {
            get
            {
                return color;
            }

            set
            {
                color = value;

                edgeColor = color;
                edgeColor.a = 0;
            }
        }

        Mesh mesh;

        int vertMax = ushort.MaxValue - 1;
        int triMax = 4 * ushort.MaxValue; // Guestimate based on 12 tris / 10 vertices.

        string name;
        List<Vector2> points;
        Color32 color;
        Color32 edgeColor;
        float width;
        float edge;
        Joint joint;
        float extension;
        bool isDiscrete;

        int numPoints;

        Vector3[] verts;
        Vector3[] norms;
        Color32[] colors;
        int[] tris;

        int vertIndex;
        int triIndex;

        Vector2[] dw;
        Vector2[] de;

        MeshFilter meshFilter;
        public MeshLine4(
            GameObject gameObject,
            string name,
            Color32 color,
            float width = 1.5f,
            float edge = 1,
            Joint joint = Joint.None,
            float extension = 0
        )
        {
            this.name = name;
            this.color = color;
            this.width = width;
            this.edge = edge;
            this.joint = joint;
            this.extension = extension;

            meshFilter = gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            var rend = gameObject.GetComponent<Renderer>();
            rend.material = Resources.Load<Material>("Materials/MyLine");

            Camera.main.orthographicSize = Screen.height / 2f;
            Camera.main.orthographic = true;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
            Camera.main.transform.position = new Vector3(0, 0, -10);

            gameObject.transform.position = new Vector3(0, 0, 0);

            edgeColor = new Color32(color.r, color.g, color.b, 0);

            dw = new Vector2[4];
            de = new Vector2[4];

            mesh = new Mesh();
            mesh.name = name;

            Reset();
        }

        public void Line(float xf, float yf, float xt, float yt)
        {
            Vector2 f;
            Vector2 t;

            f.x = xf;
            f.y = yf;

            t.x = xt;
            t.y = yt;

            Line(f, t);
        }

        public void Line(Vector2 from, Vector2 to)
        {
            GetOffsets(from, to, dw, de);
            AddVerts(from, to, dw, de);
            AddSegmentTris();
        }

        public void Display()
        {
            Array.Resize(ref verts, vertIndex);
            Array.Resize(ref colors, vertIndex);
            Array.Resize(ref tris, triIndex);

            mesh.vertices = verts;
            mesh.colors32 = colors;
            mesh.triangles = tris;

            Reset();

            Mesh = mesh;
            meshFilter.mesh = mesh;
        }

        void GetOffsets(Vector2 p0, Vector2 p1, Vector2[] dw, Vector2[] de)
        {
            float dx = p1.x - p0.x;
            float dy = p1.y - p0.y;

            float length = Mathf.Sqrt(dx * dx + dy * dy);
            float widthScale = width / length;
            float edgeScale = edge / length;

            float wx = widthScale * dx;
            float wy = widthScale * dy;

            float ex = edgeScale * dx;
            float ey = edgeScale * dy;

            dw[0] = new Vector2(-wy, wx);
            dw[1] = new Vector2(-wy, wx);
            dw[2] = new Vector2(wy, -wx);
            dw[3] = new Vector2(wy, -wx);

            de[0] = new Vector2(-wy - ey - ex, wx + ex - ey);
            de[1] = new Vector2(-wy - ey + ex, wx + ex + ey);
            de[2] = new Vector2(wy + ey + ex, -wx - ex + ey);
            de[3] = new Vector2(wy + ey - ex, -wx - ex - ey);
        }

        void AddVerts(Vector2 p0, Vector2 p1, Vector2[] dw, Vector2[] de)
        {
            verts[vertIndex + 0] = p0 + de[0];
            verts[vertIndex + 1] = p0 + dw[0];
            verts[vertIndex + 2] = p0;
            verts[vertIndex + 3] = p0 + dw[3];
            verts[vertIndex + 4] = p0 + de[3];
            verts[vertIndex + 5] = p1 + de[1];
            verts[vertIndex + 6] = p1 + dw[1];
            verts[vertIndex + 7] = p1;
            verts[vertIndex + 8] = p1 + dw[2];
            verts[vertIndex + 9] = p1 + de[2];

            if (tmp != null)
            {
                tmp.text = $"{Camera.main.orthographicSize}, {Screen.width} x {Screen.height}\n";

                for (int v = 0; v < 10; ++v)
                {
                    tmp.text += $"{v}: {verts[vertIndex + v].x}, {verts[vertIndex + v].y}\n";
                }
            }

            colors[vertIndex + 0] = edgeColor;
            colors[vertIndex + 1] = color;
            colors[vertIndex + 2] = color;
            colors[vertIndex + 3] = color;
            colors[vertIndex + 4] = edgeColor;
            colors[vertIndex + 5] = edgeColor;
            colors[vertIndex + 6] = color;
            colors[vertIndex + 7] = color;
            colors[vertIndex + 8] = color;
            colors[vertIndex + 9] = edgeColor;

            vertIndex = vertIndex + 10;
        }

        void AddSegmentTris()
        {
            int v9 = vertIndex - 1;
            int v8 = v9 - 1;
            int v7 = v8 - 1;
            int v6 = v7 - 1;
            int v5 = v6 - 1;
            int v4 = v5 - 1;
            int v3 = v4 - 1;
            int v2 = v3 - 1;
            int v1 = v2 - 1;
            int v0 = v1 - 1;

            AddTri(v0, v5, v1);
            AddTri(v1, v5, v6);
            AddTri(v1, v6, v2);
            AddTri(v2, v6, v7);
            AddTri(v2, v7, v3);
            AddTri(v3, v7, v8);
            AddTri(v3, v8, v4);
            AddTri(v4, v8, v9);
            AddTri(v0, v1, v3);
            AddTri(v0, v3, v4);
            AddTri(v5, v9, v6);
            AddTri(v6, v9, v8);
        }

        void AddTri(int v0, int v1, int v2)
        {
            tris[triIndex + 0] = v0;
            tris[triIndex + 1] = v1;
            tris[triIndex + 2] = v2;

            triIndex = triIndex + 3;
        }

        void Reset()
        {
            verts = new Vector3[vertMax];
            colors = new Color32[vertMax];
            tris = new int[triMax]; // 3 vertexes per triangle.

            vertIndex = 0;
            triIndex = 0;
        }
    }
}

