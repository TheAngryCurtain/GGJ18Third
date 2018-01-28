using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TownGenerator : MonoBehaviour
{
    //[SerializeField] private Tilemap mGroundMap;
    //[SerializeField] private Tilemap mHouseMap;

    //[SerializeField] private Tile mGroundTile;
    //[SerializeField] private Tile mPathTile;
    //[SerializeField] private Tile[] mBuildingTiles;

    //private void Start()
    //{
    //    BuildTown();
    //}

    //private void BuildTown()
    //{
    //    int mapWidth = 30;
    //    int mapLength = 30;

    //    List<int> verticalStreetXs = new List<int>();
    //    List<int> horizontalStreetYs = new List<int>();

    //    BuildStreets(mapWidth, mapLength, ref verticalStreetXs, ref horizontalStreetYs);
    //    PlaceHouses(mapWidth, mapLength, ref verticalStreetXs, ref horizontalStreetYs);
    //}

    //private void BuildStreets(int mapWidth, int mapLength, ref List<int> verticalStreetXs, ref List<int> horizontalStreetYs)
    //{
    //    int sectorMin = 3;

    //    int numHoriztonalStreets = UnityEngine.Random.Range(5, 8);
    //    int numVerticalStreets = UnityEngine.Random.Range(5, 8);

    //    //List<int> verticalHouseXs = new List<int>();
    //    //List<int> horizontalHouseYs = new List<int>();

    //    int sectorWidth = mapWidth / numVerticalStreets;
    //    int sectorLength = mapLength / numHoriztonalStreets;

    //    //Vector3Int lastPlacedHouse = Vector3Int.zero;

    //    // define intersections
    //    int prevSectorX = 0;
    //    for (int i = 0; i < numHoriztonalStreets; i++)
    //    {
    //        int min = prevSectorX + sectorMin;
    //        int randX = UnityEngine.Random.Range(min, min + sectorWidth);
    //        verticalStreetXs.Add(randX);

    //        // potential houses on either side
    //        //verticalHouseXs.Add(randX - 2);
    //        //verticalHouseXs.Add(randX + 2);

    //        prevSectorX = min + sectorWidth;
    //    }

    //    int prevSectorY = 0;
    //    for (int i = 0; i < numHoriztonalStreets; i++)
    //    {
    //        int min = prevSectorY + sectorMin;
    //        int randY = UnityEngine.Random.Range(min, min + sectorLength);
    //        horizontalStreetYs.Add(randY);

    //        // potential houses on either side
    //        //horizontalHouseYs.Add(randY - 2);
    //        //horizontalHouseYs.Add(randY + 2);

    //        prevSectorY = min + sectorLength;
    //    }

    //    // fill tilemap
    //    bool skipHorizontalStreet = false;
    //    bool skipVerticalStreet = false;
    //    //bool skipHouse = false;
    //    for (int i = 0; i < mapLength; i++)
    //    {
    //        for (int j = 0; j < mapWidth; j++)
    //        {
    //            // place some grass
    //            Vector3Int pos = new Vector3Int(j, i, 0);
    //            mGroundMap.SetTile(pos, mGroundTile);

    //            bool horizontalStreet = horizontalStreetYs.Contains(i);
    //            bool verticalStreet = verticalStreetXs.Contains(j);
    //            bool intersection = horizontalStreet && verticalStreet;

    //            //bool horizontalHouse = horizontalHouseYs.Contains(i);
    //            //bool verticalHouse = verticalHouseXs.Contains(j);

    //            if (intersection)
    //            {
    //                // draw intersection
    //                mGroundMap.SetTile(pos, mPathTile);

    //                // skip this street?
    //                skipHorizontalStreet = false;
    //                skipVerticalStreet = false;
    //                //skipHouse = false;

    //                int rand = UnityEngine.Random.Range(0, 5);
    //                if (rand == 0)
    //                {
    //                    skipHorizontalStreet = true;
    //                    skipVerticalStreet = false;
    //                }

    //                rand = UnityEngine.Random.Range(0, 3);
    //                if (rand == 0)
    //                {
    //                    skipHorizontalStreet = false;
    //                    skipVerticalStreet = true;

    //                    //skipHouse = true;
    //                }
    //            }

    //            // place street
    //            if (verticalStreet && !skipVerticalStreet)
    //            {
    //                mGroundMap.SetTile(pos, mPathTile);
    //            }

    //            if (horizontalStreet && !skipHorizontalStreet)
    //            {
    //                mGroundMap.SetTile(pos, mPathTile);
    //            }

                //if (!skipHouse && CanPlaceHouse(i, j, mapWidth, mapLength, lastPlacedHouse))
                //{
                //    int randHouseIndex = UnityEngine.Random.Range(0, mBuildingTiles.Length - 1);
                //    if (horizontalHouse)
                //    {
                //        mHouseMap.SetTile(pos, mBuildingTiles[randHouseIndex]);
                //    }

                //    if (verticalHouse)
                //    {
                //        mHouseMap.SetTile(pos, mBuildingTiles[randHouseIndex]);
                //    }

                //    lastPlacedHouse = pos;
                //}
    //        }
    //    }
    //}

    //private void PlaceHouses(int mapWidth, int mapLength, ref List<int> verticalStreetXs, ref List<int> horizontalStreetYs)
    //{
    //    for (int i = 0; i < mapLength; i++)
    //    {
    //        for (int j = 0; j < mapWidth; j++)
    //        {
    //            Vector3Int aboveTilePos = new Vector3Int(j, i, 0);
    //            Vector3Int belowTilePos = new Vector3Int(j, i - 4, 0);
    //            Vector3Int tilePos = new Vector3Int(j, i, 0);

    //            bool horizontalHouse = mGroundMap.GetTile(aboveTilePos) == mGroundTile && (mGroundMap.GetTile(aboveTilePos) == mPathTile || mGroundMap.GetTile(belowTilePos) == mPathTile);
    //            if (horizontalHouse)
    //            {
    //                int randHouseIndex = UnityEngine.Random.Range(0, mBuildingTiles.Length - 1);
    //                mHouseMap.SetTile(belowTilePos, mBuildingTiles[randHouseIndex]);
    //            }
    //        }
    //    }
    //}

    //private bool CanPlaceHouse(int i, int j, int mapWidth, int mapLength, Vector3Int lastPlacedPos)
    //{
    //    return i != 0 && i != mapLength - 1 && j != 0 && j != mapWidth - 1 && (j > lastPlacedPos.x + 2 || i > lastPlacedPos.y + 2);
    //}
    }
