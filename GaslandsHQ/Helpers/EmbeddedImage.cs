using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GaslandsHQ.Helpers
{
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId
        {
            get;
            set;
        }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrWhiteSpace(ResourceId)) return null;
            return ImageSource.FromResource(ResourceId);
        }
    }
}
