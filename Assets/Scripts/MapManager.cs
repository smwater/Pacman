using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    // Map 배열에 들어갈 수 있는 정보를 저장하는 열거형 변수
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
        // 오브젝트 풀링을 쓰기 위해 벽 인스턴스를 충분히 만듦
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
    /// Map 배열에 저장된 정보를 읽어와 좌측 하단부터 화면에 그린다.
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
    /// 지정 방향으로 나아갈 수 있는지 여부를 bool값으로 반환한다.
    /// </summary>
    /// <param name="currentPosition">현재 위치</param>
    /// <param name="direction">지정 방향</param>
    /// <returns>벽이 없다면 true, 벽이 있다면 false를 반환</returns>
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

        // 해당 좌표에 벽이 있는지 여부에 따라 반환
        if (Map[y * MAP_SIZE_ROW + x] == MapTile.Wall)
        {
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// 벽이 없는 렌덤한 지점에 타켓을 스폰시키는 메소드 
    /// 임의로 위치값을 -15~15로 주었는데 나중에 수정 필요 
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
