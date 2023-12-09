using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    public int waveCount = 0;

    public List<(int,GameObject)> Enemies;
    public List<int> Waves;
    public int currentWave;

    private int maxWave; // The maximum wave number


    private int enemyCount;
    private bool Started = false;

    private void Start()
    {
        // Dictionary to keep track of counts for each wave
        Dictionary<int, int> waveCounts = new Dictionary<int, int>();

        // Iterate through each enemy in the Enemies list
        foreach (var enemy in Enemies)
        {
            int waveNumber = enemy.Item1; // Get the wave number

            // If the wave number is already in the dictionary, increment its count
            if (waveCounts.ContainsKey(waveNumber))
            {
                waveCounts[waveNumber]++;
            }
            else
            {
                // Otherwise, add the wave number to the dictionary with a count of 1
                waveCounts.Add(waveNumber, 1);
            }
        }

        // Clear the Waves list and populate it with the counts from the dictionary
        Waves.Clear();
        foreach (var waveCount in waveCounts)
        {
            Waves.Add(waveCount.Value); // Add the count of GameObjects for each wave
        }


        maxWave = Waves[Waves.Count];

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].Item2.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            cam.GetComponent<CameraGib>().xLock = true;
        }
        for (int i = 0; i < Waves[0]; i++)
        {
            Enemies[i].Item2.SetActive(true);
        }
        Started = true;
        //Debug.Log(collision.gameObject.ToString());
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
    private bool IsWaveCleared(int wave)
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.Item1 == wave && enemy.Item2 != null && enemy.Item2.activeSelf)
            {
                return false; // If any GameObject in the wave is still active, the wave is not cleared
            }
        }
        return true; // All GameObjects in the wave are destroyed or inactive
    }

    // Activate all GameObjects in a given wave
    private void ActivateWave(int wave)
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.Item1 == wave && enemy.Item2 != null)
            {
                enemy.Item2.SetActive(true);
            }
        }
    }
}
