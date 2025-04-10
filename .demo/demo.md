# Docker Demo Básica

## Paso 1: Crear un archivo Dockerfile
Este archivo define el contenido y comportamiento de una imagen personalizada.

**Dockerfile**
```dockerfile
# Imagen base
FROM python:3.9-slim

# Directorio de trabajo dentro del contenedor
WORKDIR /app

# Copiar archivo local al contenedor
COPY app.py .

# Comando para ejecutar la aplicación
CMD ["python", "app.py"]
```

Junto con este Dockerfile, crie o arquivo app.py:

```python
# app.py
print("¡Hola desde Docker!")
```

## 🔧 Paso 2: Construir la imagen
```bash
docker build -t demo-python .
```
- `-t demo-python`: le da un nombre a la imagen
- `.`: indica que el Dockerfile está en el directorio actual

## ▶️ Paso 3: Ejecutar la imagen como contenedor
```bash
docker run demo-python
```

## 📃 Paso 4: Ver imágenes disponibles
```bash
docker images
```

## 🛑 Paso 5: Ver y detener contenedores
```bash
# Ver contenedores (incluyendo los detenidos)
docker ps -a

# Detener un contenedor
docker stop <ID_DEL_CONTENEDOR>

# Eliminar un contenedor
docker rm <ID_DEL_CONTENEDOR>
```

## 🗑️ Paso 6: Eliminar una imagen
```bash
docker rmi demo-python
```
