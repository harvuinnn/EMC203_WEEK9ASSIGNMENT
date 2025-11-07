using UnityEngine;

public class Capsule : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 3f;
    public int latitudeSegments = 12;
    public int longitudeSegments = 12;
    public Vector3 capsulePos;

    private void OnPostRender()
    {
        DrawCapsule3D();
    }

    private void DrawCapsule3D()
    {
        latitudeSegments = Mathf.Max(6, latitudeSegments);
        longitudeSegments = Mathf.Max(6, longitudeSegments);

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        float halfBody = height * 0.5f;

        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            float phi = (2 * Mathf.PI / longitudeSegments) * lon;
            float nextPhi = (2 * Mathf.PI / longitudeSegments) * (lon + 1);

            Vector3 p1 = new Vector3(Mathf.Cos(phi) * radius, -halfBody, Mathf.Sin(phi) * radius) + capsulePos;
            Vector3 p2 = new Vector3(Mathf.Cos(phi) * radius, halfBody, Mathf.Sin(phi) * radius) + capsulePos;
            Vector3 p3 = new Vector3(Mathf.Cos(nextPhi) * radius, -halfBody, Mathf.Sin(nextPhi) * radius) + capsulePos;
            Vector3 p4 = new Vector3(Mathf.Cos(nextPhi) * radius, halfBody, Mathf.Sin(nextPhi) * radius) + capsulePos;

            DrawLine(ProjectPoint(p1), ProjectPoint(p2));

            DrawLine(ProjectPoint(p1), ProjectPoint(p3));
            DrawLine(ProjectPoint(p2), ProjectPoint(p4));
        }

        for (int lat = 0; lat <= latitudeSegments / 2; lat++)
        {
            float theta = Mathf.PI * lat / latitudeSegments / 2;
            float y = Mathf.Sin(theta) * radius;
            float ringRadius = Mathf.Cos(theta) * radius;

            Vector3[] ringPoints = new Vector3[longitudeSegments + 1];
            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = (2 * Mathf.PI / longitudeSegments) * lon;
                float x = ringRadius * Mathf.Cos(phi);
                float z = ringRadius * Mathf.Sin(phi);
                ringPoints[lon] = new Vector3(x, y + halfBody, z) + capsulePos;
            }

            for (int i = 0; i < longitudeSegments; i++)
            {
                DrawLine(ProjectPoint(ringPoints[i]), ProjectPoint(ringPoints[i + 1]));
            }
        }

        for (int lat = 0; lat <= latitudeSegments / 2; lat++)
        {
            float theta = Mathf.PI * lat / latitudeSegments / 2;
            float y = Mathf.Sin(theta) * radius;
            float ringRadius = Mathf.Cos(theta) * radius;

            Vector3[] ringPoints = new Vector3[longitudeSegments + 1];
            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = (2 * Mathf.PI / longitudeSegments) * lon;
                float x = ringRadius * Mathf.Cos(phi);
                float z = ringRadius * Mathf.Sin(phi);
                ringPoints[lon] = new Vector3(x, -y - halfBody, z) + capsulePos;
            }

            for (int i = 0; i < longitudeSegments; i++)
            {
                DrawLine(ProjectPoint(ringPoints[i]), ProjectPoint(ringPoints[i + 1]));
            }
        }

        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            float phi = (2 * Mathf.PI / longitudeSegments) * lon;
            Vector3[] topCurve = new Vector3[latitudeSegments / 2 + 1];
            Vector3[] bottomCurve = new Vector3[latitudeSegments / 2 + 1];

            for (int lat = 0; lat <= latitudeSegments / 2; lat++)
            {
                float theta = Mathf.PI * lat / latitudeSegments / 2;
                float y = Mathf.Sin(theta) * radius;
                float r = Mathf.Cos(theta) * radius;

                topCurve[lat] = new Vector3(r * Mathf.Cos(phi), y + halfBody, r * Mathf.Sin(phi)) + capsulePos;
                bottomCurve[lat] = new Vector3(r * Mathf.Cos(phi), -y - halfBody, r * Mathf.Sin(phi)) + capsulePos;
            }

            for (int i = 0; i < topCurve.Length - 1; i++)
            {
                DrawLine(ProjectPoint(topCurve[i]), ProjectPoint(topCurve[i + 1]));
                DrawLine(ProjectPoint(bottomCurve[i]), ProjectPoint(bottomCurve[i + 1]));
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