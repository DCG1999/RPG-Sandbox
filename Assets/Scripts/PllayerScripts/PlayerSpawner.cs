using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SpawnPlayer(this.transform);
    }

}
