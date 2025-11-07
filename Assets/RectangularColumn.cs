using UnityEngine;

public class RectangularColumn : MonoBehaviour
{
    public Material material;
    public float columnSize = 1f;
    public float columnHeight = 5f;
    public Vector3 columnPos;

    private void OnPostRender()
    {
        DrawColumn3D();
    }

    private void DrawColumn3D()
    {
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3 A = new Vector3(-1, 0, -1) * columnSize + columnPos;
        Vector3 B = new Vector3(1, 0, -1) * columnSize + columnPos;
        Vector3 C = new Vector3(1, 0, 1) * columnSize + columnPos;
        Vector3 D = new Vector3(-1, 0, 1) * columnSize + columnPos;

        Vector3 E = new Vector3(-1, columnHeight, -1) * columnSize + columnPos;
        Vector3 F = new Vector3(1, columnHeight, -1) * columnSize + columnPos;
        Vector3 G = new Vector3(1, columnHeight, 1) * columnSize + columnPos;
        Vector3 H = new Vector3(-1, columnHeight, 1) * columnSize + columnPos;

        Vector2 a2D = ProjectPoint(A);
        Vector2 b2D = ProjectPoint(B);
        Vector2 c2D = ProjectPoint(C);
        Vector2 d2D = ProjectPoint(D);
        Vector2 e2D = ProjectPoint(E);
        Vector2 f2D = ProjectPoint(F);
        Vector2 g2D = ProjectPoint(G);
        Vector2 h2D = ProjectPoint(H);

        DrawLine(a2D, b2D);
        DrawLine(b2D, c2D);
        DrawLine(c2D, d2D);
        DrawLine(d2D, a2D);

        DrawLine(e2D, f2D);
        DrawLine(f2D, g2D);
        DrawLine(g2D, h2D);
        DrawLine(h2D, e2D);

        DrawLine(a2D, e2D);
        DrawLine(b2D, f2D);
        DrawLine(c2D, g2D);
        DrawLine(d2D, h2D);

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
