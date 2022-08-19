using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class ScssGenerator
    {
        private const int Columns = 32;

        private string RootClass = """
            .pkicon {
            	@include crisp-rendering();
            	display: inline-block;
            	background-image: url("/assets/other/post/living-dex/pokesprite.png");
            	background-repeat: no-repeat;
            }
            """;
    }
}
