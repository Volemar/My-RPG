using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour 
    {
        public void Save(string saveFile)
        {
            print("Saving to the " + saveFile);
        }

        public void Load(string saveFile)
        {
            print("Loading to the " + saveFile);
        }
    }
}
