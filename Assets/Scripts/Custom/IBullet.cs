using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    void Fire();
}

public class LaserBullet : IBullet
{
    public void Fire()
    {
        Debug.Log("Firing laser bullet.");
    }
}

public class StandardBullet : IBullet
{
    public void Fire()
    {
        Debug.Log("Firing standard bullet.");
    }
}
