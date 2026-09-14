using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MixPotionMode : MonoBehaviour
{
    public static MixPotionMode Instance;

    public Transform ingButtonsContent;
    public Transform resultButtonContent;
    public GameObject mixSlotPref;

    private GameObject leftIngredientButton;
    private GameObject rightIngredientButton;
    private GameObject resultButton;

    private MixSlot leftIngredient;
    private MixSlot rightIngredient;
    private MixSlot result;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitIngButton(out leftIngredientButton, out leftIngredient, MixDropZone.DropZone.Left);
        leftIngredientButton.GetComponent<Button>().onClick.AddListener(() => { leftIngredient.Clear(); });

        InitIngButton(out rightIngredientButton, out rightIngredient, MixDropZone.DropZone.Right);
        rightIngredientButton.GetComponent<Button>().onClick.AddListener(() => { rightIngredient.Clear(); });

        resultButton = Instantiate(mixSlotPref, resultButtonContent);
        resultButton.GetComponent<Button>().onClick.AddListener(OnResultButtonClick);
        result = resultButton.GetComponent<MixSlot>();
        result.SetSlotScale();

        ClearAllIngredients();
    }

    private void InitIngButton(out GameObject button, out MixSlot mixSlot, MixDropZone.DropZone zone)
    {
        button = Instantiate(mixSlotPref, ingButtonsContent);
        mixSlot = button.GetComponent<MixSlot>();
        button.AddComponent<MixDropZone>();
        button.GetComponent<MixDropZone>().zone = zone;
    }

    public void SetIngredient(StorageContentData ingredient, bool leftIng)
    {
        if (ingredient.data.type == GILData.IngredientType.Food || ingredient.data.type == GILData.IngredientType.Dish || ingredient.data.type == GILData.IngredientType.Other)
        {
            Description.Instance.ShowMessage("This ingredient cannot be used for potion crafting.");
            return;
        }
        
        if (leftIng) leftIngredient.Setup(ingredient);
        else rightIngredient.Setup(ingredient);

        UpdateResult();
    }

    public void UpdateResult()
    {
        if (leftIngredient.GetIng() == null || rightIngredient.GetIng() == null)
        {
            result.Clear();
            return;
        }

        Debug.Log("Mixing: " + leftIngredient.GetIng().data.id + " and " + rightIngredient.GetIng().data.id + "\n" +
            "(Count: " + leftIngredient.GetIng().count + ", " + rightIngredient.GetIng().count + ")");
        StorageContentData finalIngredient = Mix(leftIngredient.GetIng(), rightIngredient.GetIng());

        if (finalIngredient != null) result.Setup(finalIngredient);
        else result.Clear();
    }

    public void ClearAllIngredients()
    {
        leftIngredient.Clear();
        rightIngredient.Clear();
        result.Clear();
    }

    public bool IsPossibleToSetLeft ()
    {
        return leftIngredient.GetIng() == null;
    }

    private StorageContentData Mix(StorageContentData a, StorageContentData b)
    {
        //Вода + Огонь -> Пар
        if ((a.data.id == "water" && b.data.id == "fire") || (a.data.id == "fire" && b.data.id == "water"))
        {
            StorageContentData steam = new StorageContentData();
            steam.data = new GILData();
            steam.data.id = "steam";
            steam.data.ingName = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", steam.data.id);
            steam.data.description = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsDescriptionsLocalization", steam.data.id);
            steam.icon = Resources.Load<Sprite>("IngredientsSprites/steam");
            return steam;
        }

        //Вода + X
        if (a.data.id == "water" && b.count > 0)
        {
            if (b.data.density <= 1) return null;
            return NewIngredient(b, b.data.density - 1);
        }
        if (b.data.id == "water" && a.count > 0)
        {
            if (a.data.density <= 1) return null;
            return NewIngredient(a, a.data.density - 1);
        }

        //Огонь + X
        if (a.data.id == "fire")
        {
            if (b.data.densityLimit < b.data.density + 1) return null;
            if (b.count < 2) Description.Instance.ShowMessage("You need more of this ingredient");
            return NewIngredient(b, b.data.density + 1);
        }
        if (b.data.id == "fire")
        {
            if (a.data.densityLimit < a.data.density + 1) return null;
            if (a.count < 2) Description.Instance.ShowMessage("You need more of this ingredient");
            return NewIngredient(a, a.data.density + 1);
        }

        //X + Y -> Зелье
        if (a.data.id != b.data.id) return CreatePotion(a, b);

        return null;
    }

    private StorageContentData NewIngredient(StorageContentData baseIng, int newDensity)
    {
        var newIngredient = new StorageContentData();
        newIngredient.data = new GILData()
        {
            type = baseIng.data.type,
            description = baseIng.data.description,
            effect = baseIng.data.effect,
            effectIds = new List<string>(baseIng.data.effectIds),
            densityLimit = baseIng.data.densityLimit,
            recipe = new List<IngredientData>(baseIng.data.recipe),
            conflictIds = new List<string>(baseIng.data.conflictIds)
        };

        newIngredient.data.density = newDensity;

        string iconPath = "default";
        if (baseIng.data.type == GILData.IngredientType.Loot)
        {
            newIngredient.data.ingName = baseIng.data.ingName.Replace(NumbersConvertor.ToRoman(baseIng.data.density), "") + NumbersConvertor.ToRoman(newIngredient.data.density);
            newIngredient.data.id = baseIng.data.id.Substring(0, baseIng.data.id.Length - 1) + (newIngredient.data.density - 1).ToString();
            iconPath = "IngredientsSprites/" + newIngredient.data.id.Substring(0, newIngredient.data.id.Length - 2);
        }
        else if (baseIng.data.type == GILData.IngredientType.Potion)
        {
            string newIngredientId = GILFactory.FindIdForPotion(newIngredient.data);
            newIngredient.data.ingName = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", "potion") + " " + newIngredientId;
            newIngredient.data.id = newIngredientId;
            iconPath = "IngredientsSprites/" + newIngredient.data.id;
        }

        if (Resources.Load<Sprite>(iconPath) != null) newIngredient.icon = Resources.Load<Sprite>(iconPath);
        else newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/default");

        return newIngredient;
    }

    private StorageContentData CreatePotion(StorageContentData a, StorageContentData b)
    {
        var potion = new StorageContentData();
        potion.data = new GILData();
        potion.data.recipe = new List<IngredientData>();

        potion.data.density = (int)Math.Ceiling((a.data.density + b.data.density) / 2.0);
        potion.data.densityLimit = (int)Math.Ceiling((a.data.densityLimit + b.data.densityLimit) / 2.0);

        if (a.data.type == GILData.IngredientType.Loot && a.data.id.Substring(0, a.data.id.Length - 1) == b.data.id.Substring(0, b.data.id.Length - 1))
        {
            potion.icon = Resources.Load<Sprite>("IngredientsSprites/" + a.data.id.Substring(0, a.data.id.Length - 2));
            potion.data.id = a.data.id.Substring(0, a.data.id.Length - 1) + (potion.data.density - 1).ToString();
            potion.data.ingName = a.data.ingName.Replace(NumbersConvertor.ToRoman(a.data.density), "") + "+ " + b.data.ingName.Replace(" " + NumbersConvertor.ToRoman(b.data.density), "");
            potion.data.type = GILData.IngredientType.Loot;
            potion.data.description = a.data.description;
            potion.data.effect = a.data.effect;
            potion.data.effectIds.AddRange(a.data.effectIds);
        }
        else
        {
            potion.icon = Resources.Load<Sprite>("IngredientsSprites/default");
            potion.data.type = GILData.IngredientType.Potion;
            potion.data.description = "???";
            potion.data.effect = "???";

            var dict = new Dictionary<string, int>();

            void AddRecipe(StorageContentData ing)
            {
                if (ing.data.type == GILData.IngredientType.Loot)
                {
                    AddIngredient(ing.data.id, 1);
                }
                else
                {
                    foreach (var r in ing.data.recipe)
                    {
                        AddIngredient(r.id, r.count);
                    }
                }
            }
            void AddIngredient(string id, int count)
            {
                if (!dict.ContainsKey(id)) dict[id] = 0;
                dict[id] += count;
            }

            AddRecipe(a);
            AddRecipe(b);
            foreach (var pair in dict)
            {
                potion.data.recipe.Add(new IngredientData
                {
                    id = pair.Key,
                    count = pair.Value
                });
            }
            foreach (var ing in potion.data.recipe)
            {
                Debug.Log("RECIPE: " + ing.id + " - " + ing.count);
            }

            potion.data.effectIds.AddRange(a.data.effectIds);
            foreach (var effect in b.data.effectIds)
            {
                if (!a.data.effectIds.Contains(effect))
                {
                    potion.data.effectIds.Add(effect);
                }
            }
            foreach (var effect in potion.data.effectIds)
            {
                Debug.Log("EFFECT: " + effect);
            }

            string newPotionId = GILFactory.FindIdForPotion(potion.data);
            potion.data.id = newPotionId;
            potion.data.ingName = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", "potion") + " " + newPotionId;
        }

        return potion;
    }

    public void OnResultButtonClick()
    {
        StorageContentData finalIngredient = result.GetIng();
        StorageContentData left = leftIngredient.GetIng();
        StorageContentData right = rightIngredient.GetIng();
        if (finalIngredient == null || finalIngredient.data.id == "steam") return;
        finalIngredient.count = 1;
        if (left.data.id == "fire" || right.data.id == "fire")
        {
            if (left.data.id == "fire")
            {
                if (right.count < 2)
                {
                    ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания", false);
                    return;
                }
                IngredientFactory.RemoveIngredient(right.data.id, 2, ScenesConfig.IsHome);
                right.count -= 2;
            }
            else
            {
                if (left.count < 2)
                {
                    ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания", false);
                    return;
                }
                IngredientFactory.RemoveIngredient(left.data.id, 2, ScenesConfig.IsHome);
                left.count -= 2;
            }
        }
        else
        {
            if (left.data.id == "water" && right.count > 0)
            {
                IngredientFactory.RemoveIngredient(right.data.id, 1, ScenesConfig.IsHome);
                finalIngredient.count = 2;
                right.count -= 1;
            }
            else if (right.data.id == "water" && left.count > 0)
            {
                IngredientFactory.RemoveIngredient(left.data.id, 1, ScenesConfig.IsHome);
                finalIngredient.count = 2;
                left.count -= 1;
            }
            else if (left.count < 1 || right.count < 1)
            {
                ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания", false);
                return;
            }
            else
            {
                IngredientFactory.RemoveIngredient(left.data.id, 1, ScenesConfig.IsHome);
                IngredientFactory.RemoveIngredient(right.data.id, 1, ScenesConfig.IsHome);
                left.count -= 1;
                right.count -= 1;
            }
        }
        if (left.data.type == GILData.IngredientType.Potion || right.data.type == GILData.IngredientType.Potion)
        {
            finalIngredient.count = 2;
        }
        GILFactory.CreateIngredient(finalIngredient.data);
        string homeOrCamp = ScenesConfig.IsHome ? "Home" : "Camp";
        IngredientFactory.AddIngredient(finalIngredient.data.id, finalIngredient.count, homeOrCamp);

        leftIngredient.Setup(left, true);
        rightIngredient.Setup(right, true);
        UpdateResult();
    }
}
