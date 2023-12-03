using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject cylinderObject;
    [SerializeField] GameObject player;
    Image cylinder;
    List<Image> bullets = new();
    EntityClass playerEntity;

    private void Start()
    {
        playerEntity = player.GetComponent<EntityClass>();
        cylinder = cylinderObject.GetComponent<Image>();
        for (int i = 0; i < cylinderObject.transform.childCount; i++) bullets.Add(cylinderObject.transform.GetChild(i).GetComponent<Image>());
    }

    void Update()
    {
        DisplayHealth();
    }

    void DisplayHealth()
    {
        if (playerEntity.Health >= 0) healthText.text = playerEntity.Health.ToString();
        else healthText.text = "0";
    }
}
