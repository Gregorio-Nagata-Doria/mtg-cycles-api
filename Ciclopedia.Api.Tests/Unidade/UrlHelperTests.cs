using System;
using System.Collections.Generic;
using System.Text;
using Ciclopedia.Api.Helpers;

namespace Ciclopedia.Api.Tests.Unidade
{
    public class UrlHelperTests
    {

        [Theory]
        [InlineData("1c9675fb-1a89-420f-aea8-50e0642f549c", "https://cards.scryfall.io/small/front/1/c/1c9675fb-1a89-420f-aea8-50e0642f549c.jpg")]
        [InlineData("36fa9a0b-b0c9-43ea-ba11-99d7982f974e", "https://cards.scryfall.io/small/front/3/6/36fa9a0b-b0c9-43ea-ba11-99d7982f974e.jpg")]
        [InlineData("8e37fae5-ddd0-4e16-8581-71579f89d9c5", "https://cards.scryfall.io/small/front/8/e/8e37fae5-ddd0-4e16-8581-71579f89d9c5.jpg")]
        [InlineData("f39aa2e9-e294-4ce6-bf5e-e1f579101a7a", "https://cards.scryfall.io/small/front/f/3/f39aa2e9-e294-4ce6-bf5e-e1f579101a7a.jpg")]
        public void Miniaturize(string id, string esperado)
        {
            Assert.Equal(esperado, UrlHelper.Miniatura(id));
        }


    }
}
