using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Map 배열에 들어갈 수 있는 정보를 저장하는 열거형 변수
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
        // 오브젝트 풀링을 쓰기 위해 벽 인스턴스를 충분히 만듦
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
    /// 파일로 저장한 맵을 읽어와 Map 배열에 정보를 저장한다.
    /// 현재는 관련 기능이 구현되지 않아 수동으로 맵 정보를 저장한다.
    /// </summary>
    private void MapLoad()
    {
        // 아래는 임시로 만드는 맵
        for (int r = 0; r < MAP_SIZE_ROW; r++)
        {
            for (int c = 0; c < MAP_SIZE_COLUMN; c++)
            {
                // 벽 설치
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
                // 유령 소환 지점
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
                // 플레이어 소환 지점
                if (c == 14 && r == 10)
                {
                    Map[r, c] = MapTile.PlayerSpawnPoint;
                }
                // 큰 코인
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
                // 나머지는 작은 코인으로 채움
                if (Map[r, c] == MapTile.None)
                {
                    Map[r, c] = MapTile.SmallCoin;
                }
                // 별도로 빈 공간이 존재하므로 설정
                if (r >= 0 && r <= 1 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.None;
                }
                if (r >= 14 && r <= 15 && c >= 0 && c <= 2)
                {
                    Map[r, c] = MapTile.None;
                }
                if (r == 13 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.None;
                }
                if (r == 14 && ((c >= 12 && c <= 13) || (c >= 16 && c <= 17)))
                {
                    Map[r, c] = MapTile.None;
                }
                if (r == 15 && (c == 12 || c == 17))
                {
                    Map[r, c] = MapTile.None;
                }
                if (r == 16 && ((c >= 12 && c <= 13) || (c >= 16 && c <= 17)))
                {
                    Map[r, c] = MapTile.None;
                }
                if (r == 17 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.None;
                }
                if (r >= 14 && r <= 15 && c >= 27 && c <= MAP_SIZE_COLUMN - 1)
                {
                    Map[r, c] = MapTile.None;
                }
                if (r >= 28 && r <= MAP_SIZE_ROW - 1 && c >= 14 && c <= 15)
                {
                    Map[r, c] = MapTile.None;
                }
            }
        }
    }

    /// <summary>
    /// Map 배열에 저장된 정보를 읽어와 좌측 하단부터 화면에 그린다.
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
    /// 지정 방향으로 나아갈 수 있는지 여부를 bool값으로 반환한다.
    /// </summary>
    /// <param name="currentPosition">현재 위치</param>
    /// <param name="direction">지정 방향</param>
    /// <returns>막히지 않았다면 true, 막혀 있다면 false를 반환</returns>
    public bool CheckDirectionToGo(Vector2 currentPosition, Direction direction)
    {
        // 현재 위치가 배열에서 어디를 가리키는지 알기 위해 계산
        int x = (int)(currentPosition.x - _startPosition.x);
        int y = (int)(currentPosition.y - _startPosition.y);

        // 지정 방향에 따라 다른 좌표를 검사해야하므로 세팅
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

        // 해당 좌표에 벽이나 유령의 집 문이 있는지(유령은 예외) 여부에 따라 반환
        if (Map[y, x] == MapTile.Wall)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 플레이어와 유령을 처음 자리로 배치하는 함수
    /// GameManager에서 구독한 플레이어 사망 이벤트가 실행됐을 때 호출 됨
    /// </summary>
    public void BackToStartPosition()
    {
        _player.transform.position = new Vector2(14, 10);
        _player.GetComponent<PlayerMove>().State = PlayerState.Usually;

        _ghosts[0].GetComponent<GhostAI>().SetStartPosition(9, 4);
        _ghosts[1].GetComponent<GhostAI>().SetStartPosition(20, 4);
        _ghosts[2].GetComponent<GhostAI>().SetStartPosition(3, 26);
        _ghosts[3].GetComponent<GhostAI>().SetStartPosition(26, 26);
    }

    /// <summary>
    /// Stage1을 불러오는 함수
    /// </summary>
    public void LoadStage1()
    {
        MapLoad();
        MapDraw();
    }

    /// <summary>
    /// 코인 종류에 따라 갯수를 세어준다.
    /// </summary>
    /// <param name="coin">코인의 종류</param>
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

        if (_placedSmallCoinCount == _earnedSmallCoinCount && _placedBigCoinCount == _earnedBigCoinCount)
        {
            GameManager.Instance.GameClear.Invoke(GameState.Clear);
        }
    }

    /// <summary>
    /// 코인을 점수로 전환해서 합한 값을 반환한다.
    /// </summary>
    /// <returns>얻은 점수의 합</returns>
    public int GetTotalScore()
    {
        int score = _earnedSmallCoinCount * 10;
        score += _earnedBigCoinCount * 50;

        return score;
    }
}
