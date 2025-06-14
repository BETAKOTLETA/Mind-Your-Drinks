namespace Mind_Your_Drink_Server.DTO_s
{
    public class UserDrinkRequest
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public Models.UserDrink UserDrink { get; set; }
    }
}
