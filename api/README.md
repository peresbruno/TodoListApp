### User End-Point

 - `GET /api/users` - Get a list of all users.
 - `GET /api/users/{userId}` - Get a representation of the user specified by `{userId}` paremeter.
 - `POST /api/users` - Add a new user based on the request body content (Public route).
 - `PUT /api/users/{userId}` - Edit an user specified by `{userId}` parameter and based on the request body content.
 - `DELETE /api/users/{userId}` - Remove an user specified by `{userId}` parameter.
 - `POST /api/users/sign-in` - Create an JWT token to be used for authentication purposes (Public route).

### Todos End-Point

 - `GET /api/users/{userId}/todos` - Get a list of all todos for a given user specified by `{userId}` parameter.
 - `GET /api/users/{userId}/todos/{todoId}` - Get a todo item specified by `{todoId}` parameter from `{userId}` user. 
 - `POST /api/users/{userId}/todos` - Add a new todo item based on the request body for a user specified by `{userId}`.
 - `PUT /api/users/{userId}/todos/{todoId}` - Edit a todo item based on the request body for a user specified by `{userId}`.  
 - `DELETE /api/users/{userId}/todos/{todoId}` - Remove a todo item from an user with id `{userId}`.
 
#### User payload example:

`{
    "userName" : "Bruno",
    "lastName" : "Peres",
    "age" : 28,
    "email" : "brunoperes@email.com",
    "password" : "Abc#123"
}`

#### Todo item payload example:

`{
    "description" : "A todo item...",
    "done" : false
}`


#### Credential payload example:

`{
    "email" : "bruno@email.com",
    "password" : "Abc@123"
}`
