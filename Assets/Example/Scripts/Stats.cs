using UAS;

namespace Example
{
    public class Health : Stat
    {
        public Health(float value) : base(value)
        {
        }

        public override string GetName()
        {
            return nameof(Health);
        }
    }

    public class MaxHealth : Stat
    {
        public MaxHealth(float value) : base(value)
        {
        }
        public override string GetName()
        {
            return nameof(MaxHealth);
        }
    }
    
    public class MovementSpeed : Stat
    {
        public MovementSpeed(float value) : base(value)
        {
        }
        public override string GetName()
        {
            return nameof(MovementSpeed);
        }
    }

    public class Agility : Stat
    {
        public Agility(float value) : base(value)
        {
        }
        public override string GetName()
        {
            return nameof(Agility);
        }
    }
}