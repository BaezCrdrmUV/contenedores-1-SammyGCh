# contenedores-1-SammyGCh
contenedores-1-SammyGCh created by GitHub Classroom
 
## Contenedor de la base de datos
Se utilizó la imagen de mysql para Docker y ejecuta un script para crear la base de datos y las tablas correspondientes.

El servicio se encuentra en el archivo `docker-compose.yml` y para contruirlo se ejecuta el siguiente comando:

```sh
> docker-compose up mysql_personas
```

Para acceder a la base de datos por la consola de mysql ejecuta el siguiente comando:

```sh
> docker exec -it personas_database mysql -u adminPersonas -ppractica1
```

## Aplicación de consola
La aplicación de consola fue desarrollada en NET y tiene la finalidad de consumir los datos de la base de datos que se encuentra en el contenedor. Las opciones con las que cuenta es:

```sh
1. Consultar Personas.
2. Agregar Personas.
3. Actualizar persona.
4. Salir
```

Para ejecutar el programa desde el directorio del proyecto, ejecuta el siguiente comando:

```sh
> dotnet run 1
```
