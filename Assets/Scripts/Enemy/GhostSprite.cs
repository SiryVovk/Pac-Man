using UnityEngine;

public class GhostSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ghostRenderer;

    public void SetGhostVisibility(bool isVisible)
    {
        ghostRenderer.enabled = isVisible;
    }
}
