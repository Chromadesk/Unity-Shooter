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
    Image cylinder;
    List<Image> bullets = new();
    int bulletIndex = 0;

    private void Start()
    {
        cylinder = cylinderObject.GetComponent<Image>();
        for (int i = 0; i < cylinderObject.transform.childCount; i++)
        {
            bullets.Add(cylinderObject.transform.GetChild(i).GetComponent<Image>());
        }   
    }

    public void RemoveUIAmmo(float cooldownTime)
    {
        bullets[bulletIndex].enabled = false;
        cylinder.rectTransform.LeanRotateZ(Mathf.Round(cylinder.rectTransform.eulerAngles.z + 60), cooldownTime - 0.05f);
        bulletIndex++;
    }

    public void AddUIAmmo(float rotationTime)
    {
        bulletIndex--;
        bullets[bulletIndex].enabled = true;
        cylinder.rectTransform.LeanRotateZ(Mathf.Round(cylinder.rectTransform.eulerAngles.z - 60), rotationTime - 0.05f);
    }

    public void DisplayHealth(float health)
    {
        if (health >= 0) healthText.text = health.ToString();
        else healthText.text = "0";
    }
}
