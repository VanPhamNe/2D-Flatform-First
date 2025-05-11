using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerRespawn;
   public static PlayerManager instance;
    [Header("Player Manager")]
    public Player player;
    [SerializeField] private GameObject playerPrefab;
    public Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    private PlayerHealth playerHeal;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (respawnPoint == null)
        {
            respawnPoint = FindFirstObjectByType<Startpoint>().transform;
        }
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }
    public void RespawnPlayer()
    {
        UI_Fade fade = UI_Ingame.instance.fade;
        fade.FadeEffect(1f, 1f, StartRespawnPlayer);
    }
    public void StartRespawnPlayer()
    {
        StartCoroutine(RespawnPlayerCoroutine());
    }
    private IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
        playerHeal = newPlayer.GetComponent<PlayerHealth>();
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cam != null)
        {
            cam.Follow = newPlayer.transform;
            cam.LookAt = newPlayer.transform;
        }

        if (playerHeal != null)
        {
            playerHeal.ResetHearts();
        }
        OnPlayerRespawn?.Invoke();
        UI_Fade fade = UI_Ingame.instance.fade;
        fade.FadeEffect(0f, 1f);
    }
    public void UpdateCheckpointRespawn(Transform newrespawn)
    {
        respawnPoint = newrespawn;
    }
}
