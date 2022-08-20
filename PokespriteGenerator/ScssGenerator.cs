using PokespriteGenerator.Models;
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
            	background-image: url("pokesprite.png");
            	background-repeat: no-repeat;
            }
            """;

        public string GenerateScss(List<PokemonData> pokemonDataList)
        {
            var scssClassList = new List<string> { RootClass };

            var maxWidth = pokemonDataList.Max(x => x.TrimmedWidth).GetValueOrDefault();
            var maxHeight = pokemonDataList.Max(x => x.TrimmedHeight).GetValueOrDefault();

            foreach (var item in pokemonDataList)
            {
                var number = pokemonDataList.IndexOf(item);
                var column = (number) % Columns;
                var row = number / Columns;

                var cssClass = $$""".pkicon.pkicon-{{item.Number}}{{FormData(item.Form)}} { width: {{item.TrimmedWidth}}px; height: {{item.TrimmedHeight}}px; background-position: -{{column * maxWidth}}px -{{row * maxHeight}}px }""";
                scssClassList.Add(cssClass);
            }

            var allClasses = string.Join(Environment.NewLine, scssClassList);

            return allClasses;

        }

        private string FormData(string form)
        {
            if (form == "")
            {
                return "";
            }

            return $".form-{form}";
        }
    }
}
