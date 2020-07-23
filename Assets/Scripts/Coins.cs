using UnityEngine;

public class Coins
{
    private int coins = 0;
    private float points = 0;

    private static  Coins _instance;

    private static object syncRoot = new object();

    protected Coins()
    { }
    public static Coins SetInstance()
    {
        lock (syncRoot)
        {
            if (_instance == null)
            {
                _instance = new Coins();
            }
        }
        return _instance;
    }


    public int getSetCoins { get { return GetCoins(); } set { coins = value; } }

    public float setPoints { get { return GetPoints(); } set { points = value; } }

    private int GetCoins()
    {
        return coins;
    }

    private float GetPoints()
    {
        return points;
    }
    
}
