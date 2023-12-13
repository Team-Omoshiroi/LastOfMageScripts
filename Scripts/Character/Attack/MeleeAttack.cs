using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : BaseAttack
{
    [SerializeField]
    private float speed = 1f; // 공격의 속도

    [SerializeField]
    private float lifeTime = 0.3f; // 공격이 존재할 수 있는 시간
    private List<GameObject> _obstacle;

    private void Awake()
    {
        _obstacle = new List<GameObject>();
    }

    public override void Initalize(AttackInfo attackInfo, DataContainer dataContainer, string tag)
    {
        base.Initalize(attackInfo, dataContainer, tag);
        // 추가적으로 해야되는 작업
        Invoke(nameof(Disactive), lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Disactive()
    {
        gameObject.SetActive(false);
        _obstacle.Clear();
    }

    public override bool ApplyDamage(HealthSystem health)
    {
        Damage = _data.Stats.Atk;
        // 구체적인 대미지 계산을 맡기기
        // Stats에 대미지 계산 메소드를 만든다.
        // Damage = dataContainer.Stats.GetDamage(health);
        // base.ApplyDamage(health);
        return health.TakeDamage(Damage);
        // TakeDamage에서 방어력에 따라서 입는 대미지를 줄인다.
        //gameObject.SetActive(false);
        // TODO
        // 피격음 재생
    }

    /// <summary>
    /// 플래이어, 벽, 물체
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_makerTag))
        {
            if ((1 << other.gameObject.layer) == (1 << other.gameObject.layer & AttackManager.Instance.TargetLayer.value))
            {
                // Ray를 쏴서 처음 물체가 other 경우에만 대미지
                Ray ray = new Ray(
                    gameObject.transform.position,
                    other.gameObject.transform.position - gameObject.transform.position
                );
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.gameObject.Equals(other.gameObject))
                    {
                        var data = other.GetComponent<DataContainer>();
                        ApplyDamage(data.Health);
                        // TODO
                        // NPC는 어떻게 감지해야되는지 고민
                    }
                }
            }
        }
    }
}
