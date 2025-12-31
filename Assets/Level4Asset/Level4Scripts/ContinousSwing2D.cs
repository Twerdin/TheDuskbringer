using UnityEngine;

/// <summary>
/// Bu script, bir HingeJoint2D bileşenini kontrol ederek objenin 2D'de sürekli sallanmasını sağlar.
/// Script'i HingeJoint2D bileşenine sahip olan GameObject'e ekleyin.
/// </summary>
public class ContinuousSwing2D : MonoBehaviour
{
    [Tooltip("Sallanma hareketini kontrol eden HingeJoint2D bileşeni.")]
    public HingeJoint2D hinge;

    [Tooltip("Sallanma hareketinin hızı.")]
    public float speed = 100f;

    [Tooltip("Objenin merkez noktasından itibaren maksimum kaç derece sallanacağı.")]
    public float swingAngleLimit = 45f;

    private JointMotor2D motor;

    void Start()
    {
        // HingeJoint2D bileşeni atanmamışsa onu bul
        if (hinge == null)
        {
            hinge = GetComponent<HingeJoint2D>();
        }

        if (hinge == null)
        {
            // HATA DÜZELTİLDİ: Mesaj tek satıra alındı
            Debug.LogError("ContinuousSwing2D script'i için bir HingeJoint2D bileşeni gerekli.");
            this.enabled = false;
            return;
        }

        // Motoru etkinleştir ve başlangıç ayarlarını yap
        motor = hinge.motor;
        motor.maxMotorTorque = 10000; // Yeterli tork
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    void Update()
    {
        if (hinge == null) return;

        // Joint'in anlık açısını al (2D için jointAngle kullanılır)
        float currentAngle = hinge.jointAngle;

        // Joint'ten anlık motor ayarlarını al (Struct olduğu için kopyasını alırız)
        JointMotor2D currentMotor = hinge.motor;

        // --- YÖN KONTROLÜ ---

        // 1. Sağa çok gittiyse ve hala sağa gidiyorsa -> Sola çevir
        if (currentAngle > swingAngleLimit && currentMotor.motorSpeed > 0)
        {
            currentMotor.motorSpeed = -Mathf.Abs(speed);
        }
        // 2. Sola çok gittiyse ve hala sola gidiyorsa -> Sağa çevir
        else if (currentAngle < -swingAngleLimit && currentMotor.motorSpeed < 0)
        {
            currentMotor.motorSpeed = Mathf.Abs(speed);
        }
        // 3. Limitlerin içindeysek
        else
        {
            // İYİLEŞTİRME: Oyun esnasında 'speed' değişirse hemen uygula.
            // Mevcut yönü koru (Sign), sadece hızı güncelle.
            if (currentMotor.motorSpeed != 0)
            {
                 currentMotor.motorSpeed = Mathf.Sign(currentMotor.motorSpeed) * Mathf.Abs(speed);
            }
            else
            {
                // Eğer motor duruyorsa hareketi başlat
                currentMotor.motorSpeed = speed;
            }
        }

        // Değişikliği joint'e uygula
        hinge.motor = currentMotor;
    }
}