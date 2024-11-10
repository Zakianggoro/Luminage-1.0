using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackRange", menuName = "Operator/AttackRange")]
public class AttackRange : ScriptableObject
{
    public enum RangeShape { Circle, Rectangle, Line, Custom }

    [Header("Shape")]
    public RangeShape rangeShape;

    [Header("Circle")]
    public float radius = 1f;

    [Header("Rectangle")]
    public Vector2 rectangleSize = new Vector2(2, 1); // Width and height for Rectangle

    [Header("Straight Line")]
    public float lineLength = 3f;

    [Header("Custom Shape")]
    public Vector2[] customPoints; // For custom shapes, define the area with points

    // Draw Gizmos for 2D visualization
    public void DrawRangeGizmo(Vector2 centerPosition, Vector2? direction = null)
    {
        Gizmos.color = Color.green;

        switch (rangeShape)
        {
            case RangeShape.Circle:
                Gizmos.DrawWireSphere(centerPosition, radius);
                break;
            case RangeShape.Rectangle:
                // For rectangle, apply the direction (rotation)
                Quaternion rotation = Quaternion.Euler(0, 0, GetRotationAngle(direction));
                Gizmos.matrix = Matrix4x4.TRS(centerPosition, rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, rectangleSize);
                Gizmos.matrix = Matrix4x4.identity; // Reset Gizmos matrix
                break;
            case RangeShape.Line:
                Vector2 lineDirection = direction.HasValue ? direction.Value.normalized : Vector2.up; // Default to up
                Gizmos.DrawLine(centerPosition, centerPosition + lineDirection * lineLength);
                break;
            case RangeShape.Custom:
                if (customPoints != null && customPoints.Length > 1)
                {
                    for (int i = 0; i < customPoints.Length - 1; i++)
                    {
                        Gizmos.DrawLine(centerPosition + customPoints[i], centerPosition + customPoints[i + 1]);
                    }
                }
                break;
        }
    }

    // Use 2D raycasting to calculate the area covered by the range, now with direction
    public List<Collider2D> GetTargetsInRange(Vector2 centerPosition, LayerMask targetLayer, Vector2? direction = null)
    {
        List<Collider2D> targetsInRange = new List<Collider2D>();
        Quaternion rotation = Quaternion.identity;

        if (direction.HasValue)
        {
            rotation = Quaternion.Euler(0, 0, GetRotationAngle(direction));
        }

        switch (rangeShape)
        {
            case RangeShape.Circle:
                targetsInRange.AddRange(Physics2D.OverlapCircleAll(centerPosition, radius, targetLayer));
                break;

            case RangeShape.Rectangle:
                // Applying rotation
                Collider2D[] hits = Physics2D.OverlapBoxAll(centerPosition, rectangleSize, rotation.eulerAngles.z, targetLayer);
                Debug.Log("Rectangle Range Rotation: " + rotation.eulerAngles.z);
                targetsInRange.AddRange(hits);
                break;

            case RangeShape.Line:
                Vector2 lineDirection = direction.HasValue ? direction.Value.normalized : Vector2.up; // Default to up
                RaycastHit2D[] lineHits = Physics2D.RaycastAll(centerPosition, lineDirection, lineLength, targetLayer);
                Debug.Log("Line Range Direction: " + lineDirection);
                foreach (var hit in lineHits)
                {
                    targetsInRange.Add(hit.collider);
                }
                break;

            case RangeShape.Custom:
                // Implement your custom shape handling here
                break;
        }

        return targetsInRange;
    }


    // Helper method to calculate the angle of rotation based on a direction
    private float GetRotationAngle(Vector2? direction)
    {
        if (!direction.HasValue)
            return 0f; // Default angle for no direction

        Vector2 dir = direction.Value.normalized;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // Convert direction to angle in degrees

    }
}
