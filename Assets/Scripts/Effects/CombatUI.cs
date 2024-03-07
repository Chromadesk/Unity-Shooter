using Abilities;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Texture2D cursor;

    private void Start()
    {
        Cursor.SetCursor(cursor, new Vector2(Mathf.Floor(cursor.width / 2), Mathf.Round(cursor.height / 2)), CursorMode.Auto);
    }

    public void DisplayHealth(float health)
    {
        if (health >= 0) healthText.text = health.ToString();
        else healthText.text = "0";
    }
}
