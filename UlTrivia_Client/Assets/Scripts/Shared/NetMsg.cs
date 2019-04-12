public static class NetOP
{
    public const int None = 0;

    public const int SendText = 3;
    public const int SendVote = 4;
}
[System.Serializable]
public abstract class NetMsg
{
    public byte OP { set; get; }
    
    public NetMsg()
    {
        OP = NetOP.None;
    }
}