using System;
using System.Collections.Generic;
using System.Text;
using Ciclopedia.Api.Helpers;

namespace Ciclopedia.Api.Tests.Unidade
{
    public class SlugHelperTests
    {

        [Theory]
        [InlineData("1mv-tutor", "cycle-1mv-tutor")]
        [InlineData("aer-automaton", "cycle-aer-automaton")]
        public void ParaPublico(string slug, string esperado)
        {
            Assert.Equal(esperado, SlugHelper.Publico(slug));
        }

        // TODO(você): previsão do Interno("cycle-") — uma linha [InlineData] que entra
        // AQUI (com o esperado que você prevê) OU no ParaInternoIsNull abaixo. Onde ela
        // entra é a sua previsão. Leia SlugHelper.cs:13-17 antes de decidir.
        [Theory]
        [InlineData("cycle-1mv-tutor", "1mv-tutor")]
        [InlineData("cycle-sword-ally-color", "sword-ally-color")]
        public void ParaInterno(string slug ,string esperado)
        {
            Assert.Equal(esperado, SlugHelper.Interno(slug));
        }

        [Theory]
        [InlineData("1mv-tutor")]
        [InlineData("CYCLE-1mv-tutor")]
        [InlineData("cycle1mv-tutor")]
        public void ParaInternoIsNull(string slug )
        {
            Assert.Null(SlugHelper.Interno(slug));
        }
    }
}
