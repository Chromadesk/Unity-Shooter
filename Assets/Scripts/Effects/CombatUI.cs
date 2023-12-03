using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class CombatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject cylinderObject;
    Image cylinder;
    List<Image> bullets = new();

    private void Start()
    {
        cylinder = cylinderObject.GetComponent<Image>();
        for (int i = 0; i < cylinderObject.transform.childCount; i++)
        {
            bullets.Add(cylinderObject.transform.GetChild(i).GetComponent<Image>());
        }   
    }

    int bulletIndex = 0;
    public void RemoveUIAmmo(float rotationTime)
    {
        bullets[bulletIndex].enabled = false;
        cylinder.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Round(cylinder.rectTransform.eulerAngles.z + 60));
        bulletIndex++;
    }

    public void AddUIAmmo(float rotationTime)
    {
        bulletIndex--;
        bullets[bulletIndex].enabled = true;
        cylinder.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Round(cylinder.rectTransform.eulerAngles.z - 60));
    }
    public void DisplayHealth(float health)
    {
        if (health >= 0) healthText.text = health.ToString();
        else healthText.text = "0";
    }
}
