using UnityEngine;

[CreateAssetMenu(fileName = "GhostSO", menuName = "Scriptable Objects/GhostSO")]
public class GhostSO : ScriptableObject
{
    public GameObject GhostObject;

    public GhostStratagySO GhostStratagy;
}
