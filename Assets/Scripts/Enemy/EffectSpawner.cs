using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effectPrefab;

    private ObjectPool<ParticleSystem> _pool;
    private List<ParticleSystem> _activeElements = new();
    private int _poolCapacity = 20;
    private int _poolMaxSize = 50;

    private void Awake()
    {
        _pool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(_effectPrefab),
        actionOnGet: ps =>
        {
            ps.gameObject.SetActive(true);
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play(true);
        },
        actionOnRelease: ps =>
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.gameObject.SetActive(false);
        },
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize
        );
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        while (_activeElements.Count > 0)
        {
            _pool.Release(_activeElements[0]);
        }

        _activeElements.Clear();
    }

    public ParticleSystem Spawn(Vector3 position, Quaternion rotation, Transform parent)
    {
        ParticleSystem particleSystem = _pool.Get();
        particleSystem.transform.SetPositionAndRotation(position, rotation);
        particleSystem.transform.SetParent(parent);

        TryAddToActiveList(particleSystem);
        StartCoroutine(ReleaseWhenFinished(particleSystem));

        return particleSystem;
    }

    private IEnumerator ReleaseWhenFinished(ParticleSystem particleSystem)
    {
        while(particleSystem.IsAlive(true))
           yield return null;

        TryRemoveFromActiveList(particleSystem);
        _pool.Release(particleSystem);
    }

    private bool TryAddToActiveList(ParticleSystem particleSystem)
    {
        if (_activeElements.Contains(particleSystem))
            return false;

        _activeElements.Add(particleSystem);
        return true;
    }

    private bool TryRemoveFromActiveList(ParticleSystem particleSystem)
    {
        if (_activeElements.Contains(particleSystem) == false)
            return false;

        _activeElements.Remove(particleSystem);
        return true;
    }
}
