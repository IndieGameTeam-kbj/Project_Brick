using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public int score;
    public int prevBestScore;
    public bool isNewBestScore;

    public List<BrickSaveData> boardBricks = new List<BrickSaveData>();

    // 하단 칸마다 종류 저장. -1이면 빈칸.
    public int[] preparedTypes;
}

[Serializable]
public class BrickSaveData
{
    public BrickType type;
    public int row;
    public int column;
}