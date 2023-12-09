using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    public int waveCount = 0;

    public List<GameObject> Enemies; // Assuming this is just a list of GameObjects now
    public List<int> Waves; // Assuming this still represents the number of enemies in each wave
    public int currentWave;

    private int maxWave; // The maximum wave number

    private int enemyCount;
    private bool Started = false;

    private void Start()
    {
        maxWave = Waves.Count - 1; // Set the maxWave based on the number of waves

        // Deactivate all enemies initially
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
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

    public void CheckAndSpawnNextWave()
    {
        if (IsWaveCleared(currentWave))
        {
            if (currentWave >= maxWave)
            {
                // All waves are cleared, disable this GameObject
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
    // This needs a new implementation
    private bool IsWaveCleared(int wave)
    {
        // Implementation depends on how you determine which enemies belong to which wave
        // For now, this function returns true for simplicity
        return true;
    }

    // Activate a certain number of GameObjects for the given wave
    private void ActivateWave(int wave)
    {
        int enemiesToActivate = Waves[wave];
        int activatedEnemies = 0;

        foreach (var enemy in Enemies)
        {
            if (enemy != null && !enemy.activeSelf && activatedEnemies < enemiesToActivate)
            {
                enemy.SetActive(true);
                activatedEnemies++;
            }
        }
    }
}