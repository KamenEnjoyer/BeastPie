using NUnit.Framework;

public abstract class PotionEffect
{
    protected int density;
    protected int price;

    public void Setup(int density, int price)
    {
        this.density = density;
        this.price = price;
    }

    public abstract void ApplyPotion();
    public abstract void ApplyDish();
    public abstract EffectData GetEffectData();
}