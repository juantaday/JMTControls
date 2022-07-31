namespace JMControls.Helpers
{
    using System.Drawing;
    public static class ExtensionMethods
    {
        public static Color FromHex(this string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
