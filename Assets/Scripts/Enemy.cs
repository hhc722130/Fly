using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject damageTextPrefab; // 피해 텍스트 프리팹
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트
    private Color flashColor = Color.red; // 플래시 색상
    public float flasgDuration = 0.1f;
    private Color originalColor; // 원래 색상
    public float enemyHP = 1f;

    [SerializeField]
    public float moveSpeed = 1f; // 적 이동 속도
    public GameObject Coin;
    public GameObject Effect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 원래 색상 저장
    }

    // Update is called once per frame
    void Flash()
    {
        StopAllCoroutines(); // 모든 코루틴 중지
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = flashColor; // 색상 변경
        yield return new WaitForSeconds(flasgDuration); // 잠시 대기
        spriteRenderer.color = originalColor; // 원래 색상으로 복원
    }
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime; // 적 아래로 이동
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject); // 적이 화면 아래로 내려가면 제거
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile"))
        {
            Missile missile = collision.GetComponent<Missile>();
            StopAllCoroutines();
            StartCoroutine("HitColor");
            // Flash(); 

            enemyHP = enemyHP - missile.missileDamage; // 적의 HP 감소
            if (enemyHP <= 0)
            {
                Destroy(gameObject); // 적 제거
                Instantiate(Coin, transform.position, Quaternion.identity); // 코인 생성
                Instantiate(Effect, transform.position, Quaternion.identity); // 이펙트 생성
            }
            TakeDamage(missile.missileDamage); // 피해 처리
        }
    }

    IEnumerator HitColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red; // 적의 색상을 빨간색으로 변경
        yield return new WaitForSeconds(0.2f); // 0.1초
        spriteRenderer.color = Color.white; // 다시 원래 색상으로 변경
    }

    void TakeDamage(int damage)
    {
        DamagePopupManager.Instance.CreateDamageText( damage, transform.position); // 피해 텍스트 생성
    }
}
