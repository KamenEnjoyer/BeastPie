using Unity.VisualScripting;
using UnityEngine;

public class MenuBook : MonoBehaviour
{
    public static MenuBook Instance;
    public GameObject alchemyBookPref;
    public GameObject beastBookPref;
    public GameObject magicBookPref;
    public Transform parent;

    private GameObject currentBook;

    void Awake()
    {
        Instance = this;
    }

    public void ShowAlchemyBook()
    {
        ShowBook(alchemyBookPref);
    }

    public void ShowBeastBook()
    {
        ShowBook(beastBookPref);
    }

    public void ShowMagicBook()
    {
        ShowBook(magicBookPref);
    }

    public void ShowBook(GameObject pref)
    {
        if (currentBook != null && currentBook.name.Contains(pref.name)) HideBook(); //refactor
        else
        {
            HideBook();
            currentBook = Instantiate(pref, parent, false);
        }
    }

    public void HideBook()
    {
        if (currentBook != null)
        {
            StorageContent.Instance.UpdateIngredients();
            Destroy(currentBook);
        }
    }
}
