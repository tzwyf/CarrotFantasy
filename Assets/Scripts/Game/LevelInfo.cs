using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo {

    public int bigLevelID; 
    public int levelID;

    public List<GridPoint.GridState> gridPoints;

    public List<GridPoint.GridIndex> monsterPath;

    public List<Round.RoundInfo> roundInfo;
}
