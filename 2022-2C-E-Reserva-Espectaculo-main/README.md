# 2022-2C-E-Reserva-Espectaculo


## Objetivos 
Desarrollar un sistema, que permita la administración general del cine (de cara a los empleados): Peliculas, Salas, , Funciones, Clientes, etc., como así también, permitir a los clientes, realizar reserva de las funciones ofrecidas.
Utilizar Visual Studio 2019 preferentemente y crear una aplicación utilizando ASP.NET MVC Core (versión a definir por el docente 3.1 o 6).

<hr />

## Enunciado 
La idea principal de este trabajo práctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relevó e identificó la información aquí contenida. 
A partir de este momento, deberán comprender lo que se está requiriendo y construir dicha aplicación, 

Deben recopilar todas las dudas que tengan y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, él nos ayudará a conseguir la información ya un poco más procesada. 
Es importante destacar, que este proceso, no debe esperar a ser en clase; es importante, que junten algunas consultas, sea de índole funcional o técnicas, en lugar de cada consulta enviarla de forma independiente.

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relación del paciente con Turno es 1:1 o 1:N?

2.Está bien que encaremos la validación del turno activo, con una propiedad booleana en el Turno?

<hr />

### Proceso de ejecución en alto nivel 
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/).
 - Adicionar todos los modelos dentro de la carpeta Models cada uno en un archivo separado.
 - Especificar todas las restricciones y validaciones solicitadas a cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Crear las relaciones entre las entidades
 - Crear una carpeta Data que dentro tendrá al menos la clase que representarив el contexto de la base de datos DbContext. 
 - Crear el DbContext utilizando base de datos en memoria (con fines de testing inicial). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades en el DbContext.
 - Crear el Scaffolding para permitir los CRUD de las entidades al menos solicitadas en el enunciado.
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Realizar un sistema de login con al menos los roles equivalentes a <Usuario Cliente> y <Usuario Administrador> (o con permisos elevados).
 - Si el proyecto lo requiere, generar el proceso de registración. 
 - Un administrador podrá realizar todas tareas que impliquen interacciиоn del lado del negocio (ABM "Alta-Baja-Modificaciиоn" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente> sólo podrá tomar acción en el sistema, en base al rol que tiene.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos del lado de los permisos.
 - Todo lo referido a la presentación de la aplicaión (cuestiones visuales).
 
<hr />

## Entidades 

- Usuario
- Cliente
- Empleado
- Reserva
- Función
- Pelicula
- Sala
- Genero

`Importante: Todas las entidades deben tener su identificador unico. Id o <ClassNameId>`

`
Las propiedades descriptas a continuación, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como así también las restricciones.
`

**Usuario**
```
- Nombre
- Email
- FechaAlta
- Password
```

**Cliente**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email 
- Reservas
```

**Empleado**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email
- Legajo
```

**Pelicula**
```
- FechaLanzamiento
- Titulo
- Descripcion
- Genero
- Funciones
```
**Genero**
```
- Nombre
- Peliculas
```

**Sala**
```
- Numero
- TipoSala
- CapacidadButacas
- Funciones
```

**TipoSala**
```
- Nombre
- Precio
```

**Función**
```
- Fecha
- Hora
- Descripcion
- ButacasDisponibles
- Confirmada
- Pelicula
- Sala
- Reservas
```


**Reserva**
```
- Funcion
- FechaAlta
- Cliente
- CantidadButacas
```

**NOTA:** aquí un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades 
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

**Usuario**
- Los clientes pueden auto registrarse.
- La autoregistración desde el sitio, es exclusiva para los clientes. Por lo cual, se le asignará dicho rol.
- Los empleados, deben ser agregados por otro Empleado.
	- Al momento, del alta del empleado, se le definirá un username y password.
    - También se le asignará a estas cuentas el rol de empleado.

**Cliente**
- Un cliente puede realizar una reserva Online
    - El proceso será en modo Wizard.
        - Selecciona la pelicula
        - Selecciona la cantidad de butacas que quiere reservar.
        - Seleccionará una función disponible dentro de la oferta.
            - La oferta estará restringida desde el momento de la consulta hasta 7 dias posteriores.
            - Las funciones deben estar confirmadas
            - No debe incluir desde luego funciones que no tenga butacas disponibles.            
            - Debe ser en base a la oferta de la pelicula seleccionada.
            - El cliente, solo puede tener una reserva activa.
        - El cliente, podrá en todo momento, ver si tiene o no una reserva para una función futura.            
            - Podrá cancelarla, solo si es hasta 24hs. antes.
- Puede ver las reservas pasadas.
- Puede actualizar datos de contacto, como el telefono, dirección,etc.. Pero no puede modificar su DNI, Nombre, Apellido, etc.

**Empleado**
- El empleado puede listar las reservas por cada función "en el futuro" o "en el pasado".
- El empleado, puede habilitar o cancelar funciones. 
    - Solo pueden cancelarse, si no tiene reservas.
- También, puede ver un balance de recaudación por pelicula en mes calendario. 
- Puede dar de alta las Salas, Peliculas, etc. 
    - Nadie, puede eliminar las salas, pero si puede cambiar el tipo.


**Reserva**
- La reserva al crearse debe estar en estado activa.
- El cliente solo puede tener una reserva activa.
- La reserva, tiene que validar, que sea factible, en cuanto a la cantidad de butacas que selecciona al cliente para una función especifica.
    - Si puede realizar la reserva se debe actualizar las butacas disponibles (Capacidad de la sala vs Reservas realizadas previas y actual).


**Aplicación General**
- Información institucional.
- Se deben listar las peliculas en cartelera sin iniciar sesión.
- Poder reservar, automáticamente desde la una pelicula (con solicitud de inicio de sesión previo si fuese requerido)
- Por cada pelicula, se tiene que poder listar las funciones activas para la próxima semana. 
- La disponibilidad de las funciones, solo puede verse al tener una sesión iniciada como cliente.


`
Nota: Las butacas no son numeradas. El complejo, no tiene limites fisicos en la construcción de salas. Las funciones tienen una duración fija de 2hs. 

![Diagrama UML](https://raw.githubusercontent.com/florenciazabala/PNT1-Diagram/main/Diagram%202022-09-18%2022-13-53.png)
