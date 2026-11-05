using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;
    public UI_SkillTree skillTree;
    private bool skillTreeEnable;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ToggleSkillTree()
    {
        skillTreeEnable = !skillTreeEnable;
        skillTree.gameObject.SetActive(skillTreeEnable);
        skillToolTip.ShowTooltip(false,null);
    }
}
