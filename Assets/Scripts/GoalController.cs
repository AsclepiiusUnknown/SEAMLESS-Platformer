using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public GameObject confettiEffect;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Level Completed");
            GameObject confettiInstance = Instantiate(confettiEffect, transform.position, Quaternion.identity);
            Destroy(confettiInstance, 5);

            Invoke("CompleteLevel", 3);
        }
    }

    public void CompleteLevel()
    {
        gameManager.CompleteLevel();
    }
}
