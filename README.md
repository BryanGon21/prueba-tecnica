## Características principales

- CRUD completo de libros
- Autenticación JWT con roles (`admin`, `user`)
- Validaciones con FluentValidation
- Logging y manejo global de errores
- Versionado de API (v1)
- Documentación Swagger
- Tests unitarios con xUnit
- Dockerfile

---

## Estructura del proyecto

- **LibraryAPI.API**: Capa de presentación (Web API)
- **LibraryAPI.Application**: Lógica de aplicación, validaciones, tests
- **LibraryAPI.Domain**: Entidades y lógica de dominio
- **LibraryAPI.Infrastructure**: Persistencia (EF Core SQLite), servicios externos

---

## Cómo ejecutar la API

### Opción 1: Local (recomendado para desarrollo)

1. **Restaura los paquetes y compila:**
   ```sh
   dotnet restore
   dotnet build
   ```

2. **Ejecuta la API:**
   ```sh
   cd LibraryAPI.API
   dotnet run
   ```

3. Accede a Swagger en [http://localhost:5154/swagger](http://localhost:5154/swagger)  

---

### Opción 2: Docker

1. **Construye la imagen:**
   ```sh
   docker build -f LibraryAPI.API/Dockerfile -t libraryapi .
   ```

2. **Ejecuta el contenedor:**
   ```sh
   docker run -p 5000:8080 libraryapi
   ```

3. Accede a la API en [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## Autenticación y usuarios de prueba

- **Usuario admin por defecto:**
  - Usuario: `admin`
  - Email: `admin@library.com`
  - Contraseña: `admin123`
  - Rol: `admin`

### Obtener token JWT

1. Haz un POST a `/api/v1/Auth/login` con:
   ```json
   {
     "username": "admin",
     "password": "admin123"
   }
   ```
2. Copia el token de la respuesta y úsalo en Swagger o Postman como `Bearer <token>`.

---

## Endpoints principales

- `GET /api/v1/Books` — Listar libros (público)
- `GET /api/v1/Books/{id}` — Obtener libro por ID (público)
- `POST /api/v1/Books` — Crear libro (**admin**)
- `PUT /api/v1/Books/{id}` — Actualizar libro (**admin**)
- `DELETE /api/v1/Books/{id}` — Eliminar libro (**admin**)
- `PATCH /api/v1/Books/{id}/borrow` — Marcar como prestado (**user/admin**)
- `PATCH /api/v1/Books/{id}/return` — Marcar como disponible (**user/admin**)

---

## Tests

- Los tests unitarios están en `LibraryAPI.Application/Tests`.
- Para ejecutarlos:
  ```sh
  dotnet test LibraryAPI.Application
  ```

#### Ejemplo de test unitario (xUnit):

```csharp
[Fact]
public async Task Handle_Should_Create_Book_When_Valid()
{
    var request = new CreateBookRequest { ... };

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.NotNull(result);
    
}
```

---

## Notas técnicas

- La base de datos se inicializa automáticamente al arrancar la API.

---

