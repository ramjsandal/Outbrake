using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }
    public class TileInfo
    {
        public bool traversable;

        public TileInfo(bool traversable)
        {
            this.traversable = traversable;
        }
    }

    public Tilemap traversable;
    public Tilemap notTraversable;
    public Tilemap road;
    public Dictionary<Vector2Int, TileInfo> map;
    public Dictionary<NodeInfo, NodeInfo> pathsToPlayer;

    private Player player;


    // used in dijkstras
    private SortedSet<NodeInfo> toSearch;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            traversable.CompressBounds();
            notTraversable.CompressBounds();
            map = new Dictionary<Vector2Int, TileInfo>();
            pathsToPlayer = new Dictionary<NodeInfo, NodeInfo>();
            player = FindObjectOfType<Player>();
            CreateGrid();
            prevPosition = GetCellPosition(player.transform.position);
            toSearch = new SortedSet<NodeInfo>();
        }
    }

    Vector2Int prevPosition;
    private void Update()
    {
        if (GetCellPosition(player.transform.position) != prevPosition)
        {
            pathsToPlayer.Clear();
            Dijkstras(ref pathsToPlayer, GetCellPosition(player.transform.position), 60);
        }
    }

    public void CreateGrid()
    {
        for (int x = traversable.cellBounds.xMin; x < traversable.cellBounds.xMax; x++)
        {
            for (int y = traversable.cellBounds.yMin; y < traversable.cellBounds.yMax; y++)
            {
                Vector3 worldPosition = traversable.CellToWorld(new Vector3Int(x, y, 0));
                if (notTraversable.HasTile(notTraversable.WorldToCell(worldPosition)))
                {
                    map.Add(new Vector2Int(x, y), new TileInfo(false));
                }
                else
                {
                    map.Add(new Vector2Int(x, y), new TileInfo(true));
                }
            }
        }

    }
    public Vector2 GetTileCenter(Vector2Int gridPos)
    {
        TileInfo tile;
        bool exists = map.TryGetValue(gridPos, out tile);
        Vector3Int posn = new Vector3Int(gridPos.x, gridPos.y, 0);

        if (!exists)
        {
            throw new ArgumentException("tile does not exist on grid");
        }

        if (tile.traversable)
        {
            return (Vector2)traversable.GetCellCenterWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
        }
        else
        {
            return (Vector2)notTraversable.GetCellCenterWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
        }
    }

    private void TintTile(Vector2Int gridPos, Color color)
    {
        TileInfo tile;
        bool exists = map.TryGetValue(gridPos, out tile);
        Vector3Int posn = new Vector3Int(gridPos.x, gridPos.y, 0);

        if (!exists)
        {
            throw new ArgumentException("tile does not exist on grid");
        }

        if (tile.traversable)
        {
            traversable.SetTileFlags(posn, TileFlags.None);
            traversable.SetColor(posn, color);
        }
        else
        {
            traversable.SetTileFlags(posn, TileFlags.None);
            notTraversable.SetColor(posn, color);
        }

    }

    public void TintTiles(List<Vector2Int> tiles, Color color)
    {
        foreach (var tile in map)
        {
            if (tiles.Contains(tile.Key))
            {
                TintTile(tile.Key, color);
            }
            else
            {
                TintTile(tile.Key, Color.white);
            }
        }
    }

    public void NoTint()
    {
        foreach (var tile in map)
        {
            TintTile(tile.Key, Color.white);
        }
    }

    public Vector2Int GetCellPosition(Vector3 worldPos)
    {
        Vector3Int pos3 = traversable.WorldToCell(worldPos);
        Vector2Int pos = new(pos3.x, pos3.y);
        return pos;
    }

    public Vector3 GetWorldPosition(Vector2Int cellPosition)
    {
        return traversable.GetCellCenterWorld(new Vector3Int(cellPosition.x, cellPosition.y));
    }

    public Vector2Int MouseToGrid()
    {
        return GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (map.ContainsKey(new Vector2Int(position.x - 1, position.y)))
        {
            neighbors.Add(new Vector2Int(position.x - 1, position.y));
        }

        if (map.ContainsKey(new Vector2Int(position.x + 1, position.y)))
        {
            neighbors.Add(new Vector2Int(position.x + 1, position.y));
        }

        if (map.ContainsKey(new Vector2Int(position.x, position.y - 1)))
        {
            neighbors.Add(new Vector2Int(position.x, position.y - 1));
        }

        if (map.ContainsKey(new Vector2Int(position.x, position.y + 1)))
        {
            neighbors.Add(new Vector2Int(position.x, position.y + 1));
        }

        return neighbors;

    }
    public class NodeInfo : IComparable<NodeInfo>
    {
        // which position on the map does this correspond to
        public Vector2Int position;

        // parent node
        public NodeInfo parent;

        // distance from origin in moves
        public int distance;

        public int CompareTo(NodeInfo other)
        {
            int dist = distance - other.distance;
            if (dist == 0)
            {
                dist = position.x - other.position.x;
                if ( dist == 0)
                {
                    return position.y - other.position.y;
                } else
                {
                    return dist;
                }
            } else
            {
                return dist;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (!(obj is NodeInfo))
                return false;

            NodeInfo info = (NodeInfo)obj;
            // compare elements here
            return info.position == this.position;
        }

        public override int GetHashCode()
        {
            return (int)position.GetHashCode();
        }

        public List<NodeInfo> NeighborsToNodeInfos(List<Vector2Int> neighbors, NodeInfo parent)
        {
            List<NodeInfo> nodeInfos = new List<NodeInfo>();

            foreach (Vector2Int neighbor in neighbors)
            {
                NodeInfo current = new NodeInfo();
                current.position = neighbor;
                current.distance = distance + 1;
                current.parent = parent;
                nodeInfos.Add(current);
            }

            return nodeInfos;
        }

    }


    // checks if a square is both traversable and unoccupied
    // if we give range = -1, search whole map
    public void Dijkstras(ref Dictionary<NodeInfo, NodeInfo> searched, Vector2Int startingSquare, int range)
    {
        toSearch.Clear();
        searched.Clear();
        NodeInfo start = new NodeInfo();
        start.position = startingSquare;
        start.distance = 0;
        start.parent = null;
        toSearch.Add(start);

        while (toSearch.Count > 0)
        {
            NodeInfo current = toSearch.Min;
            int currentDist = current.distance;
            toSearch.Remove(current);
            searched.Add(current, current.parent);

            List<NodeInfo> neighbors = current.NeighborsToNodeInfos(GetNeighbors(current.position), current);

            foreach (NodeInfo neighbor in neighbors)
            {

                // if the node isnt on the map, ignore it
                if (!map.ContainsKey(neighbor.position))
                {
                    continue;
                }

                // if were out of our range, ignore these nodes
                int distance = currentDist + 1;
                if (distance > range && range != -1)
                {
                    continue;
                }

                // if it isnt traversible ignore this node
                if (!map[neighbor.position].traversable)
                {
                    continue;
                }

                // if already in searched list, dont add
                if (searched.ContainsKey(neighbor))
                {
                    continue;
                }

                bool inSearch = toSearch.Contains(neighbor);

                if (!inSearch)
                {
                    toSearch.Add(neighbor);
                }

            }

        }

    }

    public List<NodeInfo> GetPathToPlayer(Vector3 worldPosition)
    {
        List<NodeInfo> ret = new List<NodeInfo>();
        NodeInfo currentCell = new NodeInfo();
        currentCell.position = GetCellPosition(worldPosition);

        // find ourself in the dijkstras result
        bool exists = pathsToPlayer.ContainsKey(currentCell);

        // if we dont exist we just return an array with just us in it
        if (!exists)
        {
            ret.Add(currentCell);
            return ret;
        }
        else
        {
            // we want to trace our steps
            while (currentCell != null)
            {
                ret.Add(currentCell);
                currentCell = pathsToPlayer[currentCell];
            }
        }

        return ret;
    }

    // Return the first node on the smoothed path
    public NodeInfo SmoothPath(Vector3 startingWorldPosition, List<NodeInfo> path)
    {
        Vector2Int startingPosition = path[0].position;
        for (int i = path.Count - 1; i >= 0; i--)
        {
            if (StraightLineWalkable(startingWorldPosition, GetWorldPosition(path[i].position)))
            {
                return path[i];
            }
        }

        return path[0];

    }

    private bool StraightLineWalkable(Vector3 startPos, Vector3 endPos)
    {
        Vector3 direction = endPos - startPos;
        float length = direction.magnitude;
        float elapsedLength = 0;

        direction.Normalize();
        float stepSize = direction.magnitude / 100.0f;

        Vector3 currentPosition = startPos;

        while (elapsedLength < length)
        {
            if (notTraversable.HasTile(notTraversable.WorldToCell(currentPosition)))
            {
                return false;
            }
            currentPosition += direction / 100.0f;
            elapsedLength += stepSize;
        }

        return true;


    }

    public bool OnRoad(Vector3 worldPosition)
    {
        Vector3Int tilePosition = road.WorldToCell(worldPosition);
        TileBase tile = road.GetTile(tilePosition);
        return tile != null;
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

}
