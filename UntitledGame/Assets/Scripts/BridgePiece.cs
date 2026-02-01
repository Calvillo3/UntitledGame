using UnityEngine;

public class BridgePiece : MonoBehaviour
{
    public AnchorPoint[] anchorPoints;

    private void Awake()
    {
        if (anchorPoints == null || anchorPoints.Length == 0)
        {
            anchorPoints = GetComponentsInChildren<AnchorPoint>();
        }
    }

    public AnchorPoint GetClosestFreeAnchor(Vector3 position)
    {
        AnchorPoint closest = null;
        float minDistance = float.MaxValue;

        foreach (var anchor in anchorPoints)
        {
            if (anchor.IsOccupied) continue;

            float distance = Vector3.Distance(position, anchor.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = anchor;
            }
        }

        return closest;
    }
}
