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
