using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    // Map �迭�� �� �� �ִ� ������ �����ϴ� ������ ����
    public enum MapTile
    {
        None,
        Wall,
        PlayerSpawnPoint,
        GhostSpawnPoint,
        GhostHouseDoor
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public MapTile[,] Map = new MapTile[MAP_SIZE_ROW, MAP_SIZE_COLUMN];
    public const int MAP_SIZE_ROW = 30;
    public const int MAP_SIZE_COLUMN = 30;

    public GameObject PlayerPrefab;
    private GameObject _player;

    public GameObject GhostHouseDoorPrefab;
    private const int DOOR_COUNT = 2;
    private GameObject[] _ghostHouseDoors;

    public GameObject WallPrefab;
    private readonly int _wallMaxCount = MAP_SIZE_ROW * MAP_SIZE_COLUMN;
    private GameObject[] _walls;

    private static readonly float _startPosX = 0f;
    private static readonly float _startPosY = 0f;
    private readonly Vector2 _startPosition = new Vector2(_startPosX, _startPosY);

    private void Start()
    {
        // ������Ʈ Ǯ���� ���� ���� �� �ν��Ͻ��� ����� ����
        _walls = new GameObject[_wallMaxCount];
        for (int i = 0; i < _wallMaxCount; i++)
        {
            _walls[i] = Instantiate(WallPrefab, _startPosition, Quaternion.identity);
            _walls[i].SetActive(false);
            _walls[i].transform.SetParent(transform);
        }

        _player = Instantiate(PlayerPrefab, _startPosition, Quaternion.identity);
        _player.transform.SetParent(transform);

        _ghostHouseDoors = new GameObject[DOOR_COUNT];
        for (int i = 0; i < DOOR_COUNT; i++)
        {
            _ghostHouseDoors[i] = Instantiate(GhostHouseDoorPrefab, _startPosition, Quaternion.identity);
            _ghostHouseDoors[i].SetActive(false);
            _ghostHouseDoors[i].transform.SetParent(transform);
        }

        MapLoad();
        MapDraw();
    }

    /// <summary>
    /// ���Ϸ� ������ ���� �о�� Map �迭�� ������ �����Ѵ�.
    /// ����� ���� ����� �������� �ʾ� �������� �� ������ �����Ѵ�.
    /// </summary>
    private void MapLoad()
    {
        // �Ʒ��� �ӽ÷� ����� ��
        for (int r = 0; r < MAP_SIZE_ROW; r++)
        {
            for (int c = 0; c < MAP_SIZE_COLUMN; c++)
            {
                if ((r == 0 || r == MAP_SIZE_ROW - 1) && c != 14 && c != 15)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if ((c == 0 || c == MAP_SIZE_COLUMN - 1) && r != 14 && r != 15)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (((r >= 2 && r <= 3) || (r >= 5 && r <= 7) || (r >= 9 && r <= 10)) && c >= 2 && c <= 3)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (((r >= 2 && r <= 3) || (r >= 5 && r <= 7) || (r >= 9 && r <= 10)) && c >= 7 && c <= 8)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (((r >= 2 && r <= 3) || (r >= 5 && r <= 7) || (r >= 9 && r <= 10)) && c >= 21 && c <= 22)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (((r >= 2 && r <= 3) || (r >= 5 && r <= 7) || (r >= 9 && r <= 10)) && c >= 26 && c <= 27)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if ((c == 5 || c == 13 || c == 16 || c == 24) && r >= 3 && r <= 9)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (c == 12 && (r == 3 || r == 9))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (c == 17 && (r == 3 || r == 9))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (c == 14 && r == 10)
                {
                    Map[r, c] = MapTile.PlayerSpawnPoint;
                }
                if (r == 13 && ((c >= 0 && c <= 6) || (c >= 11 && c <= 18) || (c >= 23 && c <= MAP_SIZE_COLUMN - 1)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (r == 16 && ((c >= 0 && c <= 6) || (c >= 23 && c <= MAP_SIZE_COLUMN - 1)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if ((c == 11 || c == 18) && r >= 13 && r <= 17)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (c == 13 && r == 15)
                {
                    Map[r, c] = MapTile.GhostSpawnPoint;
                }
                if (r == 17 && ((c >= 11 && c <= 13) || (c >= 16 && c <= 18)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (r == 17 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.GhostHouseDoor;
                }
                if (r == 18 && ((c >= 3 && c <= 7) || (c >= 22 && c <= 26)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (r == 27 && ((c >= 3 && c <= 7) || (c >= 22 && c <= 26)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if ((c == 14 || c == 15) && r >= 19 && r <= 27)
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (((c == 2 || c == 3) || (c >= 5 && c <= 9) || (c >= 20 && c <= 24) || (c == 26 || c == 27)) && r >= 21 && r <= 24)
                {
                    Map[r, c] = MapTile.Wall;
                }
            }
        }
    }

    /// <summary>
    /// Map �迭�� ����� ������ �о�� ���� �ϴܺ��� ȭ�鿡 �׸���.
    /// </summary>
    private void MapDraw()
    {
        int usedWallCount = 0;
        int usedDoorCount = 0;

        for (int r = 0; r < MAP_SIZE_ROW; r++)
        {
            for (int c = 0; c < MAP_SIZE_COLUMN; c++)
            {
                if (Map[r, c] == MapTile.Wall)
                {
                    _walls[usedWallCount].SetActive(true);
                    _walls[usedWallCount].transform.Translate(new Vector2(c, r));
                    usedWallCount++;
                    continue;
                }
                if (Map[r, c] == MapTile.GhostHouseDoor)
                {
                    _ghostHouseDoors[usedDoorCount].SetActive(true);
                    _ghostHouseDoors[usedDoorCount].transform.Translate(new Vector2(c, r));
                    usedDoorCount++;
                    continue;
                }
                if (Map[r, c] == MapTile.PlayerSpawnPoint)
                {
                    _player.transform.Translate(new Vector2(c, r));
                }
            }
        }
    }
    
    /// <summary>
    /// ���� �������� ���ư� �� �ִ��� ���θ� bool������ ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="currentPosition">���� ��ġ</param>
    /// <param name="direction">���� ����</param>
    /// <returns>������ �ʾҴٸ� true, ���� �ִٸ� false�� ��ȯ</returns>
    public bool CheckDirectionToGo(Vector2 currentPosition, Direction direction)
    {
        // ���� ��ġ�� �迭���� ��� ����Ű���� �˱� ���� ���
        int x = (int)(currentPosition.x - _startPosition.x);
        int y = (int)(currentPosition.y - _startPosition.y);

        // ���� ���⿡ ���� �ٸ� ��ǥ�� �˻��ؾ��ϹǷ� ����
        switch (direction)
        {
            case Direction.Up:
                y++;
                break;
            case Direction.Down:
                y--;
                break;
            case Direction.Left:
                x--;
                break;
            case Direction.Right:
                x++;
                break;
            default:
                break;
        }



        // �ش� ��ǥ�� ���̳� ������ �� ���� �ִ��� ���ο� ���� ��ȯ
        if (Map[y, x] == MapTile.Wall || Map[y, x] == MapTile.GhostHouseDoor)
        {
            return false;
        }

        return true;
    }
}
