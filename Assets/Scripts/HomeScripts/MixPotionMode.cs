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
        leftIngredient = Instantiate(mixSlotPref, ingButtonsContent).GetComponent<MixSlot>();
        rightIngredient = Instantiate(mixSlotPref, ingButtonsContent).GetComponent<MixSlot>();

        leftIngredient.gameObject.AddComponent<MixDropZone>().zone = MixDropZone.DropZone.LeftPotion;
        rightIngredient.gameObject.AddComponent<MixDropZone>().zone = MixDropZone.DropZone.RightPotion;

        leftIngredient.GetComponent<Button>().onClick.AddListener(() => { leftIngredient.Clear(true); });
        rightIngredient.GetComponent<Button>().onClick.AddListener(() => { rightIngredient.Clear(true); });

        resultButton = Instantiate(mixSlotPref, resultButtonContent);
        resultButton.GetComponent<Button>().onClick.AddListener(OnResultButtonClick);
        result = resultButton.GetComponent<MixSlot>();
        result.SetSlotScale();

        ClearAllIngredients();
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

        if (a.data.type == GILData.IngredientType.Catalyst && b.data.type != GILData.IngredientType.Catalyst) 
            return NewIngredient(b, a); //REFACTOR
        else if (b.data.type == GILData.IngredientType.Catalyst && a.data.type != GILData.IngredientType.Catalyst) 
            return NewIngredient(a, b); //REFACTOR

        //X + Y -> Зелье
        if (a.data.id != b.data.id) return CreatePotion(a, b);

        return null;
    }

    private StorageContentData NewIngredient(StorageContentData baseIng, StorageContentData catalyst)
    {
        CatalystEffectInterface effect = EffectRegistry.GetCatalystEffect(catalyst.data.effectIds[0]);
        if (baseIng.count < effect.GetNeededCount()) Description.Instance.ShowMessage("You need more of this ingredient");

        StorageContentData newIngredient = new StorageContentData();
        newIngredient.data = new GILData()
        {
            type = baseIng.data.type,
            description = baseIng.data.description,
            effect = baseIng.data.effect,
            effectIds = new List<string>(baseIng.data.effectIds),
            density = baseIng.data.density,
            densityLimit = baseIng.data.densityLimit,
            price = baseIng.data.price,
            recipe = new List<IngredientData>(baseIng.data.recipe),
            conflictIds = new List<string>(baseIng.data.conflictIds),
        };

        effect.MixPotion(newIngredient);
        if (newIngredient == null) return null;

        string iconPath = "default";
        if (baseIng.data.type == GILData.IngredientType.Loot)
        {
            newIngredient.data.ingName = baseIng.data.ingName.Replace(NumbersConvertor.ToRoman(baseIng.data.density), "") + NumbersConvertor.ToRoman(newIngredient.data.density);
            newIngredient.data.id = baseIng.data.id.Substring(0, baseIng.data.id.Length - 1) + (newIngredient.data.density - 1).ToString();
            iconPath = "IngredientsSprites/" + newIngredient.data.id.Substring(0, newIngredient.data.id.Length - 2);
        }
        else if (baseIng.data.type == GILData.IngredientType.Potion)
        {
            string newIngredientId = GILFactory.FindIdForNewIngredient(newIngredient.data, GILData.IngredientType.Potion);
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

            potion.data.effectIds.AddRange(a.data.effectIds);
            foreach (var effect in b.data.effectIds)
            {
                if (!a.data.effectIds.Contains(effect))
                {
                    potion.data.effectIds.Add(effect);
                }
            }

            string newPotionId = GILFactory.FindIdForNewIngredient(potion.data, GILData.IngredientType.Potion);
            potion.data.id = "potion" + newPotionId;
            potion.data.ingName = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", "potion") + " " + newPotionId;
        }
        potion.count = 1;
        return potion;
    }

    public void OnResultButtonClick()
    {
        StorageContentData finalIngredient = result.GetIng();
        StorageContentData left = leftIngredient.GetIng();
        StorageContentData right = rightIngredient.GetIng();
        if (finalIngredient == null || finalIngredient.data.id == "steam") return;

        if (left.data.type == GILData.IngredientType.Catalyst || right.data.type == GILData.IngredientType.Catalyst)
        {
            CatalystEffectInterface effect = EffectRegistry.GetCatalystEffect(left.data.type == GILData.IngredientType.Catalyst ? left.data.effectIds[0] : right.data.effectIds[0]);
            if (left.data.type == GILData.IngredientType.Catalyst)
            {
                if (right.count < effect.GetNeededCount())
                {
                    ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания");
                    return;
                }
                IngredientFactory.RemoveIngredient(right.data.id, effect.GetNeededCount(), ScenesConfig.IsHome);
            }
            else
            {
                if (left.count < effect.GetNeededCount())
                {
                    ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания");
                    return;
                }
                IngredientFactory.RemoveIngredient(left.data.id, effect.GetNeededCount(), ScenesConfig.IsHome);
            }
        }
        else
        {
            if (left.count < 1 || right.count < 1)
            {
                ShakeButton.Instance.Shake(resultButton.transform, "Недостаточно ингредиентов для смешивания");
                return;
            }
            else
            {
                IngredientFactory.RemoveIngredient(left.data.id, 1, ScenesConfig.IsHome);
                IngredientFactory.RemoveIngredient(right.data.id, 1, ScenesConfig.IsHome);
            }
        }
        if (left.data.type == GILData.IngredientType.Potion && right.data.type == GILData.IngredientType.Potion) //||?
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
