using UnityEngine;

public interface ICounterAble
{
    public bool CanBeParry { get; }
    public void HandleCounter();
}
