using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyType")]
public class EnemyTypeAsset : ScriptableObject
{
    public string id;
    public int level;

    public enum EmotionPattern { Joy, Sadness, Anger, Fear, Disgust, None };
    public EmotionPattern emotionPattern = EmotionPattern.None;

    public enum BehaviourType {  Meat, Fire, Water, Earth, Air, Plant };
    public BehaviourType behaviourType = BehaviourType.Meat;

    [Header("Behaviour")]
    public float maxHealth = 3f;
    public GameObject behaviourPrefab;

    [Serializable]
    public struct Loot
    {
        public string ingredientId;
        public int minQuantity;
        public int maxQuantity;
    }
    public Loot[] lootTable;
}
