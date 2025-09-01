using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Persistence.Model;
using StudentManagement.Persistence.Repository;

public class HomeController : Controller
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMenuTypeRepository _menuTypeRepository;
    private readonly IIngredientComboRepository _ingredientComboRepository;

    public HomeController(IMenuRepository menuRepository, IMenuTypeRepository menutypeRepository, IIngredientComboRepository ingredientComboRepository)
    {
        _menuRepository = menuRepository;
        _menuTypeRepository = menutypeRepository;
        _ingredientComboRepository = ingredientComboRepository;
    }

    // 목록
    public async Task<IActionResult> Index()
    {
        var menus = await _menuRepository.GetAllAsync();
        return View(menus);
    }

    [HttpGet]
    public async Task<IActionResult> GetMenusJson()
    {
        var menus = await _menuRepository.GetAllAsync();
        var menuTypes = await _menuTypeRepository.GetAllAsync();
        int totalMenuTypeCount = menuTypes.Count;

        // MenuId별로 그룹화
        var groupedMenus = menus
            .GroupBy(m => m.MenuId)
            .Select(g => new MenuDto
            {
                MenuId = g.Key,
                MenuName = g.First().MenuName?.MenuNameValue ?? "",
                Percentage = ((totalMenuTypeCount==0)?0:Math.Round((double)g.Count() / totalMenuTypeCount * 100, 2)).ToString() + "% (" + g.Count() + "/" + totalMenuTypeCount + ")"
            })
            .ToList();

        return Json(new { data = groupedMenus });
        //return Json(new { data = groupedMenus }, JsonRequestBehavior.AllowGet); << .NET4.0에서는 이렇게 AllowGet을 넣어줘야 할 수도 있습니다!
    }


    [HttpPost]
    public async Task<IActionResult> Create(int MenuId, int MenuTypeId, int IngredientComboId)
    {
        try
        {
            var menu = new Menu
            {
                MenuId = MenuId,
                MenuTypeId = MenuTypeId,
                IngredientComboId = IngredientComboId
            };
            await _menuRepository.AddAsync(menu);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetIngredientCombosJson()
    {
        // IngredientComboRepository에서 전체 목록 가져오기
        var combos = await _ingredientComboRepository.GetAllAsync();
        var result = combos.Select(c => new {
            ingredientComboId = c.IngredientComboId,
            ingredient1 = c.Ingredient1,
            ingredient2 = c.Ingredient2,
            ingredient3 = c.Ingredient3
        }).ToList();
        return Json(new { data = result });
    }
}
