using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> Rooms = new List<Room>();
    [SerializeField] private int numRooms = 5;
    [SerializeField] private Vector2Int tilesPerRoom = new Vector2Int(5, 10);
    [SerializeField] private GameObject TilePrefab;
    private Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        Vector2Int startPos = new Vector2Int(0, 0);
        for (int i = 0; i < numRooms; i++)
        {
            Room room = new Room.RoomBuilder()
                .AddTileWithRandomWalk(startPos, Random.Range(tilesPerRoom.x, tilesPerRoom.y + 1), grid)
                .Build();
            Rooms.Add(room);
            startPos = room.tiles.Keys.Last() + new Vector2Int(1,0);
        }

        foreach(Room room in Rooms)
        {
            room.Spawn(TilePrefab);
            foreach(var tile in room.tiles)
            {
                grid.TryAdd(tile.Key, tile.Value);
            }
        }
    }
}

public class Room
{
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    private GameObject roomParent;

    public void AddTiles(Vector2Int[] positions)
    {
        foreach (var pos in positions)
        {
            tiles.Add(pos, new Tile(pos));
        }
    }

    public void Spawn(GameObject prefab)
    {
        roomParent = new GameObject("Room");
        foreach (var vk in tiles)
        {
            vk.Value.Spawn(prefab, roomParent);
        }
    }

    public class RoomBuilder
    {
        public Room roomInstance;

        public RoomBuilder() { roomInstance = new Room(); }

        public Room Build()
        {
            return roomInstance;
        }

        public RoomBuilder AddTileWithRandomWalk(Vector2Int globalStartPos, int numSteps, Dictionary<Vector2Int, Tile> grid)
        {
            var directions = new List<Vector2Int>()
            {
                new Vector2Int(1,0),
                new Vector2Int(-1,0),
                new Vector2Int(0,1),
                new Vector2Int(0,-1),
            };
            HashSet<Vector2Int> newPositions = new HashSet<Vector2Int>();
            for (int i = 0; i < numSteps; i++)
            {
                directions.Shuffle();
                bool tileSet = false;
                foreach (var direction in directions)
                {
                    var newPos = globalStartPos + direction;
                    if (grid.ContainsKey(newPos)) { continue; }
                    tileSet = true;
                    newPositions.Add(newPos);
                    globalStartPos = newPos;
                    break;
                }
                if (!tileSet)
                {
                    i--;
                }
            }
            roomInstance.AddTiles(newPositions.ToArray());
            return this;
        }
    }


}

public class Tile
{
    public Vector2Int position;
    private GameObject spawnedObject;

    public Tile(Vector2Int position)
    {
        this.position = position;
    }

    public void Spawn(GameObject prefab, GameObject parent)
    {
        spawnedObject = MonoBehaviour.Instantiate(prefab, position.ToVector3(0), Quaternion.identity, parent.transform);
    }

    public void Despawn()
    {
        GameObject.Destroy(spawnedObject.gameObject);
    }
}

public static class VectorExtension
{
    public static Vector3 ToVector3(this Vector2Int vec, float y = 0)
    {
        return new Vector3(vec.x, y, vec.y);
    }
}

public static class Utility
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T el = list[i];
            int rnd = Random.Range(i, list.Count - 1);
            list[i] = list[rnd];
            list[rnd] = el;
        }
    }
}