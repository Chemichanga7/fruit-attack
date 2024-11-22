using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectColliderI 
{
    public GameObject collidedObject1;
    public GameObject collidedObject2;

    public FruitProperties getFruitPropsObject1() {
        return collidedObject1.GetComponent<FruitProperties>();
    }

    public FruitProperties getFruitPropsObject2() {
        return collidedObject2.GetComponent<FruitProperties>();
    }
}
