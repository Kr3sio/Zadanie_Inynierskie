
using UnityEngine;

public class Enemy_Respowner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoint;

    public bool visited = false;
   

    private Transform player;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>().transform;
        

    }

    private void Update()
    {
        


    }

    public void CreateNewEnemy()
    {
        //int respawnPointIndex = Random.Range(0, respawnPoint.Length);
        //Vector3 spawnPoint = respawnPoint[respawnPointIndex].position;

        //GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        //bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        foreach (Transform point in respawnPoint)
        {
            // Jeśli z jakiegoś powodu zapomnisz przypisać punkt, kod go pominie bez wywalania błędu
            if (point == null) continue;

            // Tworzymy przeciwnika dokładnie w miejscu obecnego punktu z pętli
            GameObject newEnemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);

            // Twoja linijka sprawdzająca pozycję (zadziała teraz dla każdego stworzonego wroga)
            bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;
        }
    }



    public void SpawnEnemies()
    {
        
        if (!visited)
        {
            Debug.Log("Gracz wszedł do pokoju");
            CreateNewEnemy();
            visited = true;
        }
    }
}
