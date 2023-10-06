using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    void En_TakeDamage(int damage);

    void En_CheckDeath();
}
