using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 3f;
    public int segments = 12;
    public Vector3 cylinderPos;

    private void OnPostRender()
    {
        DrawCylinder3D();
    }

    private void DrawCylinder3D()
    {
        if (segments < 6) segments = 6;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3[] bottomPoints = new Vector3[segments];
        Vector3[] topPoints = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = (2 * Mathf.PI / segments) * i;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            bottomPoints[i] = new Vector3(x, 0, z) + cylinderPos;
            topPoints[i] = new Vector3(x, height, z) + cylinderPos;
        }

        Vector2[] bottom2D = new Vector2[segments];
        Vector2[] top2D = new Vector2[segments];

        for (int i = 0; i < segments; i++)
        {
            bottom2D[i] = ProjectPoint(bottomPoints[i]);
            top2D[i] = ProjectPoint(topPoints[i]);
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            DrawLine(bottom2D[i], bottom2D[next]);

            DrawLine(top2D[i], top2D[next]);

            DrawLine(bottom2D[i], top2D[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector2 ProjectPoint(Vector3 point)
    {
        float perspective = PerspectiveCamera.Instance.GetPerspective(point.z);
        return new Vector2(point.x * perspective, point.y * perspective);
    }

    private void DrawLine(Vector2 p1, Vector2 p2)
    {
        GL.Vertex3(p1.x, p1.y, 0);
        GL.Vertex3(p2.x, p2.y, 0);
    }
}
