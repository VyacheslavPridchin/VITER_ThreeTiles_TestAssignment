using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PocketGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject pocketPrefab;
    [SerializeField]
    private SpriteRenderer spawnZone;
    [field: SerializeField]
    public Vector2Int GridSize { get; set; }
    [HideInInspector]
    public UnityEvent PocketGridChanged = new UnityEvent();
    public PocketController[,] pockets;

    // Spawn Grid of Pockets
    void Awake()
    {
        pockets = new PocketController[GridSize.x, GridSize.y];

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                GameObject go = Instantiate(pocketPrefab);
                go.transform.parent = transform;

                PocketController pocketController = go.GetComponentInChildren<PocketController>();
                pocketController.GraySquareChanged.AddListener(() =>
                {
                    PocketGridChanged?.Invoke();
                });

                Vector2 newPosition = new Vector2();
                newPosition.x = transform.position.x - spawnZone.size.x / 2;
                newPosition.y = transform.position.y + spawnZone.size.y / 2;
                newPosition.x += spawnZone.size.x / GridSize.x / 2 + (spawnZone.size.x / GridSize.x * x);
                newPosition.y -= spawnZone.size.y / GridSize.y / 2 - (spawnZone.size.y / GridSize.y * -y);

                go.transform.position = newPosition;
                pockets[x, y] = pocketController;
            }
        }
    }
}
