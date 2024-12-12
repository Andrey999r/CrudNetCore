# CrudNetCore

## Описание

**CrudNetCore** — это веб-приложение для создания и управления опросами. Оно позволяет пользователям создавать опросы, делиться ими, собирать ответы и анализировать результаты. 

Приложение разработано на **ASP.NET Core** с использованием **Entity Framework Core** и **PostgreSQL**.

---

## Быстрый старт

### Шаг 1: Убедитесь, что установлены следующие зависимости

- [Docker](https://www.docker.com/products/docker-desktop) (Docker и Docker Compose должны быть установлены и работать).

---

### Шаг 2: Запуск проекта

1. Перейдите в корень проекта (где находится файл `Docker`):
   ```bash
   cd /путь/к/проекту
   ```

2. Соберите и запустите проект с помощью Docker Compose:
   ```bash
   docker-compose up --build
   ```

3. Приложение станет доступно по адресу:  
   [http://localhost:8080](http://localhost:8080).

   **PostgreSQL** доступен на порту `5432`.

---

## Функциональность

1. **Авторизация и регистрация пользователей**
   - Создание нового аккаунта.
   - Вход в систему.

2. **Управление опросами**
   - Создание, удаление и редактирование опросов.
   - Добавление и удаление вопросов.

3. **Прохождение опросов**
   - Пользователи могут отвечать на вопросы опроса по ссылке.

4. **Анализ результатов**
   - Владелец опроса может посмотреть список участников и их ответы.

---

## Контакты

- **Email**: your-email@example.com

---

**Готово к запуску!**
