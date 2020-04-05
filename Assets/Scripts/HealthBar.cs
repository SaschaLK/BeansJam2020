using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image fillAmount;

    private int maxHealth = 100;

    private void Start() {
        fillAmount = GameObject.Find("HealthOverlay").GetComponent<Image>();
    }

    public void SetMaxHealth(int health) {
        maxHealth = health;
        SetHealth(health);
    }

    public void SetHealth(int health) {
        fillAmount.fillAmount = (float)health / (float)maxHealth;
    }
}
