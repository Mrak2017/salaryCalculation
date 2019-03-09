Для запуска в первый раз нужно создать базу данных:
1) Tools > NuGet Package Manager > Package Manager Console
команда:
Add-Migration InitialCreate
2) В той же консоли, команда:
Update-Database