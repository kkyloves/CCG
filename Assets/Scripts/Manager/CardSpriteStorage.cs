using Script.Misc;
using Scriptable;
using UnityEngine;

[System.Serializable]
public class EnergyRelatedSprites
{
    public EnergyType EnergyType;
    public Sprite EnergySprite;

    public EnergyRelatedSprites(EnergyType energyType, Sprite energySprite)
    {
        EnergyType = energyType;
        EnergySprite = energySprite;
    }
}

public class CardSpriteStorage : Singleton<CardSpriteStorage>
{
    [SerializeField] private EnergyRelatedSprites[] energyIcons;
    [SerializeField] private EnergyRelatedSprites[] energyNameBanners;
    [SerializeField] private EnergyRelatedSprites[] borderBanners;
    [SerializeField] private EnergyRelatedSprites[] energyLogos;
    [SerializeField] private Sprite[] energyContainers;
    [SerializeField] private Sprite backCardSprite;

    public Sprite BackCardSprite => backCardSprite;
    
    public Sprite GetEnergyBanner(int energyCount)
    {
        var adjustedCount = energyCount - 1;
        return energyContainers[adjustedCount];
    }
    
    public Sprite GetEnergyIconSprite(EnergyType type)
    {
        foreach (var energy in energyIcons)
        {
            if (energy.EnergyType.Equals(type))
            {
                return energy.EnergySprite;
            }
        }

        return null;
    }
    
    public Sprite GetNameBannerSprite(EnergyType type)
    {
        foreach (var energy in energyNameBanners)
        {
            if (energy.EnergyType.Equals(type))
            {
                return energy.EnergySprite;
            }
        }

        return null;
    }
    
    public Sprite GetBorderBannerSprite(EnergyType type)
    {
        foreach (var energy in borderBanners)
        {
            if (energy.EnergyType.Equals(type))
            {
                return energy.EnergySprite;
            }
        }

        return null;
    }
    
    public Sprite GetEnergyLogoSprite(EnergyType type)
    {
        foreach (var energy in energyLogos)
        {
            if (energy.EnergyType.Equals(type))
            {
                return energy.EnergySprite;
            }
        }

        return null;
    }
    
}