# Publishing

[← Back to README](../README.md)

Інструкція для розробників бібліотеки — як зібрати та опублікувати нову версію NuGet-пакета після внесення змін.

## 1. Оновити версію

Версія задається у `.csproj` бібліотеки (`SentinelLib.Monitoring.SDK.csproj`):

```xml
<PropertyGroup>
  <Version>1.1.0</Version>
</PropertyGroup>
```

Дотримуйтесь [Semantic Versioning](https://semver.org/):

| Тип зміни | Приклад | Версія |
| --- | --- | --- |
| Breaking change (несумісна зміна API) | зміна сигнатури публічного методу, видалення члена | major (`1.x.x` → `2.0.0`) |
| Нова функціональність (сумісна) | новий overload, новий extension-метод | minor (`1.1.x` → `1.2.0`) |
| Виправлення багів, документація | без зміни публічного API | patch (`1.1.0` → `1.1.1`) |

## 2. Локальна перевірка перед публікацією

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

## 3. Сформувати пакет (`dotnet pack`)

```bash
dotnet pack ./SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK.csproj --configuration Release --output ./SentinelLib.Monitoring.SDK/bin/Release
```

Це створить файл на кшталт:

```
./SentinelLib.Monitoring.SDK/bin/Release/SentinelLib.Monitoring.SDK.1.1.0.nupkg
```

Переконатись, що:
- `.dll` та `.xml` (документація) присутні;
- версія відповідає очікуваній;
- README.md включено.

## 4. Опублікувати пакет у локальний feed
 
Пакети публікуються копіюванням `.nupkg` у спільну папку, зареєстровану як NuGet source.

## 5. Після публікації

- Оновити версію в `<PackageReference>` у сервісах, що використовують бібліотеку.
