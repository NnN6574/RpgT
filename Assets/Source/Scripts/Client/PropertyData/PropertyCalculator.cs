using Mistave.Client.Character.Data;
using Zenject;

namespace Mistave.Client.Data.Property
{
    public class PropertyCalculator
    {
        [Inject] private CharacterAttributes _characterAttributes;
        [Inject] private CharacterProperties _characterProperties;

        public void UpdateCharacterProperties()
        {
            UpdateStrenghtProperties();
            UpdateDexterityProperties();
            UpdateIntelligenceProperties();
        }

        private void UpdateStrenghtProperties()
        {


        }
        private void UpdateDexterityProperties()
        {
          
        }
        private void UpdateIntelligenceProperties()
        {
        
        }
    }
}