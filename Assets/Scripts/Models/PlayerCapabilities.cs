using System.Collections.Generic;

namespace Models
{
    public class LifeContainer
    {
        public float capacity;
        public float current;

        public LifeContainer(float capacity, float current)
        {
            this.capacity = capacity;
            this.current = current;
        }
    }
    
    public class PlayerCapabilities
    {
        public float damage = 3.0f;
        public float tickPause = .333f;
        public float reloadPause = 1f;
        public float range = 10.0f;
        public float shotSpeed = 300f;
        public float shotTTL = 2.0f;
        public List<LifeContainer> life = new List<LifeContainer>();

        public PlayerCapabilities()
        {
            life.Add(new LifeContainer(2f, 2f));
            life.Add(new LifeContainer(3f, 3f));
        }
    }
}