using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task6.Services.Helpers;

public static class SeedHelper
{
    private const int MadConst = 1392;

    public static int GetSeed(long globalSeed, int mad)
    {
        return (int)((mad * MadConst + globalSeed) % int.MaxValue);
    }
}
