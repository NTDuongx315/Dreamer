using System;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoint;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    public Player_SkillManager skillManager { get; private set; }

    [ContextMenu("Tay Tuy")]
    private void Awake()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
    }

    public void RefundAllSkills()
    {
        UI_TreeNode[] nodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in nodes) node.Refund();
    }
    public bool EnoughPoint(int cost) => skillPoint >= cost;
    public void RemoveSkillPoint(int cost) => skillPoint -= cost;
    public void AddSkillPoint(int points) => skillPoint += points;

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections(); 
        }
    }
}
