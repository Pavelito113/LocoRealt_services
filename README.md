# LocoRealt

Сайт агентства недвижимости на ASP.NET Core MVC 9+

---

## О проекте
LocoRealt — современный сайт для агентства недвижимости, реализованный на ASP.NET Core MVC 9+. Позволяет публиковать, искать и фильтровать объявления о продаже и аренде недвижимости с удобным интерфейсом и адаптивным дизайном.

---

## Основные возможности
- Поиск и фильтрация объявлений по множеству параметров
- Категории и подкатегории недвижимости
- Продажа и аренда (разные режимы)
- Геофильтры: страна, регион, город, улица
- Современный UI с анимациями и Astro-стилем
- Авторизация и личный кабинет (Identity)
- Адаптивная верстка (Bootstrap 5.3+)
- Поддержка миграций и работы с БД

---

## Технологии
- **ASP.NET Core MVC 9+**
- **Entity Framework Core** (Code First)
- **Bootstrap 5.3**
- **Font Awesome**
- **SQL Server
- **jQuery** (минимально)

---

## Быстрый старт
1. Клонируйте репозиторий:
   ```sh
   git clone https://github.com/Pavelito113/LocoRealties.git
   cd LocoRealties
   ```
2. Проверьте настройки строки подключения к БД в `appsettings.json`.
3. Примените миграции (если нужно):
   ```sh
   dotnet ef database update
   ```
4. Запустите проект:
   ```sh
   dotnet run
   ```
5. Откройте в браузере: [http://localhost:5000](http://localhost:5000) (или порт, указанный в настройках)

---

## Структура проекта
- `Controllers/` — контроллеры MVC
- `Models/` — модели данных и ViewModel
- `Views/` — Razor-шаблоны (включая частичные)
- `wwwroot/` — статика (css, js, изображения, библиотеки)
- `Data/` — контекст EF и миграции
- `app.db` — файл базы данных (SQLite, для теста)

---

## .gitignore
Рекомендуется использовать стандартный .gitignore для .NET:

```
# Build results
bin/
obj/

# User-specific files
*.user
*.suo
*.userosscache
*.sln.docstates

# Logs
*.log

# Database
*.mdf
*.ldf
*.db-journal
*.sqlite
*.sqlite3
*.bak
*.tmp

# VS Code
.vscode/

# OS files
.DS_Store
Thumbs.db
```

---

## Контакты
- Автор: [Pavelito113](https://github.com/Pavelito113)
- Вопросы и предложения: через Issues на GitHub

---

**LocoRealt — ваш современный портал недвижимости!** 