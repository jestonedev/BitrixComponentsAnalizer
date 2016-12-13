using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace UnitTests.BitrixInfrastructure
{
    internal static class BitrixHelper
    {
        public static List<BitrixComponent>  GetBitrixComponentsSample()
        {
            return new List<BitrixComponent>
            {
                new BitrixComponent
                {
                    Name = "list",
                    Category = "bitrix:news",
                    Files = new List<BitrixFile>
                    {
                        new BitrixFile
                        {
                            FileName = "1.php"
                        }
                    }.AsReadOnly()
                },
                new BitrixComponent
                {
                    Name = "any",
                    Category = "bitrix:news.list",
                    Files = new List<BitrixFile>
                    {
                        new BitrixFile
                        {
                            FileName = "2.php"
                        }
                    }.AsReadOnly()
                },
                new BitrixComponent
                {
                    Name = "any2",
                    Category = "bitrix:news.list2",
                    Files = null
                }
            };
        }
    }
}
