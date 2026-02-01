using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    public bool IsOccupied { get; private set; }
    public AnchorPoint ConnectedAnchor { get; private set; }

    public void Attach(AnchorPoint other)
    {
        IsOccupied = true;
        ConnectedAnchor = other;
    }

    public void Detach()
    {
        IsOccupied = false;
        ConnectedAnchor = null;
    }
}
