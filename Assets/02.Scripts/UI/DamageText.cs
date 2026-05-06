using UnityEngine;
using TMPro;

public class DamageText : PoolAble
{
    public float moveSpeed = 1f;
    public float lifeTime = 0.5f;

    private TextMeshPro textMesh;
    private float initialLifeTime;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        initialLifeTime = lifeTime;
    }

    void OnEnable()
    {
        lifeTime = initialLifeTime;
    }

    public void SetDamage(int damage)
    {
        if (textMesh != null)
            textMesh.text = damage.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            ReleaseObject();
    }
}
