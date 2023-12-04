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
    [SerializeField] Texture2D cursor;
    Image cylinder;
    List<Image> bullets = new();
    int bulletIndex = 0;

    private void Start()
    {
        //Set bullet gui
        cylinder = cylinderObject.GetComponent<Image>();
        for (int i = 0; i < cylinderObject.transform.childCount; i++)
        {
            bullets.Add(cylinderObject.transform.GetChild(i).GetComponent<Image>());
        }

        Cursor.SetCursor(cursor, new Vector2(Mathf.Floor(cursor.width / 2), Mathf.Round(cursor.height / 2)), CursorMode.Auto);
    }

    public void RemoveUIAmmo(float cooldownTime)
    {
        bullets[bulletIndex].enabled = false;
        cylinder.rectTransform.LeanRotateZ(Mathf.Floor(cylinder.rectTransform.eulerAngles.z + 60), cooldownTime - 0.05f);
        bulletIndex++;
    }

    public void AddUIAmmo(float rotationTime)
    {
        bulletIndex--;
        bullets[bulletIndex].enabled = true;
        cylinder.rectTransform.LeanRotateZ(Mathf.Floor(cylinder.rectTransform.eulerAngles.z - 60), rotationTime - 0.05f);
    }

    int spins = 0;
    float spinTime;
    public void SpinCylinder(float aSpinTime)
    {
        spinTime = aSpinTime / 4;
        SpinLoop();
    }
    void SpinLoop()
    {
        float cylinderZ = cylinder.rectTransform.eulerAngles.z;
        cylinder.rectTransform.LeanRotateZ(Mathf.Floor(cylinderZ + 180), spinTime - 0.02f); //Added to prevent overspinning
        spins++;

        if (spins < 2) { Invoke(nameof(SpinLoop), spinTime); return; }

        spins = 0;
    }

    public void DisplayHealth(float health)
    {
        if (health >= 0) healthText.text = health.ToString();
        else healthText.text = "0";
    }
}
