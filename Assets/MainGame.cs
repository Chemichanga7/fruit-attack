using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    [SerializeField] private EventBus eventBus;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject cursorLine;
    [SerializeField] private GameObject cursorFigure;
    [SerializeField] private PhysicsMaterial2D material;
    private bool isMouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        eventBus.onGameObjectCollided += OnGameObjectCollided;

        GenerateFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !isMouseDown)
        {
            OnMouseDown(Input.mousePosition);
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        OnMouseMove(Input.mousePosition);
    }

    public void OnMouseDown(Vector3 mousePosition)
    {
        if (cursorFigure) AddPhysics(cursorFigure);
        GenerateFruit();
    }

    public void OnMouseMove(Vector3 mousePosition)
    {
        Vector3 cursorLinePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        cursorLinePosition.z = cursorLine.transform.position.z;
        cursorLinePosition.y = cursorLine.transform.position.y;
        cursorLine.transform.position = cursorLinePosition;

        Vector3 cursorFigurePosition = new Vector3(cursorLinePosition.x, 4, 10);

        if (cursorFigure != null) cursorFigure.transform.position = cursorFigurePosition;
    }

    private void GenerateFruit()
    {
        Vector3 cursorFigurePosition = new Vector3(cursorLine.transform.position.x, 4, 10);
        cursorFigure = CreateInstance(cursorFigurePosition, Random.Range(0, 4));
    }

    private void OnGameObjectCollided(GameObjectColliderI gameObjects)
    {
        FruitProperties fruitPropsObject1 = gameObjects.getFruitPropsObject1();
        FruitProperties fruitPropsObject2 = gameObjects.getFruitPropsObject2();

        if (fruitPropsObject1 == null || fruitPropsObject2 == null || fruitPropsObject1.isDeleted || fruitPropsObject2.isDeleted) return;

        if (fruitPropsObject1.type == fruitPropsObject2.type) {
            fruitPropsObject1.isDeleted = true;
            fruitPropsObject2.isDeleted = true;

            Destroy(gameObjects.collidedObject1);
            Destroy(gameObjects.collidedObject2);

            Debug.Log("Они одинаковые " + fruitPropsObject1.type + " " + prefabs.Length);
            if (fruitPropsObject1.type >= prefabs.Length) return;

            AddPhysics(CreateInstance(GetMiddlePoint(gameObjects.collidedObject1.transform.position, gameObjects.collidedObject2.transform.position), fruitPropsObject1.type));
        }
    }

    private GameObject CreateInstance(Vector3 position, int type) {
        GameObject prefab = prefabs[type];
        return Instantiate(prefab, position, transform.rotation);
    }

    private void AddPhysics(GameObject gameObject) {
        Rigidbody2D rigidBody = gameObject.AddComponent<Rigidbody2D>();
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.sharedMaterial = material;
    }

    private Vector3 GetMiddlePoint(Vector3 point1, Vector3 point2)
    {
        // Координаты средней точки для двумерного пространства
        return new Vector3((point1.x + point2.x) / 2, (point1.y + point2.y) / 2, 10);
    }
}
