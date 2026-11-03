namespace Ciclopedia.Api.Helpers
{
    public static class SlugHelper
    {

        private const string Prefixo = "cycle-";
        public static string Publico(string slug) => $"{Prefixo}{slug}";


        public static string? Interno(string slug) {


            if(slug.StartsWith(Prefixo))
            {
                return slug[Prefixo.Length..];
            }
            return null;
        }

    }
}
