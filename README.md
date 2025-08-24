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
