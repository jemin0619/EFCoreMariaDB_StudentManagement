EF Core 사용하기
일단 전부 MIT 라이센스여서 편하게 써도 됨

Persistence
- DBContext
- Entity << 개인적인 생각으로 Update, Create 등의 쿼리를 위해 Domain Model을 Entity로 변환해주는 Mapper가 필요할 것으로 보임.
- Repository

패키지 관리자 콘솔에서 아래 명령어 입력 (Tools > Nuget Package 관리자)
```cpp
dotnet ef migrations add {Migration명} --project ./{솔루션명}
//Migration 폴더에 Initial이란 이름의 DB Migraion이 만들어짐
//Model에서 제한자를 public 등으로 풀어줘야 정상적으로 만들어짐 ㅎㅎ...
	
dotnet ef database update --project ./{솔루션명}
//DB가 만들어짐!!
```

필수 Package
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- MysqlConnector
- Pomelo.EntityFrameworkCore.MySql

### 250901) Forein Key를 사용하는데 왜 순환 참조 에러가 발생하나요??
- A. 저는 GetAll 후 Json 변환하는 부분에서 이 에러가 발생했습니다. 원인은 Json 변환시에 내부에 ICollection 때문입니다. ICollection이 부모 집합을 가리키고, 부모는 또 ICollection이 속한 자식을 가리키니 이런 에러가 발생합니다. (비전공자라 표현이 부적절할 수 있습니다...)
- 해결 방법 : DTO 사용하기

```csharp
namespace StudentManagement.DTOs
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Percentage { get; set; }
    }
}


//HomeController.cs
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
```

조합을 이걸로 하기... 그래야 DB 안깨질듯
utf8mb4_general_ci


Add 버튼이 조금 위로 올라오는 문제는 invisible Label 추가하여 해결할 것.
```
<div class="col-md-3">
    <label for="menuId" class="form-label">MenuId</label>
    <input type="text" id="menuId" name="menuId" class="form-control" required style="width : stretch" />
</div>
<div class="col-md-3">
    <label class="form-label invisible"> </label>
    <button type="submit" class="btn btn-primary w-100">Add</button>
</div>
```


```css
@{
    ViewData["Title"] = "MenuNames 목록";
}

<!-- Bootstrap & jQuery CDN (이미 있다면 생략) -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap5.min.css" />
<script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap5.min.js"></script>

<div class="container mt-5">
    <h2>MenuNames 목록</h2>
    <table id="menuNamesTable" class="table table-bordered table-striped">
        <thead>
            <tr>
                <th>MenuId</th>
                <th>MenuNameValue</th>
                <th>Action</th>
            </tr>
        </thead>
    </table>
</div>

<!-- 상세 모달 -->
<div class="modal fade" id="detailModal" tabindex="-1" aria-labelledby="detailModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header bg-info">
                <h5 class="modal-title" id="detailModalLabel">Menus 상세 목록</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="닫기"></button>
            </div>
            <div class="modal-body">
                <table id="menusTable" class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>MenuId</th>
                            <th>MenuTypeId</th>
                            <th>IngredientComboId</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="modal-footer">
                <button type="button" id="saveMenusBtn" class="btn btn-primary">Save</button>
            </div>
        </div>
    </div>
</div>

<!-- IngredientCombo 선택 모달 -->
<div class="modal fade" id="ingredientComboModal" tabindex="-1" aria-labelledby="ingredientComboModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header bg-warning">
                <h5 class="modal-title" id="ingredientComboModalLabel">IngredientCombo 선택</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="닫기"></button>
            </div>
            <div class="modal-body">
                <table id="ingredientComboTable" class="table table-hover">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Ingredient1</th>
                            <th>Ingredient2</th>
                            <th>Ingredient3</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
</div>

<script>
    let currentMenuId = null;
    let editingRowIndex = null;

    $(function () {
        // MenuNames 목록 Datatable
        $('#menuNamesTable').DataTable({
            ajax: {
                url: '/Home/GetMenuNamesJson',
                dataSrc: 'data'
            },
            columns: [
                { data: 'menuId' },
                { data: 'menuNameValue' },
                {
                    data: null,
                    render: function (data, type, row) {
                        return `<a href="javascript:void(0);" class="detail-link" data-menuid="${row.menuId}">상세</a>`;
                    }
                }
            ]
        });

        // 상세 링크 클릭 시
        $('#menuNamesTable tbody').on('click', '.detail-link', function () {
            currentMenuId = $(this).data('menuid');
            var modal = new bootstrap.Modal(document.getElementById('detailModal'));
            modal.show();

            // 상세 Datatable 생성/갱신
            if ($.fn.DataTable.isDataTable('#menusTable')) {
                $('#menusTable').DataTable().destroy();
            }
            $('#menusTable').DataTable({
                ajax: {
                    url: '/Home/GetMenusByMenuIdJson',
                    data: { menuId: currentMenuId },
                    dataSrc: 'data'
                },
                columns: [
                    { data: 'menuId' },
                    { data: 'menuTypeId' },
                    { data: 'ingredientComboId' },
                    {
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-warning edit-ingredient-btn" data-rowidx="${meta.row}">수정</button>`;
                        }
                    }
                ],
                destroy: true
            });
        });

        // 수정 버튼 클릭 시 IngredientCombo 선택 모달
        $(document).on('click', '.edit-ingredient-btn', function () {
            editingRowIndex = $(this).data('rowidx');
            var modal = new bootstrap.Modal(document.getElementById('ingredientComboModal'));
            modal.show();

            // IngredientCombo 목록 Datatable
            if ($.fn.DataTable.isDataTable('#ingredientComboTable')) {
                $('#ingredientComboTable').DataTable().destroy();
            }
            $('#ingredientComboTable').DataTable({
                ajax: {
                    url: '/Home/GetIngredientCombosJson',
                    dataSrc: 'data'
                },
                columns: [
                    { data: 'ingredientComboId' },
                    { data: 'ingredient1' },
                    { data: 'ingredient2' },
                    { data: 'ingredient3' }
                ]
            });

            // IngredientCombo 선택 시
            $('#ingredientComboTable tbody').off('click').on('click', 'tr', function () {
                var comboData = $('#ingredientComboTable').DataTable().row(this).data();
                var menusTable = $('#menusTable').DataTable();
                var rowData = menusTable.row(editingRowIndex).data();
                rowData.ingredientComboId = comboData.ingredientComboId;
                menusTable.row(editingRowIndex).data(rowData).draw(false);
                var modal = bootstrap.Modal.getInstance(document.getElementById('ingredientComboModal'));
                modal.hide();
            });
        });

        // Save 버튼 클릭 시 DB 반영
        $('#saveMenusBtn').click(function () {
            var menusTable = $('#menusTable').DataTable();
            var allData = menusTable.rows().data().toArray();
            $.ajax({
                url: '/Home/SaveMenusByMenuId',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    menuId: currentMenuId,
                    menus: allData
                }),
                success: function (result) {
                    alert('저장되었습니다.');
                    var modal = bootstrap.Modal.getInstance(document.getElementById('detailModal'));
                    modal.hide();
                },
                error: function () {
                    alert('저장 실패');
                }
            });
        });
    });
</script>
```
