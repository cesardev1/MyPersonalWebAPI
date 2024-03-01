# MyPersonalWebAP

Este proyecto es una API diseñada para gestionar proyectos personales. Incluye funcionalidades para manejar mensajes de WhatsApp, autenticación mediante JWT, y próximamente se implementarán endpoints con servicios nuevos, también tiene el propósito de servir de muestra de los conocimientos que tengo del lenguaje de C#.

![En construcción](https://img.shields.io/badge/Estatus-En%20Construccion-red)

![GitHub commit activity](https://img.shields.io/github/commit-activity/w/cesardev1/MyPersonalWebAPI)


## Requisitos

- [.NET Core 7](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Registro en facebook developer](https://developers.facebook.com/) (para uso del api de Whatsapp)

## Instrucciones de Instalación

1. Clona el repositorio: `git clone  https://github.com/cesardev1/PersonalApi`
2. Navega al directorio del proyecto: `cd MyPersonalWebAPI`
3. Ejecuta la aplicación: `dotnet run --project MyPersonalWebAPI`

## Ejemplos de Uso

### Manejo de Mensajes de WhatsApp

Este endpoint servirá para recibir y controlar los mensajes generados por whatsapp, tengo pensado también implementar un worker para el envió de platillas.

## Documentación de API

#### Endopoints

`/api/whatsapp` endpoint dedicado a la interacción con el api de whatsapp (validación de webhook y  recepción de mensajes)

`/api/user` endpoint dedicado al registro y manejo de usuarios que podrán hacer uso del api y por lo tanto de los distintos servicios que ofrecerá


## Configuración

- **Autenticación JWT:** La API utiliza autenticación mediante tokens JWT. Para obtener un token, realiza una solicitud POST al endpoint `/api/user/login` con las credenciales adecuadas.
- **Variable de entorno** Los secretos del proyecto se pueden gestionar atravez de variables de entorno del sistema operativo, dejo la lista de variables a usar y una breve descripción de cual es su utilidad a continuación:  

## Estructura del Proyecto

- `src/`: Contiene el código fuente de la API.
- `docs/`: Documentación del proyecto.(por agregar)

## Contribuciones



## Licencia

Este proyecto está bajo la licencia [MIT](LICENSE).


## Contacto

Puedes contactarme a través de [cesar.rnl.dev@gmail.com].
