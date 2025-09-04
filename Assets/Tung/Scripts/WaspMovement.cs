using UnityEngine;

public class BeeOrbit : MonoBehaviour
{
    public float orbitSpeed = 20f;
    public float orbitRadius = 3f;
    public float bobbingAmount = 0.5f;

    float mySpeed;
    float myRadius;
    float myAngle;
    Vector3 orbitPoint;

    void Start()
    {
        float direction = Random.Range(0, 2) == 0 ? 1f : -1f;
        mySpeed = orbitSpeed + Random.Range(-100f, 100f) * direction;
        myRadius = orbitRadius + Random.Range(-1f, 1f);

        myAngle = Random.Range(0f, 360f);
        orbitPoint = transform.position + Random.insideUnitSphere * 1f;
    }

    void Update()
    {
        myAngle += mySpeed * Time.deltaTime;
        if (myAngle > 360f)
        {
            myAngle -= 360f;
        }

        var angleInRadians = myAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleInRadians) * myRadius;
        float z = Mathf.Sin(angleInRadians) * myRadius;

        float y = Mathf.Sin(Time.time * 3f) * bobbingAmount;

        Vector3 offset = new Vector3(x, y, z);
        transform.position = orbitPoint + offset;
        
        var lookAngleInRadians = (myAngle + 1) * Mathf.Deg2Rad;
        Vector3 lookPosition = orbitPoint + new Vector3(Mathf.Cos(lookAngleInRadians) * myRadius, y, Mathf.Sin(lookAngleInRadians) * myRadius);
        transform.LookAt(lookPosition);
    }
}