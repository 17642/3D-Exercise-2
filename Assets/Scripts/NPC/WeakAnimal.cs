using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 _targetPos)
    {
        nav.speed = runSpeed;
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;//반대 방향 Direction 설정
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
        {
            Run(_targetPos);
        }
    }
}
