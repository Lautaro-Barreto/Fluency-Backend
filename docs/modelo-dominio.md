# Modelo de dominio del sistema

> Este documento refleja el esquema tal como está definido en `Persistence/SQL/creacion-tablas.sql` y
> mapeado en `Persistence/Models/`. Es la fuente de verdad actual del esquema; la sección "Brechas conocidas"
> al final señala dónde este esquema todavía no cubre reglas de negocio que sí están definidas para el
> sistema.

## Convenciones y nomenclatura

El nombre de todas las tablas se escribe en minúscula. Aquellas que involucren dos palabras, serán separadas por un guión bajo.
Los atributos siguen la misma nomenclatura.

## Tablas del sistema (usuarios)

USUARIO
{
  int id PK
  string nombre
  string apellido
  string correo UK
  string username UK
  string password_hash
  boolean activo
}

## Catálogos

ORIGEN_COMERCIAL
{
  int id PK
  string descripcion
}

ESTADO_CLIENTE
{
  int id PK
  string descripcion
}

NIVEL_INGLES
{
  int id PK
  string descripcion
}

MODALIDAD
{
  int id PK
  string descripcion
}

## Tablas del negocio

EMPRESA
{
  int id PK
  string razon_social
  string cuit
  string industria
  string correo
  string telefono
  string direccion
  int id_estado FK
  int id_origen FK
  string observaciones
}

CONTACTO
{
  int id PK
  string nombre
  string apellido
  string documento
  string cargo
  string correo
  string telefono
  int id_estado FK
  int id_origen FK
  int id_empresa FK
  string observaciones
}

SERVICIO
{
  int id PK
  string nombre
  string descripcion
  decimal precio_referencia
  int duracion_horas
  int id_nivel FK
  int id_modalidad FK
  boolean activo
}

ETAPA_COMERCIAL
{
  int id PK
  string nombre
  string descripcion
  int orden
}

OPORTUNIDAD
{
  int id PK
  string titulo
  int id_usuario FK
  int id_empresa FK
  int id_contacto FK
  int id_servicio FK
  int id_etapa FK
  date fecha_estimada_cierre
  datetime fecha_cierre
  int id_origen FK
  int id_estado FK
  string observaciones
}

OPORTUNIDAD_ITEM
{
  int id PK
  int id_oportunidad FK
  int id_servicio FK
  int cantidad
  decimal precio_unitario
  decimal descuento
}

HISTORIAL_ETAPAS
{
  int id PK
  int id_oportunidad FK
  int id_etapa_anterior FK
  int id_nueva_etapa FK
  datetime fecha
  int id_usuario FK
  string observacion
}

## Relaciones

* 1 Usuario puede ser responsable de muchas Oportunidades (`oportunidad.id_usuario`).
* 1 Empresa puede tener muchos Contactos (`contacto.id_empresa`, se pone en null si se borra la empresa).
* 1 Empresa puede tener muchas Oportunidades; 1 Contacto puede tener muchas Oportunidades.
* 1 Oportunidad tiene 1 Etapa_Comercial actual (`oportunidad.id_etapa`).
* 1 Oportunidad puede referenciar 1 Servicio principal (`oportunidad.id_servicio`) y, a través de
  Oportunidad_Item, muchos Servicios con cantidad/precio/descuento propios.
* 1 Oportunidad acumula muchos registros en Historial_Etapas, uno por cada cambio de etapa
  (`id_etapa_anterior` → `id_nueva_etapa`).
* Empresa, Contacto y Oportunidad comparten los catálogos Origen_Comercial y Estado_Cliente.
* Servicio se clasifica por Nivel_Ingles y Modalidad.

## Reglas generales del negocio

* Cada oportunidad deberá tener una única etapa actual.
* La etapa deberá ser compatible con el estado de la oportunidad.
* Una oportunidad abierta no podrá estar en una etapa ganada o perdida.
* Una oportunidad ganada deberá registrar la fecha real de cierre.
* Una oportunidad perdida deberá registrar la fecha real de cierre y el motivo de pérdida.
* Cada cambio deberá conservarse en el historial.
* Una oportunidad cerrada no podrá volver a una etapa abierta sin autorización.
* Si se modifica una oportunidad cerrada, el cambio deberá quedar registrado.
* Toda oportunidad deberá tener un responsable.
* Toda oportunidad deberá estar asociada, como mínimo, con una empresa o un contacto.
* Toda oportunidad deberá tener una etapa actual.
* Una oportunidad abierta deberá encontrarse en una etapa abierta.
* Una oportunidad ganada deberá registrar fecha de cierre y valor final, si el negocio utiliza valores monetarios.
* Una oportunidad perdida deberá registrar fecha de cierre y motivo de pérdida.
* Cada cambio de etapa deberá conservarse en el historial.
* Los registros con historial comercial no deberán eliminarse físicamente.
* Los vendedores solo podrán acceder a la información permitida por su rol.
* Los permisos deberán validarse en el backend y no solamente en el frontend.
* Las contraseñas deberán almacenarse utilizando un mecanismo seguro.
* Una oportunidad cerrada no deberá modificarse sin autorización.
* Los cambios importantes deberán permitir identificar al usuario que los realizó.

## Brechas conocidas entre el esquema actual y estas reglas

El esquema en `creacion-tablas.sql` todavía no modela por completo algunas de las reglas de arriba;
quedan pendientes para una futura migración:

* No existen tablas de roles/permisos (`ROL`, `PERMISO`, `USUARIO_ROL`, `PERMISO_ROL`): `usuario` es una
  tabla plana sin ningún vínculo a roles, por lo que "los vendedores solo podrán acceder a la información
  permitida por su rol" no está soportado a nivel de datos todavía.
* No existe una tabla `MOTIVO_RECHAZO` ni una columna equivalente en `oportunidad`, por lo que el motivo de
  pérdida de una oportunidad no tiene dónde persistirse.
* No existen tablas de actividades (`ACTIVIDAD`, `ACTIVIDAD_OPORTUNIDAD`) ni de auditoría de cambios
  (`LOG_OPORTUNIDAD_CAMBIO`).
* `oportunidad.id_empresa` y `oportunidad.id_contacto` son ambos nullable sin ningún `CHECK` que obligue a
  que al menos uno esté presente.
* `etapa_comercial` no distingue explícitamente si una etapa es "abierta", "ganada" o "perdida" (solo tiene
  `orden`), por lo que la compatibilidad etapa/estado no puede validarse solo con el esquema.
