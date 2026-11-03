using System;
using System.ComponentModel.DataAnnotations;
using Ciclopedia.Api.Dtos;
using Ciclopedia.Api.Helpers;


namespace Ciclopedia.Api.Tests.Unidade
{
    public class CiclosQueryTests
    {

        static readonly CiclosQuery SemFiltro = new(
    Set: null!,
    Rarity: null!,
    Structure: null!,
    Color: null!,
    Year: null!,
    Q: null,
    Sort: null);


        static readonly CiclosQuery Valida = new(
    Set: new[] { "dmr" },
    Rarity: new[] { "rare" },
    Structure: new[] { "5-mono" },
    Color: new[] { "w" },
    Year: new[] { 2023 },
    Q: "tutor",
    Sort: "name-asc",
    Page: 1,
    PerPage: 24);


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]

        public void PaginaInvalida(int esperado)
        {
            var invalida = Valida with { Page = esperado };
            var erros = invalida.Validate(new ValidationContext(invalida)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Page), erroSingular.MemberNames);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(101)]
        [InlineData(500)]
        public void PorPaginaInvalida(int perPage)
        {
            var invalida = Valida with { PerPage = perPage };
            var erros = invalida.Validate(new ValidationContext(invalida)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.PerPage), erroSingular.MemberNames);
        }

        [Theory]
        [InlineData("lixo")]
        public void OrdenacaoInvalida(string sort)
        {
            var invalida = Valida with { Sort = sort };
            var erros = invalida.Validate(new ValidationContext(invalida)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Sort), erroSingular.MemberNames);
        }
        [Theory]
        [InlineData(100)]
        [InlineData(1)]

        public void PorPaginaValida(int perPage)
        {
            var copia = Valida with { PerPage = perPage };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            Assert.Empty(erros);
        }

        [Theory]
        [InlineData("")]
        [InlineData("name-asc")]

        public void SortValido(string sort)
        {
            var copia = Valida with { Sort = sort };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            Assert.Empty(erros);
        }

        [Fact]
        public void ValidaSemErros()
        {
            var erros = Valida.Validate(new ValidationContext(Valida)).ToList();
            Assert.Empty(erros);
        }

        [Theory]
        [InlineData("u")]
        [InlineData("c")]
        public void CorValida(string cor)
        {
            var copia = Valida with { Color = new[] { cor } };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            Assert.Empty(erros);
        }

        [Theory]
        [InlineData("abc")]
        public void RaridadeInvalida(string raridade)
        {
            var invalida = Valida with { Rarity = new[] { raridade } };
            var erros = invalida.Validate(new ValidationContext(invalida)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Rarity), erroSingular.MemberNames);
        }

        [Theory]
        [InlineData("xyz")]
        public void EstruturaInvalida(string estrutura)
        {
            var invalida = Valida with { Structure = new[] { estrutura } };
            var erros = invalida.Validate(new ValidationContext(invalida)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Structure), erroSingular.MemberNames);
        }

        [Fact]
        public void DuasRaridadesValidas()
        {
            var copia = Valida with { Rarity = new[] { "rare", "mythic" } };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            Assert.Empty(erros);
        }


        [Fact]

        public void SemFiltro2()
        {
       
            var erros = SemFiltro.Validate(new ValidationContext(SemFiltro)).ToList();
            Assert.Empty(erros);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("W")]

        public void CorInvalida(string color)
        {
            var copia = Valida with { Color = new[] { color } };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Color), erroSingular.MemberNames);
        }




        [Fact]
        public void RaridadeComUmaInvalida()
        {
            var copia = Valida with { Rarity = new[] { "rare", "abc" } };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            var erroSingular = Assert.Single(erros);
            Assert.Contains(nameof(CiclosQuery.Rarity), erroSingular.MemberNames);
        }


        [Fact]
        public void TresCamposInvalidos()
        {
            var copia = Valida with { Rarity = new[] { "abc"}, Page = 0, PerPage = 500 };
            var erros = copia.Validate(new ValidationContext(copia)).ToList();
            Assert.Equal(3, erros.Count);
        }

    }
}
