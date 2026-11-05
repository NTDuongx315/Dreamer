using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    DownLeft,
    DownRight,
    UpLeft,
    UpRight,
    None
}

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(Direction dir, float length, float offset)
    {
        bool shoulBeActive = dir != Direction.None;
        float finalLength = shoulBeActive ? length : 0f;
        float angle = GetDirectionAngle(dir);

        rotationPoint.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }
    
    public Image GetConnectionImage() => connectionLength.GetComponent<Image>(); 
    //use connectionLength because connectionLength is assigned with ConnectionLine object.
    //and inside ConnectionLine object contains Image and Color of the line only which is what we need to modify.

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out var localPointion
        );
        return localPointion;
    }
    
    private float GetDirectionAngle(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return 90f;
            case Direction.Down: return -90f;
            case Direction.DownLeft: return -135f;
            case Direction.DownRight: return -45f;
            case Direction.UpLeft: return 135f;
            case Direction.UpRight: return 45f;
            case Direction.Left: return 180f;
            case Direction.Right: return 0f;
            default: return 0f;
        }
    }
}

