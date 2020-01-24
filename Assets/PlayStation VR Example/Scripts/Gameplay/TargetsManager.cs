using UnityEngine;
using System.Collections;

namespace PlayStationVRExample
{
    public class TargetsManager : MonoBehaviour
    {
        public float spawnRateMin = 0.5f;
        public float spawnRateMax = 2.0f;
        public GameObject targetPrefab;
        public Transform spawnPoint;
        public Vector3 spawnArea = Vector3.one;
        public int maxSpawnedObjects = 12;
        bool m_AllowSpawning = false;

        public void BeginSpawning()
        {
            m_AllowSpawning = true;
            StartCoroutine(SpawnTarget());
        }

        public void StopSpawning()
        {
            m_AllowSpawning = false;
            StopAllCoroutines();
        }

        // Overrides the delay on spawning and immediately creates a new target, used when there are few targets left
        public void SpawnTargetImmediately()
        {
            if (m_AllowSpawning && spawnPoint.childCount < 2)
                Spawn();
        }

        // Repeatedly spawns a new target over time, as long as there aren't too many objects already spawned
        IEnumerator SpawnTarget()
        {
            if (m_AllowSpawning && spawnPoint.childCount < maxSpawnedObjects)
                Spawn();

            yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));

            if (m_AllowSpawning)
                StartCoroutine(SpawnTarget());
        }

        void Spawn()
        {
            var tempTarget = Instantiate(targetPrefab, RandomSpawnPoint(), Quaternion.identity) as GameObject;
            tempTarget.transform.parent = spawnPoint.transform;
        }

        // Draws the spawn area in the editor
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnPoint.position, spawnArea);
        }

        // Gets a random spawn location inside the defined spawn area
        Vector3 RandomSpawnPoint()
        {
            var randomPosition = new Vector3(Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f),
                Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f),
                Random.Range(-spawnArea.z * 0.5f, spawnArea.z * 0.5f));
            return spawnPoint.position + randomPosition;
        }
    }
}