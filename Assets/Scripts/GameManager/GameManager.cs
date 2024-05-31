using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Transform player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>(); 
    }
    public static GameManager Instance;

    public Transform GetPlayer()
    {
        return player;
    }
}
