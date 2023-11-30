using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject player;
    EntityClass playerEntity;

    private void Start()
    {
        playerEntity = player.GetComponent<EntityClass>();
    }

    void Update()
    {
        healthText.text = playerEntity.Health.ToString();
    }
}
