using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;

    [SerializeField] private Collider2D touchingCol;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];

    [SerializeField]
    private bool isGrounded;
    public bool Ground
    {
        get
        {
            return isGrounded;
        }
        private set
        {
            isGrounded = value;
        }
    }

    [SerializeField]
    private bool isOnWall;

    private Vector2 WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool Wall
    {
        get
        {
            return isOnWall;
        }
        private set
        {
            isOnWall = value;
        }
    }

    private void FixedUpdate()
    {
        Ground = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        Wall = touchingCol.Cast(WallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
}