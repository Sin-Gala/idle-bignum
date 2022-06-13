/* Based on:
- Base javascript script: https://github.com/FredericRezeau/idle-bignum
- ToString & Normalization: https://tech.innogames.com/dealing-with-huge-numbers-in-idle-games/
- Notation format: https://cell-to-singularity.fandom.com/wiki/Notation
*/

using System.Collections.Generic;
public class BigNum
{
    #region CONSTRUCTOR
    public BigNum(double value,  uint exp)
    {
        this.value = value;
        this.exp = exp;

        Normalize();
    }
    #endregion

    public double value;
    public uint exp;

    // Normalize a number
    public void Normalize()
    {
        if (value <= 0)
        {
            value = 0;
            exp = 0;
            return;
        }

        while (value >= BigNumDatas.TEN_CUBED)
        {
            value /= BigNumDatas.TEN_CUBED;
            exp += 3;
        }

        while(value < 1 && exp != 0)
        {
            value *= BigNumDatas.TEN_CUBED;
            exp -= 3;
        }
    }

    // Compute the equivalent number at 1.Eexp (note: assumes exp is greater than this.exp)
    public void Align(uint otherExp)
    {
        uint diff = otherExp - exp;
        if (diff > 0)
        {
            value = (diff <= BigNumDatas.MAX_MAGNITUDE) ? value / Mathf.Pow(10, diff) : 0;
            exp = otherExp;
        }
    }

    // Add a number to this number
    public void Add(BigNum bigNumToAdd)
    {
        if (bigNumToAdd.exp < exp)
            bigNumToAdd.Align(exp);
        else
            Align(bigNumToAdd.exp);

        value += bigNumToAdd.value;
        Normalize();
    }

    // Substract a number from this number
    public void Substract(BigNum bigNumToSubstract)
    {
        if (bigNumToSubstract.exp < exp)
            bigNumToSubstract.Align(exp);
        else
            Align(bigNumToSubstract.exp);

        value -= bigNumToSubstract.value;
        Normalize();
    }

    // Multiply this number by factor
    public void Multiply(uint factor)
    {
        if (factor >= 0) // Does not support negative numbers
        {
            value *= factor;
            Normalize();
        }    
    }

    // Divide this number by divisor
    public void Divide(uint divisor)
    {
        if (divisor > 0)
        {
            value /= divisor;
            Normalize();
        }
    }

    private string GetUnit(uint magnitude)
    {
        return (BigNumDatas.POWER_TO_NAME.ContainsKey(magnitude)) ? BigNumDatas.POWER_TO_NAME[magnitude] : "???";
    }
    public override string ToString()
    {
        return value.ToString("0.##") + GetUnit(exp);
    }
}

public static class BigNumDatas
{
    public static Dictionary<uint, string> POWER_TO_NAME { get; private set; } = new Dictionary<uint, string>()
    {
        {0, ""},
        {3, "K" },
        {6, "M" },
        {9, "B" },
        {12, "T" },
        {15, "Qa" },
        {18, "Qi" },
        {21, "Sx" },
        {24, "Sp" },
        {27, "Oc" },
        {30, "No" },
        {33, "Dc" },
        {36, "Udc" },
        {39, "Ddc" },
        {42, "Tdc" },
        {45, "Qtdc" },
        {48, "Qdc" },
        {51, "Sdc" },
        {54, "Stdc" },
        {57, "Odc" },
        {60, "Ndc" },
        {63, "Vg" },
        {66, "Uvg" },
        {69, "Dvg" },
        {72, "Tvg" },
        {75, "Qtvg" },
        {78, "Qvg" },
        {81, "Svg" },
        {84, "Stvg" },
        {87, "Ovg" },
        {90, "Nvg" },
        {93, "Tg" },
        {96, "Utg" },
        {99, "Dtg" },
        {102, "Ttg" },
        {105, "Qttg" },
        {108, "Qtg" },
        {111, "Stg" },
        {114, "Sttg" },
        {117, "Otg" },
        {120, "Ntg" },
        {123, "Qdg" },
        {126, "Uqdg" },
        {129, "Dqdg" },
        {132, "Tqdg" },
        {135, "Qtqdg" },
        {138, "Qqdg" },
        {141, "Sqdg" },
        {144, "Stqdg" },
        {147, "Oqdg" },
        {150, "Nqdg" },
        {153, "Qg" },
        {156, "Uqg" },
        {159, "Dqg" },
        {162, "Tqg" },
        {165, "Qtqg" },
        {168, "Qqg" },
        {171, "Sqg" },
        {174, "Stqg" },
        {177, "Oqg" },
        {180, "Nqg" },
        {183, "Sg" },
        {186, "Usg" },
        {189, "Dsg" },
        {192, "Tsg" },
        {195, "Qtsg" },
        {198, "Qsg" },
        {201, "Ssg" },
        {204, "Stsg" },
        {207, "Osg" },
        {210, "Nsg" },
        {213, "Stg" },
        {216, "Ustg" },
        {219, "Dstg" },
        {222, "Tstg" },
        {225, "Qtstg" },
        {228, "Qstg" },
        {231, "Sstg" },
        {234, "Ststg" },
        {237, "Ostg" },
        {240, "Nstg" },
        {243, "Og" },
        {246, "Uog" },
        {249, "Dog" },
        {252, "Tog" },
        {255, "Qtotg" },
        {258, "Qog" },
        {261, "Sog" },
        {264, "Stog" },
        {267, "Oog" },
        {270, "Nog" },
        {273, "Ng" },
        {276, "Ung" },
        {279, "Dng" },
        {282, "Tng" },
        {285, "Qtntg" },
        {288, "Qng" },
        {291, "Sng" },
        {294, "Stng" },
        {297, "Ong" },
        {300, "Nng" },
        {303, "C" },
        {306, "Uc" },
        {309, "Dc" },
        {312, "Tc" },
        {315, "Qtc" },
        {318, "Qc" },
        {321, "Sc" },
        {324, "Stc" },
        {327, "Oc" },
        {330, "Nc" },
        {333, "Dcc" }
    };

    public static uint MAX_MAGNITUDE { get; private set; } = 12; // Max power magnitude diff for operands
    public static double TEN_CUBED { get; private set; } = 1000; // Used for normalizing numbers

}
