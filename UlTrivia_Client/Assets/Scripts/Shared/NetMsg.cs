public static class NetOP
{
    public const int None = 0;

    public const int CreateAccount = 1;
    public const int SelectChara = 2;
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