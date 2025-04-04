# Survey Platform

**Survey Platform** – это современное веб-приложение, разработанное на базе **ASP.NET Core**. Оно предназначено для создания и управления опросами, хранения данных в **PostgreSQL** и отправки email-уведомлений через SMTP. Приложение обладает следующими возможностями:

- **Создание и управление опросами:** Легкий способ создавать, редактировать и просматривать опросы.
- **Хранение данных в PostgreSQL:** Надежное и масштабируемое решение для управления данными.
- **Отправка email-уведомлений:** Интеграция с SMTP-сервером для информирования пользователей.
- **Защищенный доступ к функциям приложения:** Реализация механизмов валидации и авторизации для безопасности.

---


## Требования

- **.NET Core SDK** (рекомендуется последняя LTS версия)
- **PostgreSQL** (версия 10 и выше)
- **Visual Studio** или любой другой редактор/IDE

---

## Клонирование репозитория

Склонируйте проект из публичного репозитория **GitHub** на свой компьютер, используя следующую команду:

```sh
git clone https://github.com/Andrey999r/CrudNetCore.git

```

### Установка зависимостей
- **.NET 6.0 SDK** ([официальный сайт](https://dotnet.microsoft.com/download))
- **PostgreSQL 14+** ([скачать](https://www.postgresql.org/download/))

### Настройка базы данных
1. Создайте новую БД:
```sql
CREATE DATABASE surveydb 
   ENCODING 'UTF8' 
   LC_COLLATE 'Russian_Russia.1251' 
   LC_CTYPE 'Russian_Russia.1251';
```

2. Настройте подключение в `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=surveydb;User Id=postgres;Password=ВашПароль;"
}
```

### Настройка Email (опционально)
```json
"EmailSettings": {
  "SmtpServer": "smtp.yandex.ru",
  "SmtpPort": 587,
  "SenderEmail": "noreply@surveyapp.com",
  "SenderPassword": "ВашПароль",
}
```

### Миграции базы данных
```bash
dotnet ef database update --project CrudNetCore.Infrastructure
```

### Сборка и запуск
```bash
cd smth
dotnet restore
dotnet build
dotnet run
```


## 🛠️ Дополнительные команды
| Команда | Описание |
|---------|----------|
| `dotnet ef migrations add Initial` | Создать новую миграцию |
| `dotnet watch run` | Запуск с hot-reload |
| `dotnet test` | Запуск unit-тестов |
