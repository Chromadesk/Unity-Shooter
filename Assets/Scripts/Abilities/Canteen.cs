using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Canteen : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    public readonly int maxCharge = 40;
    readonly int maxOverCharge = 80;
    public int currentCharge = 0;
    readonly float damageDealtCharge = 0.1f; //Percentage
    readonly float damageRecievedCharge = 0.1f; //Percentage
    readonly float cooldown = 5;
    bool onCooldown = false;
    float decayTime;

    private void Start()
    {
        decayTime = Time.time;
    }

    private void Update()
    {
        DecayOverharge();
    }

    public void Use()
    {
        if (currentCharge <= 0 || onCooldown) return;

        if (player.Health + currentCharge > player.maxHealth) player.Health = player.maxHealth; 
        else player.Health += currentCharge;

        currentCharge = 0;
        onCooldown = true;
        Invoke(nameof(ResetCooldown), cooldown);
    }

    public void Charge(float amount, bool damDealt)
    {
        if (currentCharge >= maxOverCharge) return;

        float chargeMultiplier;

        if (damDealt) chargeMultiplier = damageDealtCharge; else chargeMultiplier = damageRecievedCharge;

        int addCharge = (int)Mathf.Round(amount * chargeMultiplier);

        if (addCharge + currentCharge > maxOverCharge) currentCharge = maxOverCharge; else currentCharge += addCharge;
    }

    void DecayOverharge()
    {
        if (currentCharge <= maxCharge || Time.time - decayTime < 2) return;
        currentCharge -= 1;
        decayTime = Time.time;
    }

    void ResetCooldown() { onCooldown = false; }
}
