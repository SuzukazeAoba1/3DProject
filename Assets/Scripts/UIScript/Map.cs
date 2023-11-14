using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MapData
{
    int mapId;
    string mapName;
    string mapInfo;

    public void get(int id, string name, string info)
    {
        mapId = id;
        mapName = name;
        mapInfo = info;
    }

    public int setId()
    {
        return mapId;
    }

    public string setName()
    {
        return mapName;
    }

    public string setInfo()
    {
        return mapInfo;
    }

}
