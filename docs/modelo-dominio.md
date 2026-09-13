# Modelo de dominio del sistema

## Convenciones y nomenclatura

El nombre de todas las tablas se escribe en minúscula. Aquellas que involucren dos palabras, serán separadas por un guión bajo.
Los atributos siguen la misma nomenclatura.

## Tablas del sistema (permisos, usuarios, roles)

USUARIO 
{
  int id PK
  string nombre
  string apellido
  string email UK
  string password_hash
  boolean activo
}

ROL
{
  int id PK
  string nombre UK
  string descripcion
}

PERMISO 
{
  int id PK
  string descripcion
}

USUARIO_ROL 
{
  int id_usuario PK, FK
  int id_rol PK, FK
}

PERMISO_ROL 
{
  int id_rol PK, FK
  int id_permiso PK, FK
}

## Tablas del negocio

ORIGEN_COMERCIAL 
{
  int id PK
  string nombre UK
}

ESTADO_CLIENTE 
{
  int id PK
  string descripcion
}

ESTADO_OPORTUNIDAD
{
  int id PK
  string descripcion
}

MOTIVO_RECHAZO
{
  int id PK
  string descripcion
}

ACTIVIDAD
{
  int id PK
  string descripcion
}

ETAPA_COMERCIAL
{
  int id PK
  string descripcion
}

SERVICIO
{
  int id PK
  string nombre
  string descripción
  decimal precio_referencia
}

EMPRESA 
{
  int id PK
  string razon_social
  string cuit UK
  string industria
  string correo
  string telefono
  string direccion
  string observaciones
  int id_origen FK
  int id_estado FK
}

CONTACTO
{
  int id PK
  string nombre
  string apellido
  string documento UK
  string cargo
  string correo
  string telefono
  string observaciones
  int id_empresa FK
  int id_origen FK
  int id_estado FK
  smallint edad
}

ETAPA_COMERCIAL 
{
  int id PK
  string nombre UK
  int orden UK
}

OPORTUNIDAD 
{
int id PK
string titulo
int id_usuario FK
int id_empresa FK
int id_contacto FK
int id_etapa FK
int fecha_estimada_cierre
datetime fecha_cierre_real
int id_origen FK
int id_estado FK
string observaciones
string id_motivo_perdida FK
}

OPORTUNIDAD_ITEM
{
  int id_oportunidad PK, FK
  int id_servicio PK, FK
  int cantidad
  decimal precio_unitario
  decimal descuento
}

ACTIVIDAD_OPORTUNIDAD 
{
 int id
 int tipo_actividad_id FK
 int usuario_id FK
 int empresa_id FK
 int contacto_id FK
 int oportunidad_id FK
 datetime fecha_hora
 string descripcion
 string resultado
}

HISTORIAL_ETAPA 
{
  int oportunidad_id PK, FK
  int id_etapa_anterior FK
  int id_etapa_nueva FK
  int id_usuario FK
  datetime fecha
  string observación
}

LOG_OPORTUNIDAD_CAMBIO 
{
  int id PK
  int id_oportunidad FK
  int id_usuario FK
  string campo
  string valor_anterior
  string valor_nuevo
  datetime fecha_hora
}

## Relaciones

* 1 Usuario tiene 1 Rol
* Múltiples Roles tienen múltiples permisos (Permiso_Rol)
* Múltiples Oportunidades tienen múltiples actividades (Actividad_Oportunidad)
* 1 Oportunidad tiene 1 Usuario responsable
* 1 Oportunidad tiene 1 Etapa_Comercial actual
* 1 Oportunidad puede tener muchos Servicios asociados
* 1 Oportunidad tiene asociada 1 Contacto o Empresa

## Reglas generales del negocio

* Cada oportunidad deberá tener una única etapa actual.
* La etapa deberá ser compatible con el estado de la oportunidad.
* Una oportunidad abierta no podrá estar en una etapa ganada o perdida.
* Una oportunidad ganada deberá registrar la fecha real de cierre.
* Una oportunidad perdida deberá registrar la fecha real de cierre y el motivo de pérdida.
* Cada cambio deberá conservarse en el historial.
* Una oportunidad cerrada no podrá volver a una etapa abierta sin autorización.
* Si se modifica una oportunidad cerrada, el cambio deberá quedar registrado
* Toda oportunidad deberá tener un responsable.
* Toda oportunidad deberá estar asociada, como mínimo, con una empresa o un contacto.
* Toda oportunidad deberá tener una etapa actual.
* Una oportunidad abierta deberá encontrarse en una etapa abierta.
* Una oportunidad ganada deberá registrar fecha de cierre y valor final, si el negocio utiliza valores monetarios.
* Una oportunidad perdida deberá registrar fecha de cierre y motivo de pérdida.
* Cada cambio de etapa deberá conservarse en el historial.
* Cada actividad deberá registrar el usuario y la fecha.
* Las actividades deberán relacionarse con una empresa, contacto u oportunidad.
* Los registros con historial comercial no deberán eliminarse físicamente.
* Los vendedores solo podrán acceder a la información permitida por su rol.
* Los permisos deberán validarse en el backend y no solamente en el frontend.
* Las contraseñas deberán almacenarse utilizando un mecanismo seguro.
* Una oportunidad cerrada no deberá modificarse sin autorización.
* Los cambios importantes deberán permitir identificar al usuario que los realizó.