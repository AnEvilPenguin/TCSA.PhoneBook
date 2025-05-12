namespace PhoneBook;

public class ProductController
{
    internal static void AddProduct()
    {
        var name = "test1";
        
        using var db = new ProductContext();
        db.Add(new Product { Name = name });
        db.SaveChanges();
    }
}