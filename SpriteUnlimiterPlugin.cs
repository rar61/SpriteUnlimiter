using System.IO;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Plugins;
using Torch.Views;

namespace SpriteUnlimiter
{
    public class SpriteUnlimiterPlugin : TorchPluginBase, IWpfPlugin
    {
        private Persistent<Config> config;

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            config = Persistent<Config>.Load(Path.Combine(StoragePath, $"{Name}.cfg"));
        }

        public UserControl GetControl()
        {
            return new PropertyGrid() { DataContext = config.Data };
        }
    }
}