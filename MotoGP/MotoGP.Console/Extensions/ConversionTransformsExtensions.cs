using Microsoft.ML;

namespace MotoGP.Extensions
{
    public static class ConversionTransformsExtensions
    {
        public static TransformsCatalog.ConversionTransforms MapValueToKey(
            this TransformsCatalog.ConversionTransforms transforms, string key)
        {
            ConversionsExtensionsCatalog.MapValueToKey(transforms, key);
            return transforms;
        }
    }
}