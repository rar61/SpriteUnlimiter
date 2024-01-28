using Torch;
using Torch.Views;

namespace SpriteUnlimiter
{
    public class Config : ViewModel
    {
        private float syncDistance = 32;
        [Display(Name = "Sync Distance")]
        public float SyncDistance
        { 
            get => syncDistance;
            set 
            {
                SetValue(ref syncDistance, value);
                SpriteUnlimiterPatch.DistanceSetting = value;
            }
        }
    }
}
