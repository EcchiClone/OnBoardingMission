using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _damageText;
    [SerializeField] float riseSpeed = 1f;
    [SerializeField] float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(FadeOutAndRise());
    }

    public void SetDamageText(int damage)
    {
        string damageText = damage != 0 ? damage.ToString() : "MISS";
        _damageText.text = damageText;
    }

    private IEnumerator FadeOutAndRise()
    {
        float elapsedTime = 0f;
        Color originalColor = _damageText.color;

        while (elapsedTime < fadeDuration)
        {
            // 오브젝트 위 방향으로 위치 이동
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // 알파 값 100%에서 0%로 이동
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            _damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // fadeDuration초 후 파괴
        Destroy(gameObject);
    }
}
