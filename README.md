# Book-catalog-API

* О сервисе. 

Веб-сервис REST, предназначеный для работы с БД катлог книг. Тип аутентификации – анонимная.



* Используемые технологии

Сервис работает на основе ASP NET Core. Для хранения данных пользователей используется БД PostgreeSQL, для работы с ними ипользую расширения Npgsql.EntityFrameworkCore.PostgreSQL (работа с базой пользователей).



* Методы доступа.
 
POST /api

Отправка в сервис JSON файла с именем автора и названием книги.

GET / api / {authorName}

Отображение количества книг определенного автора, необходимо указать имя автора {authorName}.

GET / authors

Получение списка авторов.

PUT / authors

Отправка в сервис JSON файла с Id и именем автора для изменения.

DELETE / authors 

Отправка в сервис JSON файла с Id и именем автора для удаления.



GET / books

Получение списка книг.

PUT / books

Отправка в сервис JSON файла с Id и названием книги для изменения.

DELETE / book

Отправка в сервис JSON файла с Id и названием книги для удаления.






* Особености работы.
 
При запуске очищает собственную БД и удаляет загруженый файл.

* Коментарий автора.

Для тестирования отправки файла в сервис использовал Postman.
