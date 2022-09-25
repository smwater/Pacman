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
        GoastSpawnPoint
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public MapTile[] Map = new MapTile[MAP_SIZE_ROW * MAP_SIZE_COLUMN];
    private const int MAP_SIZE_ROW = 30;
    private const int MAP_SIZE_COLUMN = 30;

    public GameObject PlayerPrefab;
    private GameObject _player;

    public GameObject WallPrefab;
    private readonly int _wallMaxCount = MAP_SIZE_ROW * MAP_SIZE_COLUMN;
    private GameObject[] _walls;

    [SerializeField]
    private static float _posX = -19f;
    [SerializeField]
    private static float _posY = -19f;
    private readonly Vector2 _startPosition = new Vector2(_posX, _posY);

    private float _randomX;
    private float _randomY;
    public GameObject _randomTarget;

    private void Start()
    {
        // ������Ʈ Ǯ���� ���� ���� �� �ν��Ͻ��� ����� ����
        _walls = new GameObject[_wallMaxCount];
        for (int i = 0; i < _wallMaxCount; i++)
        {
            _walls[i] = Instantiate(WallPrefab, _startPosition, Quaternion.identity);
            _walls[i].SetActive(false);
        }
        _player = Instantiate(PlayerPrefab, _startPosition, Quaternion.identity);

        MapLoad();
        MapDraw();
        SpawnRandomTarget(); 
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
                if (r == 0 || r == MAP_SIZE_ROW - 1)
                {
                    Map[r * MAP_SIZE_ROW + c] = MapTile.Wall;
                }
                if (c == 0 || c == MAP_SIZE_COLUMN - 1)
                {
                    Map[r * MAP_SIZE_ROW + c] = MapTile.Wall;
                }
                if (r == 10 && c == 14)
                {
                    Map[r * MAP_SIZE_ROW + c] = MapTile.PlayerSpawnPoint;
                }
                if (r >= 3 && r <= 20 && c % 3 == 1)
                {
                    Map[r * MAP_SIZE_ROW + c] = MapTile.Wall;
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

        for (int r = 0; r < MAP_SIZE_ROW; r++)
        {
            for (int c = 0; c < MAP_SIZE_COLUMN; c++)
            {
                if (Map[r * MAP_SIZE_ROW + c] == MapTile.Wall)
                {
                    _walls[usedWallCount].SetActive(true);
                    _walls[usedWallCount].transform.Translate(new Vector2(c, r));
                    usedWallCount++;
                    continue;
                }
                if (Map[r * MAP_SIZE_ROW + c] == MapTile.PlayerSpawnPoint)
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
    /// <returns>���� ���ٸ� true, ���� �ִٸ� false�� ��ȯ</returns>
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

        // �ش� ��ǥ�� ���� �ִ��� ���ο� ���� ��ȯ
        if (Map[y * MAP_SIZE_ROW + x] == MapTile.Wall)
        {
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// ���� ���� ������ ������ Ÿ���� ������Ű�� �޼ҵ� 
    /// ���Ƿ� ��ġ���� -15~15�� �־��µ� ���߿� ���� �ʿ� 
    /// </summary>
    private void SpawnRandomTarget()
    {
        Color color = _randomTarget.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);

        _randomX = Random.Range(-15, 15);
        _randomY = Random.Range(-15, 15);

        Vector2 randomPos = new Vector2(_randomX, _randomY);

        if (CheckDirectionToGo(randomPos, Direction.None))
        {
            _randomTarget.transform.Translate(_randomX, _randomY, 0);
        }
        else
        {
            SpawnRandomTarget();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ghost") || collision.CompareTag("Wall"))
        {
            SpawnRandomTarget();
        }
    }


}
