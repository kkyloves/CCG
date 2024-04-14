using TMPro;

public class StatsCounter
{
    private int attackCount;
    private int defCount;

    private int tempAttackCount;
    private int tempDefCount;
    
    public int AttackCount => tempAttackCount;
    public int DefCount => tempDefCount;


    private readonly TextMeshProUGUI attackText;
    private readonly TextMeshProUGUI defText;

    public StatsCounter(TextMeshProUGUI attackText, TextMeshProUGUI defText)
    {
        this.attackText = attackText;
        this.defText = defText;
    }

    public void SetStats(int attackCount, int defCount)
    {
        this.attackCount = attackCount;
        this.defCount = defCount;
        
        tempAttackCount = attackCount;
        tempDefCount = defCount;
    }

    public void Reset()
    {
        tempAttackCount = attackCount;
        tempDefCount = defCount;
        
        attackText.text = tempAttackCount.ToString();
        defText.text = tempDefCount.ToString();
    }
    
    public bool UpdateAttack(int deduction)
    {
        if (tempAttackCount > 0)
        {
            tempAttackCount -= deduction;
            if (tempAttackCount <= 0)
            {
                tempAttackCount = 0;
            }
            
            attackText.text = tempAttackCount.ToString();
        }
        else
        {
            tempAttackCount = 0;
        }

        return tempAttackCount <= 0;
        
    }

    public bool UpdateDef(int deduction)
    {
        if (tempDefCount > 0)
        {
            tempDefCount -= deduction;
            if (tempDefCount <= 0)
            {
                tempDefCount = 0;
            }
            
            defText.text = tempDefCount.ToString();
        }
        else
        {
            tempDefCount = 0;
        }

        return tempDefCount <= 0;
    }
}
