using System;
using System.Collections.Generic;
using System.Globalization;

// Big number.
public class BigNumber
{
    public double Value { get; set; }
    public int Exponent { get; set; }

    private const int MaxMagnitude = 12; // Max power magnitude diff for operands.
    //TODO: Check if the constant TenCube works with this engineering notation.
    private const double TenCubed = 1e3; // Used for normalizing numbers.
    
    // Constructor
    public BigNumber(double value, int exponent = 0)
    {
        this.Value = value;
        this.Exponent = exponent;
        
        //TODO: calling Normalize() may work like the constructor below that accepts only a double. Test its efficacy. 
        // Normalize();
    }
    
    public BigNumber(double value)
    {
        if (value == 0)
        {
            this.Value = 0;
            this.Exponent = 0;
            return;
        }

        this.Exponent = (int)Math.Floor(Math.Log10(Math.Abs(value)));
        this.Value = value / Math.Pow(10, this.Exponent);
    }

    // Normalize a number (Engineering notation).
    public void Normalize()
    {
        if (this.Value < 1 && this.Exponent != 0)
        {
            // e.g., 0.1E6 is converted to 100E3 ([0.1, 6] = [100, 3])
            this.Value *= Math.Pow(10, 3);
            this.Exponent -= 3;
        }
        else if (this.Value >= Math.Pow(10, 3))
        {
            // e.g., 10000E3 is converted to 10E6 ([10000, 3] = [10, 6])
            while (this.Value >= Math.Pow(10, 3))
            {
                this.Value *= 1 / Math.Pow(10, 3);
                this.Exponent += 3;
            }
        }
        else if (this.Value <= 0)
        {
            // Negative flag is set, but negative number operations are not supported.
            // You can add a negative flag property if needed.
            this.Exponent = 0;
            this.Value = 0;
        }
    }

    // Compute the equivalent number at 1Exp (note: assumes exp is greater than this.Exp).
    public void Align(int exponent)
    {
        int d = exponent - this.Exponent;
        if (d > 0)
        {
            this.Value = ((d <= MaxMagnitude) ? this.Value / Math.Pow(10, d) : 0);
            this.Exponent = exponent;
        }
    }

    // Add a number to this number.
    public void Add(BigNumber bigNumber)
    {
        if (bigNumber.Exponent < this.Exponent)
        {
            bigNumber.Align(this.Exponent);
        }
        else
        {
            this.Align(bigNumber.Exponent);
        }
        this.Value += bigNumber.Value;
        this.Normalize();
    }

    // Subtract a number from this number.
    public void Subtract(BigNumber bigNumber)
    {
        if (bigNumber.Exponent < this.Exponent)
        {
            bigNumber.Align(this.Exponent);
        }
        else
        {
            this.Align(bigNumber.Exponent);
        }
        this.Value -= bigNumber.Value;
        this.Normalize();
    }

    // Multiply this number by a factor.
    public void Multiply(double factor)
    {
        // We do not support negative numbers.
        if (factor >= 0)
        {
            this.Value *= factor;
            this.Normalize();
        }
    }

    // Divide this number by a divisor.
    public void Divide(double divisor)
    {
        if (divisor > 0)
        {
            this.Value /= divisor;
            this.Normalize();
        }
    }
    
    public ComparisonResult CompareTo(BigNumber other)
    {
        if (this.Exponent < other.Exponent)
        {
            this.Align(other.Exponent);
        }
        else if (this.Exponent > other.Exponent)
        {
            other.Align(this.Exponent);
        }

        if (this.Value > other.Value)
        {
            return ComparisonResult.Greater;
        }
        else if (this.Value < other.Value)
        {
            return ComparisonResult.Less;
        }
        else
        {
            return ComparisonResult.Equal;
        }
    }

    /// <summary>
    /// Return the number as string.
    /// </summary>
    /// <param name="precision">Number of decimals shown in the number.</param>
    /// <returns>The value of the number as a string.</returns>
    public string GetValue(int precision = 3)
    {
        return this.Value.ToString($"F{precision}");
    }

    // GetExpName. Return the exponent name as a string.
    public string GetExponentName()
    {
        if (PowTenToName.Names.TryGetValue(this.Exponent, out string name))
        {
            return name;
        }
        return string.Empty;
    }

    // GetExp. Return the exponent as a string.
    public string GetExponent()
    {
        return this.Exponent.ToString();
    }

    // ToString.
    public override string ToString()
    {
        string exponentName = GetExponentName();

        return $"{this.Value} {exponentName}";
    }
}

public enum ComparisonResult
{
    Greater,
    Less,
    Equal
}

public static class PowTenToName
{
    public static Dictionary<int, string> Names = new Dictionary<int, string>()
    {
        { 0, "" },
        { 3, "thousand" },
        { 6, "million" },
        { 9, "billion" },
        { 12, "trillion" },
        { 15, "quadrillion" },
        { 18, "quintillion" },
        { 21, "sextillion" },
        { 24, "septillion" },
        { 27, "octillion" },
        { 30, "nonillion" },
        { 33, "decillion" },
        { 36, "undecillion" },
        { 39, "duodecillion" },
        { 42, "tredecillion" },
        { 45, "quattuordecillion" },
        { 48, "quindecillion" },
        { 51, "sedecillion" },
        { 54, "septendecillion" },
        { 57, "octodecillion" },
        { 60, "novendecillion" },
        { 63, "vigintillion" },
        { 66, "unvigintillion" },
        { 69, "duovigintillion" },
        { 72, "tresvigintillion" },
        { 75, "quattuorvigintillion" },
        { 78, "quinvigintillion" },
        { 81, "sesvigintillion" },
        { 84, "septemvigintillion" },
        { 87, "octovigintillion" },
        { 90, "novemvigintillion" },
        { 93, "trigintillion" },
        { 96, "untrigintillion" },
        { 99, "duotrigintillion" },
        { 102, "trestrigintillion" },
        { 105, "quattuortrigintillion" },
        { 108, "quintrigintillion" },
        { 111, "sestrigintillion" },
        { 114, "septentrigintillion" },
        { 117, "octotrigintillion" },
        { 120, "noventrigintillion" },
        { 123, "quadragintillion" },
        { 126, "unquadragintillion" },
        { 129, "duoquadragintillion" },
        { 132, "tresquadragintillion" },
        { 135, "quattuorquadragintillion" },
        { 138, "quinquadragintillion" },
        { 141, "sesquadragintillion" },
        { 144, "septenquadragintillion" },
        { 147, "octoquadragintillion" },
        { 150, "novenquadragintillion" },
        { 153, "quinquagintillion" },
        { 156, "unquinquagintillion" },
        { 159, "duoquinquagintillion" },
        { 162, "tresquinquagintillion" },
        { 165, "quattuorquinquagintillion" },
        { 168, "quinquinquagintillion" },
        { 171, "sesquinquagintillion" },
        { 174, "septenquinquagintillion" },
        { 177, "octoquinquagintillion" },
        { 180, "novenquinquagintillion" },
        { 183, "sexagintillion" },
        { 186, "unsexagintillion" },
        { 189, "duosexagintillion" },
        { 192, "tresexagintillion" },
        { 195, "quattuorsexagintillion" },
        { 198, "quinsexagintillion" },
        { 201, "sesexagintillion" },
        { 204, "septensexagintillion" },
        { 207, "octosexagintillion" },
        { 210, "novensexagintillion" },
        { 213, "septuagintillion" },
        { 216, "unseptuagintillion" },
        { 219, "duoseptuagintillion" },
        { 222, "treseptuagintillion" },
        { 225, "quattuorseptuagintillion" },
        { 228, "quinseptuagintillion" },
        { 231, "seseptuagintillion" },
        { 234, "septenseptuagintillion" },
        { 237, "octoseptuagintillion" },
        { 240, "novenseptuagintillion" },
        { 243, "octogintillion" },
        { 246, "unoctogintillion" },
        { 249, "duooctogintillion" },
        { 252, "tresoctogintillion" },
        { 255, "quattuoroctogintillion" },
        { 258, "quinoctogintillion" },
        { 261, "sexoctogintillion" },
        { 264, "septemoctogintillion" },
        { 267, "octooctogintillion" },
        { 270, "novemoctogintillion" },
        { 273, "nonagintillion" },
        { 276, "unnonagintillion" },
        { 279, "duononagintillion" },
        { 282, "trenonagintillion" },
        { 285, "quattuornonagintillion" },
        { 288, "quinnonagintillion" },
        { 291, "senonagintillion" },
        { 294, "septenonagintillion" },
        { 297, "octononagintillion" },
        { 300, "novenonagintillion" },
        { 303, "centillion" },
        { 306, "uncentillion" },
        { 309, "duocentillion" },
        { 312, "trescentillion" },
        { 315, "quattuorcentillion" },
        { 318, "quincentillion" },
        { 321, "sexcentillion" },
        { 324, "septencentillion" },
        { 327, "octocentillion" },
        { 330, "novencentillion" },
        { 333, "decicentillion" }
    };
}