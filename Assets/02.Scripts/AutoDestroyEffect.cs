using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyEffect : MonoBehaviour //이펙트 일정시간 지나면 자동 삭제
{
    public float lifeTime = 1.5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}