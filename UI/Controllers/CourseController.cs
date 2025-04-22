using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class CourseController : Controller
{
    public IActionResult Create()
    {
        var model = new CourseCreateViewModel();
        return View(model);
    }
}

public class CourseCreateViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public long CategoryId { get; set; }
    public decimal  Price { get; set; }
    public long AuthorId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CoverImage { get; set; }
}