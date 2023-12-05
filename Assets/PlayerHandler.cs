using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public int money;
    public int credits;
    public int map1level, map2level;

    public List<Car> ownedCars;

    private void Awake()
    {
        money = ES3.Load<int>("playerMoney", 0);
        credits = ES3.Load<int>("playerCredits", 0);
        map1level = ES3.Load<int>("map1Level", 0);
        map2level = ES3.Load<int>("map2Level", 0);
        ownedCars = ES3.Load("playerCars", new List<Car>());
    }

}
