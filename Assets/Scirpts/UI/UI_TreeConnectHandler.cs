using System;
using UnityEngine;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public Direction direction;
    [Range(100f, 300f)] public float length;
    
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect;
    [SerializeField] private UI_TreeConnection[] treeConnections; //how many connects does its root will have to connect others
    [SerializeField] private UI_TreeConnectDetails[] details; //direction and length of the connect to others.

    private void OnValidate()
    {
        if(myRect == null) myRect = GetComponent<RectTransform>();
        if (details.Length != treeConnections.Length)
        {
            Debug.Log("amount of details should be same as amount of connection");
            return;
        }
        
        UpdateConnections();
    }
    
    private void UpdateConnections()
    {
        for (int i = 0; i < details.Length; i++)
        {
           var detail = details[i];
           var connection = treeConnections[i];
           Vector2 targetPosition = connection.GetConnectionPoint(myRect);
           
           connection.DirectConnection(detail.direction, detail.length);
           detail.childNode.SetPosition(targetPosition);
        }
    }
    
    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;
}
