FROM node:24-slim

# Establecer el directorio de trabajo dentro del contenedor
WORKDIR /usr/src/app

# Copiar el package.json y package-lock.json
COPY package*.json ./

# Instalar solo las dependencias de producción (excluyendo devDependencies)
RUN npm install --omit=dev

# Copiar el resto del código de la aplicación
COPY . .

# Exponer el puerto de la aplicación (asumiendo que será pasado por .env o por defecto el puerto que utilices)
EXPOSE 3000

# Comando para iniciar la aplicación
CMD ["npm", "start"]
