using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using PhoneBook.Model;
using Spectre.Console;

namespace PhoneBook.View;

enum CategoryOption
{
    [Display(Name = "New Category")]
    New,
    [Display(Name = "Rename Category")]
    Update,
    [Display(Name = "Delete Category")]
    Delete,
    [Display(Name = "Back to Main Menu")]
    Back
}

public class CategoryMenu () : AbstractMenu
{
    private readonly CategoryController _categoryController = new ();
    
    public void Run()
    {
        CategoryOption? choice = null;
        
        while (choice != CategoryOption.Back)
        {
            
            choice = Prompt<CategoryOption>();

            Category? category = null;
            
            switch (choice)
            {
                case CategoryOption.New:
                    _categoryController.Add();
                    break;
                
                case CategoryOption.Update:
                    category = GetCategory();
                    
                    if (category != null)
                        _categoryController.Update(category);
                    
                    AnsiConsole.Clear();
                    break;
                
                case CategoryOption.Delete:
                    category = GetCategory();
                    
                    if (category != null)
                        _categoryController.Delete(category);
                    
                    AnsiConsole.Clear();
                    return;
            }
        }
        
        AnsiConsole.Clear();
    }

    private Category? GetCategory()
    {
        var categories = _categoryController.List();

        if (!categories.Any())
        {
            AnsiConsole.MarkupLine("No categories found");
            return null;
        }
        
        return AnsiConsole.Prompt(new SelectionPrompt<Category>()
            .Title("What category?")
            .AddChoices(categories)
            .UseConverter(c => c.Name));
    }
}