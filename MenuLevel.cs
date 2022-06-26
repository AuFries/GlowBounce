using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevel : MonoBehaviour
{

    public GameObject platformPrefab;
    private Transform player;

    private float spawnHeight = 50f;
    private float heightToGenerate;
    private float accessibleHeight;

    private float xPos;
    private float yPos;

    private List<GameObject> platforms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject.transform;
        xPos = 1.5f;
        yPos = -4.5f;
    }

    // Update is called once per frame
    void Update()
    {
        heightToGenerate = player.position.y + spawnHeight;
        GeneratePlatforms();
        RemovePlatforms();
    }


    private void GeneratePlatforms()
    {

        if (accessibleHeight < heightToGenerate) //Generate a platform if inacessible
        {
            
            Vector3 spawnPosition = new Vector3(xPos,yPos);
            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);

            platforms.Add(newPlatform);
            accessibleHeight = spawnPosition.y + 2.8f;
            xPos = -xPos;
            yPos += 2.8f;
        }

    }


    private void RemovePlatforms()
    {

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[i];

            if (platform == null)
            {
                platforms.Remove(platform);
            }
            else if (platform.transform.position.y <= player.transform.position.y - Camera.main.orthographicSize - 5f)
            {
                platforms.Remove(platform);
                Destroy(platform);
            }
        }
    }
}
