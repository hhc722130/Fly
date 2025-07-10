using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    public int missileDamage = 1; // 미사일의 공격력

    [SerializeField]
    GameObject Expeffect;

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; // 미사일 위로 이동
        if (transform.position.y > 7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Missile hit enemy");
            GameObject effect = Instantiate(Expeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 이펙트가 1초 후에 사라짐
            Destroy(gameObject);
        }
    }
}
