using UnityEngine;

public class Coins
{
    /// <summary>
    /// Количество монеток
    /// </summary>
    private int coins = 0;

    /// <summary>
    /// Количество очков
    /// </summary>
    private float points = 0;

    /// <summary>
    /// Экземпляр класса
    /// </summary>
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


    /// <summary>
    /// Получить или задать монетки
    /// </summary>
    public int getSetCoins { get { return GetCoins(); } set { coins = value; } }

    /// <summary>
    /// Установить или получить очки
    /// </summary>
    public float setPoints { get { return GetPoints(); } set { points = value; } }

    /// <summary>
    /// Получить монетки
    /// </summary>
    /// <returns></returns>
    private int GetCoins()
    {
        return coins;
    }

    /// <summary>
    /// Получить очки
    /// </summary>
    /// <returns></returns>
    private float GetPoints()
    {
        return points;
    }
    
}
