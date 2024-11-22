using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventBus", menuName = "Event System/EventBus")]
public class EventBus : ScriptableObject
{
    public event Action<GameObjectColliderI> onGameObjectCollided;

    public void TriggerCollisionEvent(GameObject collidedObject1, GameObject collidedObject2)
    {
        Debug.Log("OnGameObjectCollided: " + collidedObject1.name + " " + collidedObject2.name);
        onGameObjectCollided?.Invoke(new GameObjectColliderI { collidedObject1 = collidedObject1, collidedObject2 = collidedObject2 });
    }
}
