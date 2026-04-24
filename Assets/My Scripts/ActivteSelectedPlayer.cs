using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSelectedPlayer : MonoBehaviour
{
    public List<GameObject> jetPrefabs; // assign prefabs here
    public Transform spawnPoint;        // assign spawn point

    private GameObject currentPlayer;

    private void Start()
    {
        SpawnSelectedPlayer();
    }

    void SpawnSelectedPlayer()
    {
        int index = MenuScript.instance.currentjet;

        if (index >= 0 && index < jetPrefabs.Count)
        {
            currentPlayer = Instantiate(
                jetPrefabs[index],
                spawnPoint.position,
                spawnPoint.rotation,
                spawnPoint
            );

            PlaneMovement plane = GetComponentInParent<PlaneMovement>();
            if (plane != null)
            {
                plane.playerVisualController = currentPlayer.GetComponent<PlayerVisualController>();
            }

            PlayerController controller = currentPlayer.GetComponent<PlayerController>();

            if (controller != null && UI_Canvas.instance != null)
            {
                controller.joystick = UI_Canvas.instance.joystick;
            }
        }
        else
        {
            Debug.LogError("Invalid jet index!");
        }
    }
}