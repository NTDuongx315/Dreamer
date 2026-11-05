using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public Direction direction;
    [Range(100f, 300f)] public float length;
    [Range(-50f, 50f)] public float rotation;

}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnection[] treeConnections; //how many connects does its root will have to connect others
    [SerializeField] private UI_TreeConnectDetails[] details; //direction and length of the connect to others.

    private Image ConnectionImage;
    private Color originalColor;

    private void Awake()
    {
        if(ConnectionImage != null) originalColor = ConnectionImage.color;
    }

    private void OnValidate()
    {
        if(details.Length <= 0) return;
        
        if (details.Length != treeConnections.Length)
        {
            Debug.Log("amount of details should be same as amount of connection" + gameObject.name);
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
            //Assigned the Image from ConnectionLine object to the  var ConnectionImage
            Image ConnectionImage = connection.GetConnectionImage(); 
           
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null) continue;
            detail.childNode?.SetPosition(targetPosition);
            detail.childNode?.SetConnectionImage(ConnectionImage);
            detail.childNode?.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();
        foreach (var node in details)
        {
            if (node.childNode == null) continue;
            node.childNode?.UpdateConnections();
        }
    }

    public void ConnectionImageUnlocked(bool unlocked)
    {
        if (ConnectionImage == null) return;
        ConnectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image) => ConnectionImage = image;
    
    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;
}
