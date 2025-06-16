//namespace Mind_Your_Drink_Models.Models
//{
//    public class Coctaile
//    {
//        public int Id { get; set; }

//        public string Name { get; set; }

//        public string? Description { get; set; }

//        public double Callories { get; set; }

//        public double Price { get; set; }

//        public List<Ingredient> Ingredients { get; set; } = [];

//        public Coctaile(string name)
//        {
//            Name = name;
//        }

//        public void AddIngredients(Ingredient ingredient)
//        {
//            Ingredients.Add(ingredient);
//            Update();
//        }

//        //Calculate callories and price of a product
//        public void Update()
//        {
//            Callories = 0;
//            Price = 0;
//            foreach (var ingridient in Ingredients)
//            {
//                Callories += ingridient.Callories * ingridient.Amount;
//                Price += ingridient.VolumeInMl * ingridient.Price; 
//            }
//        }
//    }
//}
