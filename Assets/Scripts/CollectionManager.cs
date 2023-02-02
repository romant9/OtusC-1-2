using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public int speed;
    void Start()
    {
        test.onLifeChange += LifeChange;
    }

    void Update()
    {
        
    }
    public CollectionsClass test;
    private void OnHit()
    {
        speed -= 2;
        test.onLifeChange.Invoke(speed);
    }
    public void LifeChange(int speed)
    {
    }
}
