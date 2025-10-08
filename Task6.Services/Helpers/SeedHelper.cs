using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task6.Services.Helpers;

public static class SeedHelper
{
    private const int MadConst = 1392;

    public static int GetSeed(int globalSeed, int mad)
    {
        return mad * MadConst + globalSeed;
    }
}
