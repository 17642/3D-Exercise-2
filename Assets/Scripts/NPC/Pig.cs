using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    private void RandomAction()
    {
        RandomSound();

        isAction = true;

        int _random = Random.Range(0, 4);//�� Anim  ��: 4

        if (_random == 0)
        {
            Wait();
        }
        else if (_random == 1)
        {
            Eat();
        }
        else if (_random == 2)
        {
            Peek();
        }
        else if (_random == 3)
        {
            TryWalk();
        }
    }
    private void Wait()
    {
        Debug.Log("���");
        currentTime = waitTime;
    }

    private void Eat()
    {
        Debug.Log("Ǯ���");
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }

    private void Peek()
    {
        Debug.Log("�θ���");
        currentTime = waitTime;
        anim.SetTrigger("Peek");
    }

    protected override void AnimReset()
    {
       
        base.AnimReset();
        RandomAction();
    }
}
