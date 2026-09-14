using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float currentHealth;
    private Transform player;
    private EnemyBehaviour behaviour;
    private EnemyTypeAsset enemyType;

    public void Initialize(EnemyTypeAsset type)
    {
        enemyType = type;
        GameObject behaviourGO = Instantiate(type.behaviourPrefab, transform);
        behaviour = behaviourGO.GetComponent<EnemyBehaviour>();
        if (behaviour == null) Debug.LogError("Behaviour prefab has no EnemyBehaviour!");

        currentHealth = type.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        behaviour?.OnUpdate();

        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        behaviour?.OnFixedUpdate();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f) //Add death animation later
        {
            foreach(var loot in enemyType.lootTable)
            {
                int lootQuantity = Random.Range(loot.minQuantity, loot.maxQuantity + 1);
                if (lootQuantity > 0) IngredientFactory.AddIngredient(loot.ingredientId, lootQuantity, "Loot");
            }
            EnemyFactory.IncBeastKillCount(enemyType.id, "Spruce_forest"); //Пока что просто затычка
            EnemySpawner.Instance.isItVictory(this);
            Destroy(gameObject);
        }
    }
}
