namespace RefactoredBomb
{
    public interface IDamageable
    {
        float Health { get; }
        void TakeDamage(float damage);
    }

}