namespace Ciclopedia.Api.Helpers
{
    public static class UrlHelper
    {

        public static string Miniatura(string id) => $"https://cards.scryfall.io/small/front/{id[0]}/{id[1]}/{id}.jpg";

    }
}
