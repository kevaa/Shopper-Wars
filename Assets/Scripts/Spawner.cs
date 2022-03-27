using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] pickupPrefabs;
    [SerializeField] Transform[] pickupSpawnPositions;
    [SerializeField] Player player;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] enemySpawnPositions;
    [Header("numEnemies < length of enemySpawnPositions")]
    [SerializeField] int numEnemies;
    [Header("numBaseGroceriesPerShopper + numSharedGroceries", order = 0)]
    [Space(-10, order = 1)]
    [Header(" < length of pickupSpawnPositions", order = 2)]
    [SerializeField] int numBaseGroceriesPerShopper;
    [SerializeField] int numSharedGroceries; // limited and shared across players
    List<Shopper> shoppers;


    private void Awake()
    {
        shoppers = new List<Shopper> { player };
        // spawn enemies
        var rand = new System.Random();
        var enemySpawnPosInd = 0;
        for (int i = 0; i < numEnemies; i++)
        {
            var prefabInd = rand.Next(enemyPrefabs.Length);
            shoppers.Add(Instantiate(enemyPrefabs[prefabInd], enemySpawnPositions[enemySpawnPosInd++]).GetComponent<Shopper>());
        }
    }

    private void Start()
    {
        var rand = new System.Random();

        // spawn base groceries
        var pickupSpawnPosInd = 0;
        foreach (var shopper in shoppers)
        {
            for (int i = 0; i < numBaseGroceriesPerShopper; i++)
            {
                var prefabInd = rand.Next(pickupPrefabs.Length);
                var spawnedGroceryName = Instantiate(pickupPrefabs[prefabInd], pickupSpawnPositions[pickupSpawnPosInd++]).GetComponent<Pickup>().GetGroceryName();
                shopper.AddToGroceryList(spawnedGroceryName);
            }
        }

        // spawn shared groceries
        for (int i = 0; i < numSharedGroceries; i++)
        {
            var prefabInd = rand.Next(pickupPrefabs.Length);
            var spawnedGroceryName = Instantiate(pickupPrefabs[prefabInd], pickupSpawnPositions[pickupSpawnPosInd++]).GetComponent<Pickup>().GetGroceryName();

            foreach (var shopper in shoppers)
            {
                shopper.AddToGroceryList(spawnedGroceryName);
            }
        }
    }
}
