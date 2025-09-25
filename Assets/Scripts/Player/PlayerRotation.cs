using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;

    private PlayerMovement playerMovement;
    private SpriteRenderer visualSprite;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        visualSprite = visualTransform.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerMovement.OnDirectionChange += ChangeRotation;
    }

    private void OnDisable()
    {
        playerMovement.OnDirectionChange += ChangeRotation;
    }

    private void ChangeRotation(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            visualTransform.rotation = Quaternion.Euler(0f, 0f, 90f);
            visualSprite.flipX = false;
            visualSprite.flipY = false;
        }
        else if (direction == Vector2.down)
        {
            visualTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
            visualSprite.flipX = false;
            visualSprite.flipY = false;
        }
        else if (direction == Vector2.left)
        {
            visualTransform.rotation = Quaternion.identity;
            visualSprite.flipX = true;
            visualSprite.flipY = false;
        }
        else if (direction == Vector2.right)
        {
            visualTransform.rotation = Quaternion.identity;
            visualSprite.flipX = false;
            visualSprite.flipY = false;
        }
    }
}
