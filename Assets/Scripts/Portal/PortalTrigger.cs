using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered portal. Game Over.");

            // Call the method to end the game
            EndGame();
        }
    }

    void EndGame()
    {
        // Load the Game Over scene
        SceneManager.LoadScene("GameCompleted");
    }

}
