## Цель
Cпроектировать функционал для информирования клиентов о статусах их заказов.

## Безопасность
Для регулирования доступа к информации о заказах предлагаю использовать ASP.NET Core Identity (далее Identity) и Bearer Authentication (JWT). Identity позволит использовать проверенные подходы к безопасности и избавит от необходимости писать стандартный код. Bearer Authentication позволит аутентифицировать различных клиентов: от веб-сайтов до других сервисов, а JWT сделает возможным реализовать авторизацию на основе claims.

Важно отметить, что данный подход легко интегрируется в pipeline исполнения запроса в ASP .NET Core (встроенный `JwtBearerMiddleware` умеет дисериализовывать токен, читая его из заголовка Authorization, а также валидировать его).

### REST API
В качестве базовой реализации предлагаю предоставить клиентам REST API с возможностью получить информацию о заказах. API будет содержать следующие методы:
* `POST /clients [AllowAnonymous]` Регистрирует нового клиента. Основная работа делигируется Identity.
* `POST auth/login` Проверяет учетные данные клиента и если они валидны - возвращает access token и refresh token.
* `POST auth/refreshtoken [AllowAnonymous]` Создает новый access token и refresh token в случае, если переданный access token валиден во всем, кроме Lifetime.
* `GET /orders` возвращает список заказов авторизованного клиента. Необходимо поддержать фильтрацию, сортировку, и постраничную отдачу.

### Web Sockets
Если требуется уведомлять клиентов со стороны сервиса учета заказов, предлагаю расширить API сервиса возможностью установить веб сокет соединение. Протокол веб сокетов не накладывает ограничений на правила аутентификации, поэтому возможно использовать тот же подход, что и в REST API.

Недостатком данного подхода является является его относительная сложность при интеграции.

### Web hooks
Еще одним вариантом уведомления клиентов о новых заказах является использование Web hooks. Клиент регистрирует URI, по которому хочет получать уведомления об изменених статусов, после чего сервис учета заказов будет присылать по этому адресу POST запросы содержащие описание изменения.

Преимуществом данного подхода является его простота для клиентов. Недостатком - сложность управления доступом на стороне сервиса учета заказов и, как следствие, риски безопасности.

### Имплементация модуля
Для имплементации я выбрал сервисы для генерации и валидации JWT. Исходный код находится в этом репозитории.