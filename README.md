# SlimTrack

һ������ .NET 9 �� Blazor WebAssembly ������������׷��Ӧ�á�

## ?? ��Ŀ���

SlimTrack ��һ��������õ����ؼ�¼�͹����ߣ������û�׷�����ر仯���ơ���Ӧ�ò���ǰ��˷���ܹ����ṩֱ�۵����ݿ��ӻ��ͱ�ݵ����ݹ����ܡ�

## ? ��Ҫ����

- ? ���ؼ�¼����ӡ����º�ɾ��
- ? �������ڵ����ݿ��ӻ�ͼ��
- ? ���ر仯����ͳ��
- ? ֧�����ڷ�Χ��ѯ
- ? ÿ�ձ�ע��¼
- ? �Զ����ݾۺϺ�ͳ�Ʒ���

## ??? ����ջ

### ǰ��
- **Blazor WebAssembly** - ���� .NET 9 ���ִ��� Web ���
- **C# 13.0** - ���µ� C# ��������
- **Chart.js** - ���ݿ��ӻ���ͨ�� WeightChart �����

### ���
- **ASP.NET Core 9** - Web API ����
- **Entity Framework Core** - ORM ���
- **SQLite** - ���������ݿ�

### �ܹ�
- **����ܹ�**:
  - `SlimTrack.Client` - Blazor WebAssembly ǰ��
  - `SlimTrack.Server` - ASP.NET Core Web API ���
  - `SlimTrack.Shared` - ��������ģ�ͺ� DTO

## ?? ��Ŀ�ṹ

```
slim-track/
������ SlimTrack.Client/              # Blazor WebAssembly ǰ����Ŀ
��   ������ Pages/
��   ��   ������ Index.razor    # ��ҳ�棨���ؼ�¼����
��   ������ Shared/
��   ��   ������ EntriesTable.razor    # �����б����
��   ��   ������ WeightChart.razor     # ͼ�����
��   ������ Services/
��   ��   ������ WeightService.cs      # API �����װ
��   ������ wwwroot/
��    ������ index.html         # ��� HTML
��       ������ css/app.css           # ��ʽ�ļ�
��
������ SlimTrack.Server/       # ASP.NET Core �����Ŀ
��   ������ Program.cs    # �������ú� API �˵�
��   ������ AppDbContext.cs      # EF Core ���ݿ�������
��   ������ appdata/
��    ������ slimtrack.db          # SQLite ���ݿ��ļ�
��
������ SlimTrack.Shared/         # ������Ŀ
  ������ WeightEntryDto.cs         # ���ݴ������
```

## ?? ���ٿ�ʼ

### ǰ��Ҫ��

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- �����ִ������

### ��װ������

1. **��¡�ֿ�**
   ```bash
   git clone https://github.com/ydfk/slim-track.git
   cd slim-track
   ```

2. **���к�˷���**
   ```bash
   cd SlimTrack.Server
   dotnet run
   ```
   ��˷��������� `https://localhost:5001` �����õĶ˿ڡ�

3. **����ǰ�ˣ������������**
   ```bash
   cd SlimTrack.Client
   dotnet run
   ```

4. **����Ӧ��**
   
   ��������д�Ӧ�õ�ַ��ͨ���� `https://localhost:5001` �������õĵ�ַ����

### ���ݿ��ʼ��

Ӧ���״�����ʱ���Զ����� SQLite ���ݿ��ļ���λ�� `SlimTrack.Server/appdata/slimtrack.db`����

�����������ݿ⣬ֻ��ɾ�����ļ�����������

## ?? API �˵�

### ��ȡ���ؼ�¼�б�
```http
GET /api/weights?start=2024-01-01&end=2024-12-31
```

### ��ȡͳ������
```http
GET /api/weights/stats?days=30
```
������� N �����Сֵ�����ֵ��ƽ��ֵ�����ݵ㡣

### ��������¼�¼
```http
POST /api/weights
Content-Type: application/json

{
  "date": "2024-01-15",
  "weightKg": 70.5,
  "note": "��ͺ�"
}
```
**ע��**��ͬһ����ֻ����һ����¼��Upsert ��������

### ɾ����¼
```http
DELETE /api/weights/{id}
```

## ?? ʹ��˵��

1. **��¼����**������ҳ����ѡ�����ڡ��������أ�1-500kg������ѡ�ע
2. **�鿴����**��ҳ���в���ͼ��չʾ��� 60 ������ر仯����
3. **��������**���������б��в鿴���м�¼��֧��ɾ������
4. **���¼�¼**��ѡ���Ѵ��ڵ����ڲ��ύ�����Զ����¸����ڵ�����

## ?? ����˵��

### ���ݿ�·��
�޸� `SlimTrack.Server/Program.cs` �е����ݿ�·����
```csharp
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "appdata", "slimtrack.db");
```

### ͼ����ʾ����
�޸� `SlimTrack.Client/Pages/Index.razor` �е�����������
```razor
<WeightChart Days="60" />
```

## ?? ����ģ��

### WeightEntry�����ݿ�ʵ�壩
- `Id` (int) - ����
- `Date` (DateOnly) - ��¼���ڣ�Ψһ������
- `WeightKg` (decimal) - ���أ�ǧ�ˣ����� 5,2��
- `Note` (string?) - ��ע����� 200 �ַ���
- `CreatedAt` (DateTime) - ����ʱ��
- `UpdatedAt` (DateTime) - ����ʱ��

## ?? ����

��ӭ�ύ Issue �� Pull Request��

## ?? ���֤

����Ŀ���� MIT ���֤����� [LICENSE](LICENSE) �ļ���

## ?? ����

**ydfk**

- GitHub: [@ydfk](https://github.com/ydfk)
- Repository: [slim-track](https://github.com/ydfk/slim-track)

## ?? ��л

��л���п�Դ�����Ĺ����ߺ�֧���ߣ�

---

**Happy Tracking! ??**
