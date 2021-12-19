using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawningSystem : MonoBehaviour
{
    public Transform m_count;
    public Transform[] spawnPoints;
    public GameObject enemyModel;
    bool isDeployed = false;
    public int TimeRemeaning = 30;
    public bool isInvoked = false;
    public Text enemyDisp;
    public EnemyAiTutorial en;
    public Target tar;
    public int waveIndex = 0;
    public Text waveNums;

    public Text SuccessText;
    public AudioSource codmw2lvlUp;

    public float timeLeftForStackUp = 30;


    // Start is called before the first frame update
    public void GO()
    {
        isDeployed = true;
    }

    // Update is called once per frame
    void Update()
    {
        waveNums.text = "" + waveIndex;
        if(m_count.childCount == 0 && isDeployed == true && isInvoked == false)
        {
            StartCoroutine("SpawnE");
            Invoke(nameof(SpawnE), TimeRemeaning);
            isInvoked = true;
            if(waveIndex > 0)
            {
                codmw2lvlUp.Play();
                SuccessText.text = "Wave " + waveIndex + " Survived !";
                Invoke(nameof(RemoveText), 5f);
            }


        }

        enemyDisp.text = "Enemies Remeaning: " + m_count.childCount;
        
    }

    public void SpawnE()
    {
        if(m_count.childCount <= 18 && isInvoked == true)
        {
            int RP = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            if(waveIndex > 7)
            {
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            }

            if(waveIndex > 20)
            {
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            Instantiate(enemyModel, spawnPoints[RP].position, spawnPoints[RP].rotation , m_count);
            }
            isInvoked = false;
            waveIndex++;

        }

    }

    public void RemoveText()
    {
        SuccessText.text = "";
    }
}
