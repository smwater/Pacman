using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Map �迭�� �� �� �ִ� ������ �����ϴ� ������ ����
public enum MapTile
{
    None,
    Wall,
    PlayerSpawnPoint,
    GhostSpawnPoint,
    SmallCoin,
    BigCoin
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class MapManager : SingletonBehaviour<MapManager>
{
    public MapTile[,] Map = new MapTile[MAP_SIZE_ROW, MAP_SIZE_COLUMN];
    public const int MAP_SIZE_ROW = 30;
    public const int MAP_SIZE_COLUMN = 30;

    public GameObject PlayerPrefab;
    private GameObject _player;

    public GameObject GhostPrefab;
    private readonly int _ghostMaxCount = 4;
    private GameObject[] _ghosts;

    public GameObject WallPrefab;
    private readonly int _wallMaxCount = 500;
    private GameObject[] _walls;

    public GameObject SmallCoinPrefab;
    private readonly int _smallCoinMaxCount = 600;
    private GameObject[] _smallCoins;

    public GameObject BigCoinPrefab;
    private readonly int _bigCoinMaxCount = 50;
    private GameObject[] _bigCoins;

    [SerializeField]
    private int _placedSmallCoinCount = 0;
    [SerializeField]
    private int _placedBigCoinCount = 0;
    [SerializeField]
    private int _earnedSmallCoinCount = 0;
    [SerializeField]
    private int _earnedBigCoinCount = 0;

    private static readonly float _startPosX = 0f;
    private static readonly float _startPosY = 0f;
    private readonly Vector2 _startPosition = new Vector2(_startPosX, _startPosY);

    private void OnEnable()
    {
        GameManager.Instance.PlayerDead.AddListener(BackToStartPosition);
    }

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
        _player.SetActive(false);
        _player.transform.SetParent(transform);

        _ghosts = new GameObject[_ghostMaxCount];
        for (int i = 0; i < _ghostMaxCount; i++)
        {
            _ghosts[i] = Instantiate(GhostPrefab, _startPosition, Quaternion.identity);
            _ghosts[i].SetActive(false);
            _ghosts[i].transform.SetParent(transform);
        }

        _smallCoins = new GameObject[_smallCoinMaxCount];
        for (int i = 0; i < _smallCoinMaxCount; i++)
        {
            _smallCoins[i] = Instantiate(SmallCoinPrefab, _startPosition, Quaternion.identity);
            _smallCoins[i].SetActive(false);
            _smallCoins[i].transform.SetParent(transform);
        }

        _bigCoins = new GameObject[_bigCoinMaxCount];
        for (int i = 0; i < _bigCoinMaxCount; i++)
        {
            _bigCoins[i] = Instantiate(BigCoinPrefab, _startPosition, Quaternion.identity);
            _bigCoins[i].SetActive(false);
            _bigCoins[i].transform.SetParent(transform);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerDead.RemoveListener(BackToStartPosition);
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
                // �� ��ġ
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
                if (r == 13 && ((c >= 0 && c <= 6) || (c >= 23 && c <= MAP_SIZE_COLUMN - 1)))
                {
                    Map[r, c] = MapTile.Wall;
                }
                if (r == 13 && ((c >= 11 && c <= 13) || (c >= 16 && c <= 18)))
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
                if (r == 17 && ((c >= 11 && c <= 13) || (c >= 16 && c <= 18)))
                {
                    Map[r, c] = MapTile.Wall;
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
                // ���� ��ȯ ����
                if (c == 9 && r == 4)
                {
                    Map[r, c] = MapTile.GhostSpawnPoint;
                }
                if (c == 20 && r == 4)
                {
                    Map[r, c] = MapTile.GhostSpawnPoint;
                }
                if (c == 3 && r == 26)
                {
                    Map[r, c] = MapTile.GhostSpawnPoint;
                }
                if (c == 26 && r == 26)
                {
                    Map[r, c] = MapTile.GhostSpawnPoint;
                }
                // �÷��̾� ��ȯ ����
                if (c == 14 && r == 10)
                {
                    Map[r, c] = MapTile.PlayerSpawnPoint;
                }
                // ū ����
                if (r == 1 && (c == 1 || c == 28))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 2 && (c == 5 || c == 24))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 6 && (c == 12 || c == 17))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 10 && (c == 4 || c == 25))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 12 && (c == 1 || c == 28))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 13 && (c == 7 || c == 22))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 14 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 15 && c >= 13 && c <= 16)
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 16 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 17 && (c == 7 || c == 22))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 20 && (c == 4 || c == 25))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 23 && (c == 13 || c == 16))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 25 && (c == 4 || c == 25))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                if (r == 28 && (c == 1 || c == 28))
                {
                    Map[r, c] = MapTile.BigCoin;
                }
                // �������� ���� �������� ä��
                if (Map[r, c] == MapTile.None)
                {
                    Map[r, c] = MapTile.SmallCoin;
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
        int usedGhostCount = 0;

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
                if (Map[r, c] == MapTile.SmallCoin)
                {
                    _smallCoins[_placedSmallCoinCount].SetActive(true);
                    _smallCoins[_placedSmallCoinCount].transform.Translate(new Vector2(c, r));
                    _placedSmallCoinCount++;
                    continue;
                }
                if (Map[r, c] == MapTile.BigCoin)
                {
                    _bigCoins[_placedBigCoinCount].SetActive(true);
                    _bigCoins[_placedBigCoinCount].transform.Translate(new Vector2(c, r));
                    _placedBigCoinCount++;
                    continue;
                }
                if (Map[r, c] == MapTile.GhostSpawnPoint)
                {
                    _ghosts[usedGhostCount].SetActive(true);
                    _ghosts[usedGhostCount].transform.Translate(new Vector2(c, r));
                    usedGhostCount++;
                    continue;
                }
                if (Map[r, c] == MapTile.PlayerSpawnPoint)
                {
                    _player.SetActive(true);
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

        // �ش� ��ǥ�� ���̳� ������ �� ���� �ִ���(������ ����) ���ο� ���� ��ȯ
        if (Map[y, x] == MapTile.Wall)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �÷��̾�� ������ ó�� �ڸ��� ��ġ�ϴ� �Լ�
    /// GameManager���� ������ �÷��̾� ��� �̺�Ʈ�� ������� �� ȣ�� ��
    /// </summary>
    public void BackToStartPosition()
    {
        _player.transform.position = new Vector2(14, 10);
        _ghosts[0].GetComponent<GhostAI>().SetStartPosition(9, 4);
        _ghosts[1].GetComponent<GhostAI>().SetStartPosition(20, 4);
        _ghosts[2].GetComponent<GhostAI>().SetStartPosition(3, 26);
        _ghosts[3].GetComponent<GhostAI>().SetStartPosition(26, 26);
    }

    /// <summary>
    /// Stage1�� �ҷ����� �Լ�
    /// </summary>
    public void LoadStage1()
    {
        MapLoad();
        MapDraw();
    }

    /// <summary>
    /// ���� ������ ���� ������ �����ش�.
    /// </summary>
    /// <param name="coin">������ ����</param>
    public void CountCoin(CoinScore coin)
    {
        if (coin == CoinScore.Small)
        {
            _earnedSmallCoinCount++;
        }    
        if (coin == CoinScore.Big)
        {
            _earnedBigCoinCount++;
        }
    }
}
