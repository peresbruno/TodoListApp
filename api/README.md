### User End-Point

 - `GET /api/users` - Get a list of all users.
 - `GET /api/users/{id}` - Get a representation of the user specified by `{id}` paremeter.
 - `POST /api/users` - Add a new user based on the request body content.
 - `PUT /api/users/{id}` - Edit an user specified by `{id}` parameter and based on the request body content.
 - `DELETE /api/users/{id}` - Remove an user specified by `{id}` parameter.

### Todos End-Point

 - `GET /api/users/{userId}/todos` - Get a list of all todos for a given user specified by `{userId}` parameter.
 - `GET /api/users/{userId}/todos/{todoId}` - Get a todo item specified by `{todoId}` parameter from `{userId}` user. 
 - `POST /api/users/{userId}/todos` - Add a new todo item based on the request body for a user specified by `{userId}`.
 - `PUT /api/users/{userId}/todos/{todoId}` - Edit a todo item based on the request body for a user specified by `{userId}`.  
 - `DELETE /api/users/{userId}/todos/{todoId}` - Remove a todo item from an user with id `{userId}`.
 
