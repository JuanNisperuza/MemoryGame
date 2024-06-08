using System;

[Serializable]
public class BlockData 
{
    public int R;
    public int C;
    public int number;
}

[Serializable]
public class BlockDataList
{
    public BlockData[] blocks;
}
