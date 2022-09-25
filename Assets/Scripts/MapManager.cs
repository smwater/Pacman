using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MonoBehaviour>
{
    public enum MapTile
    {
        None,
        Wall,
        PlayerSpawnPoint,
        GoastSpawnPoint
    }

    public MapTile[] Map = new MapTile[MAP_SIZE_ROW * MAP_SIZE_COLUMN];
    public GameObject WallPrefab;
    public GameObject PlayerPrefab;

    private const int MAP_SIZE_ROW = 30;
    private const int MAP_SIZE_COLUMN = 30;

    private readonly int _wallMaxCount = MAP_SIZE_ROW * MAP_SIZE_COLUMN;
    private GameObject[] _walls;
    private GameObject _player;

    private readonly Vector2 _startPosition = new Vector2(-19f, -19f);

    private void Start()
    {
        // 오브젝트 풀링을 쓰기 위해 인스턴스를 충분히 만듦
        _walls = new GameObject[_wallMaxCount];
        for (int i = 0; i < _wallMaxCount; i++)
        {
            _walls[i] = Instantiate(WallPrefab, _startPosition, Quaternion.identity);
            _walls[i].SetActive(false);
        }
        _player = Instantiate(PlayerPrefab, _startPosition, Quaternion.identity);

        MapLoad();
        MapDraw();
    }

    /// <summary>
    /// 파일로 저장한 맵을 읽어와 Map 배열에 정보를 저장한다.
    /// 현재는 관련 기능이 구현되지 않아 수동으로 맵 정보를 저장한다.
    /// </summary>
    private void MapLoad()
    {
        // 여기서 저장된 맵 파일 읽어오기

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
    /// Map 배열에 저장된 정보를 읽어와 화면에 그린다.
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
                }
                if (Map[r * MAP_SIZE_ROW + c] == MapTile.PlayerSpawnPoint)
                {
                    _player.transform.Translate(new Vector2(c, r));
                }
            }
        }
    }
}
