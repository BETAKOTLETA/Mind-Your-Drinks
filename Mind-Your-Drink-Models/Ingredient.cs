namespace Mind_Your_Drink_Models.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public UserDrink Drink { get; set; }
        public double Amount { get; set; }

        public double Callories => Drink?.Callories ?? 0; //Shortcut property to avoid use Ingredient.IUserDrink.Callories
        public int VolumeInMl => Drink?.VolumeInMl ?? 0;
        public double Price => Drink?.Price ?? 0;
    }
}
