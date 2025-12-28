using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    public Animator anim;
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 0.8f; // Komboyu sıfırlama süresi

    void Update()
    {
        // Combo zamanlayıcısı
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
            {
                comboStep = 0;
                comboTimer = 0f;
            }
        }

        // Tuşa basıldığında
        if (Input.GetKeyDown(KeyCode.J))
        {
            ComboAttack();
        }
    }

    void ComboAttack()
    {
        comboStep++;

        if (comboStep == 1)
        {
            anim.SetTrigger("Attack1");
        }
        else if (comboStep == 2)
        {
            anim.SetTrigger("Attack2");
        }
        else if (comboStep == 3)
        {
            anim.SetTrigger("Attack3");
        }

        // 3. saldırıdan sonra başa dön
        if (comboStep >= 3)
        {
            comboStep = 0;
        }

        comboTimer = 0f; // Timer reset
    }
}
