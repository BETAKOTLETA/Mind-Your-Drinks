namespace Mind_Your_Drink_Models.Models
{
    public interface IDrink
    {
        int Id { get; set; }

        string Name { get; set; }

        string? Description { get; set; }

        double Calories { get; set; }
    }

    public class Drink : IDrink
    {
        public int Id { get; set; }

        public string Icon { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public DrinkType Type { get; set; }

        public float Abv { get; set; }

        public double Calories { get; set; }
    }

    public interface IUserDrink
    {
        double Price { get; set; }
        int VolumeInMl { get; set; }
    }


    public class UserDrink : Drink, IUserDrink {

        public int Id { get; set; }
        public int UserId { get; set; }
        public double Price { get; set; }
        public int VolumeInMl { get; set; }
        public DateTime Time { get; set; }
    }

    //Mb later I will create a State for types of alchol
    public enum DrinkType
    {
        Beer,
        Wine,
        Cider,
        Vodka,
        Tequila,
        Whiskey,
        Rum,
        Brandy,
        Gin,
        Liqueur,
        MixedDrink,
        Other
    } 

}
