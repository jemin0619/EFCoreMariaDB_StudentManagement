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
                Percentage = ((totalMenuTypeCount==0)?0:Math.Round((double)g.Count() / totalMenuTypeCount * 100, 0)).ToString() + "% (" + g.Count() + "/" + totalMenuTypeCount + ")"
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

    [HttpGet]
    public async Task<IActionResult> GetMenuTypesJson()
    {
        var types = await _menuTypeRepository.GetAllAsync();
        var result = types.Select(t => new {
            menuTypeId = t.MenuTypeId,
            menuTypeName = t.MenuTypeName
        }).ToList();
        return Json(new { data = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuDetailJson(int menuId)
    {
        var menuTypes = await _menuTypeRepository.GetAllAsync();
        var menus = await _menuRepository.GetAllAsync();
        var ingredientCombos = await _ingredientComboRepository.GetAllAsync();

        var result = menuTypes.Select<MenuType, object>(mt =>
        {
            var menu = menus.FirstOrDefault(m => m.MenuId == menuId && m.MenuTypeId == mt.MenuTypeId);
            if (menu != null)
            {
                var combo = ingredientCombos.FirstOrDefault(ic => ic.IngredientComboId == menu.IngredientComboId);
                string comboText = combo != null
                    ? $"({combo.Ingredient1}, {combo.Ingredient2}, {combo.Ingredient3})"
                    : "Not Defined";
                return new
                {
                    menuTypeId = mt.MenuTypeId,
                    menuTypeName = mt.MenuTypeName,
                    ingredientComboId = menu.IngredientComboId,
                    ingredientComboText = comboText
                };
            }
            else
            {
                return new
                {
                    menuTypeId = mt.MenuTypeId,
                    menuTypeName = mt.MenuTypeName,
                    ingredientComboId = (int?)null,
                    ingredientComboText = "Not Defined"
                };
            }
        }).ToList();

        return Json(new { data = result });
    }

    [HttpPost]
    public async Task<IActionResult> SaveMenuDetail([FromBody] MenuDetailSaveDto dto)
    {
        var menus = await _menuRepository.GetAllAsync();
        foreach (var detail in dto.details)
        {
            var menu = menus.FirstOrDefault(m => m.MenuId == dto.menuId && m.MenuTypeId == detail.menuTypeId);
            if (detail.ingredientComboId == null)
            {
                // ingredientComboId가 null이면 Row 삭제
                if (menu != null)
                    await _menuRepository.DeleteAsync(dto.menuId, detail.menuTypeId);
            }
            else if (menu != null)
            {
                // 있으면 수정
                menu.IngredientComboId = detail.ingredientComboId.Value;
                await _menuRepository.UpdateAsync(menu);
            }
            else
            {
                // 없으면 추가
                var newMenu = new Menu
                {
                    MenuId = dto.menuId,
                    MenuTypeId = detail.menuTypeId,
                    IngredientComboId = detail.ingredientComboId.Value
                };
                await _menuRepository.AddAsync(newMenu);
            }
        }
        return Json(new { success = true });
    }


    // DTO 예시
    public class MenuDetailSaveDto
    {
        public int menuId { get; set; }
        public List<MenuDetailRowDto> details { get; set; }
    }
    public class MenuDetailRowDto
    {
        public int menuTypeId { get; set; }
        public int? ingredientComboId { get; set; }
    }


}
