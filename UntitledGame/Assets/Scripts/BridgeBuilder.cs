using UnityEngine;
using UnityEngine.InputSystem;

public class BridgeBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public BridgePiece bridgePiecePrefab;

    [Header("Placement")]
    public Transform spawnPoint;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            SpawnBridgePiece();
        }
    }

    public BridgePiece SpawnBridgePiece()
    {
        BridgePiece piece = Instantiate(
            bridgePiecePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        return piece;
    }
}
