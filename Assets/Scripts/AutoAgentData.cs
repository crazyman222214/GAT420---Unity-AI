using UnityEngine;

[CreateAssetMenu(fileName = "AutoAgentData", menuName = "Scriptable Objects/AutoAgentData")]
public class AutoAgentData : ScriptableObject
{
    [Range(5, 40)] public float displacement;
    [Range(5, 40)] public float distance;
    [Range(5, 40)] public float radius;
}
