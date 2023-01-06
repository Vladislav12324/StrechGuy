using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Cannon : Interactable<Pickable>
{
    [SerializeField] private AudioClip _shoot;
    [SerializeField] private Vector3 _shootDirection;
    [SerializeField] private AudioClip _break;

    
    public override void OnInteract(Pickable interact)
    {
        interact.UnPick();
        interact.transform.position = transform.position;
        interact.transform.parent = transform;
        Destroy(interact);
        StartCoroutine(Shoot(interact.gameObject));
    }

    private IEnumerator Shoot(GameObject bullet)
    {
        var bulletComponent = bullet.AddComponent<Bullet>();
        bulletComponent.Init(transform, _break);
        bulletComponent.Init(Audio, Vibration);

        yield return new WaitForSeconds(0.5f);
        
        Audio.Play(_shoot);
        bullet.transform.DOMove(transform.up * 25 + transform.position, 1);
    }
    
    public class Bullet : Interactable<Breakable>
    {
        private Transform _cannon;
        private AudioClip _break;

        public void Init(Transform cannon, AudioClip onBreak)
        {
            _cannon = cannon;
            _break = onBreak;
            GetComponent<Collider>().isTrigger = false;
        }

        public override void OnInteract(Breakable interact)
        {
            if(interact.transform == _cannon)
                return;

            Destroy(gameObject);
            interact.Break();
            Audio.Play(_break);
            Vibration.Vibrate();
        }
    }
}