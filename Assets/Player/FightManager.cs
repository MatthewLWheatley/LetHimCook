using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    public int waveCount = 0;

    public List<GameObject> Enemies;
    public List<int> Waves;
    public int currentWave;

    private int maxWave; // The maximum wave number
    private int enemyCount;
    private bool Started = false;

    private void Start()
    {
        maxWave = Waves.Count - 1; // The last index in the Waves list is the max wave number

        // Set all enemies inactive initially
        foreach (var enemy in Enemies)
        {
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        if (Started)
        {
            CheckAndSpawnNextWave();

            if (Input.GetKey(KeyCode.Escape))
            {
                cam.GetComponent<CameraGib>().xLock = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the MainCamera
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            // Lock the camera's x-axis movement
            cam.GetComponent<CameraGib>().xLock = true;

            // Activate the first wave of enemies
            for (int i = 0; i < Waves[0]; i++)
            {
                if (i < Enemies.Count && Enemies[i] != null)
                {
                    Enemies[i].SetActive(true);
                }
            }

            // Indicate that the fight has started
            Started = true;
        }
    }

    public void CheckAndSpawnNextWave()
    {
        if (IsWaveCleared(currentWave))
        {
            if (currentWave >= maxWave)
            {
                // All waves are cleared, disable this GameObject
                cam.GetComponent<CameraGib>().xLock = false;
                this.gameObject.SetActive(false);
            }
            else
            {
                currentWave++;
                ActivateWave(currentWave);
            }
        }
    }

    // Check if all GameObjects in a given wave are destroyed or inactive
    private bool IsWaveCleared(int wave)
    {
        int startIndex = 0;
        for (int i = 0; i < wave; i++)
        {
            startIndex += Waves[i];
        }

        int endIndex = startIndex + Waves[wave];
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < Enemies.Count && Enemies[i] != null && Enemies[i].activeSelf)
            {
                return false; // If any GameObject in the wave is still active, the wave is not cleared
            }
        }
        return true; // All GameObjects in the wave are destroyed or inactive
    }

    // Activate all GameObjects in a given wave
    private void ActivateWave(int wave)
    {
        int startIndex = 0;
        for (int i = 0; i < wave; i++)
        {
            startIndex += Waves[i];
        }

        int endIndex = startIndex + Waves[wave];
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < Enemies.Count && Enemies[i] != null)
            {
                Enemies[i].SetActive(true);
            }
        }
    }
}
