using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static Player player;
    public static GameMaster gm;
    public SmallSlimeAI smallSlimeOne;
    public SmallSlimeAI smallSlimeTwo;
    public EnemyAI slime;
    public LevelLoader levelLoader;
    public Animator anim;
    public UiController ui;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public GameObject endScreen;


    void Start()
    {
        Time.timeScale = 1f;
        if (gm == null)
        {
            gm = this;
        }


        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);

    }

    private void Update()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies.Length <= 0)
        {
            StartCoroutine("PuddleRecede");
        }
    }

    IEnumerator PuddleRecede()
    {
        yield return new WaitForSeconds(1f);
        ui.EndScreen(true);
        endScreen.SetActive(true);
    }
}
