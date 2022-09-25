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
        // ������Ʈ Ǯ���� ���� ���� �ν��Ͻ��� ����� ����
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
    /// ���Ϸ� ������ ���� �о�� Map �迭�� ������ �����Ѵ�.
    /// ����� ���� ����� �������� �ʾ� �������� �� ������ �����Ѵ�.
    /// </summary>
    private void MapLoad()
    {
        // ���⼭ ����� �� ���� �о����

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
    /// Map �迭�� ����� ������ �о�� ȭ�鿡 �׸���.
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
