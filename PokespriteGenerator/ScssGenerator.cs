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
            .pokesprite {
            	display: inline-block;
            	background-image: url("pokesprite.png");
            	background-repeat: no-repeat;
            }
            """;

        public string GenerateScss(List<BaseSpriteData> spriteData)
        {
            var scssClassList = new List<string> { RootClass };

            var maxWidth = spriteData.Max(x => x.TrimmedWidth).GetValueOrDefault();
            var maxHeight = spriteData.Max(x => x.TrimmedHeight).GetValueOrDefault();

            foreach (var item in spriteData)
            {
                var number = spriteData.IndexOf(item);
                var column = number % Columns;
                var row = number / Columns;

                var cssClass = $$""".pokesprite.{{item.ClassName}} { width: {{item.TrimmedWidth}}px; height: {{item.TrimmedHeight}}px; background-position: -{{column * maxWidth}}px -{{row * maxHeight}}px }""";
                scssClassList.Add(cssClass);
            }

            var allClasses = string.Join(Environment.NewLine, scssClassList);

            return allClasses;
        }        
    }
}
