using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material material;
    public float radius = 2f;
    public int latitudeSegments = 12;
    public int longitudeSegments = 12;
    public Vector3 spherePos;

    private void OnPostRender()
    {
        DrawSphere3D();
    }

    private void DrawSphere3D()
    {
        latitudeSegments = Mathf.Max(6, latitudeSegments);
        longitudeSegments = Mathf.Max(6, longitudeSegments);

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float theta = Mathf.PI * lat / latitudeSegments;
            float y = Mathf.Cos(theta) * radius;
            float ringRadius = Mathf.Sin(theta) * radius;

            Vector3[] ringPoints = new Vector3[longitudeSegments + 1];

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = (2 * Mathf.PI / longitudeSegments) * lon;
                float x = ringRadius * Mathf.Cos(phi);
                float z = ringRadius * Mathf.Sin(phi);

                ringPoints[lon] = new Vector3(x, y, z) + spherePos;
            }

            for (int i = 0; i < longitudeSegments; i++)
            {
                Vector2 p1 = ProjectPoint(ringPoints[i]);
                Vector2 p2 = ProjectPoint(ringPoints[i + 1]);
                DrawLine(p1, p2);
            }
        }

        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            float phi = (2 * Mathf.PI / longitudeSegments) * lon;
            Vector3[] longPoints = new Vector3[latitudeSegments + 1];

            for (int lat = 0; lat <= latitudeSegments; lat++)
            {
                float theta = Mathf.PI * lat / latitudeSegments;
                float x = Mathf.Sin(theta) * Mathf.Cos(phi) * radius;
                float y = Mathf.Cos(theta) * radius;
                float z = Mathf.Sin(theta) * Mathf.Sin(phi) * radius;

                longPoints[lat] = new Vector3(x, y, z) + spherePos;
            }

            for (int i = 0; i < latitudeSegments; i++)
            {
                Vector2 p1 = ProjectPoint(longPoints[i]);
                Vector2 p2 = ProjectPoint(longPoints[i + 1]);
                DrawLine(p1, p2);
            }
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
