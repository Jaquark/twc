# ✅ Use Node.js for building the Angular app
FROM node:23 AS build
WORKDIR /app

RUN npm install -g @angular/cli

# ✅ Copy package.json and install dependencies
COPY package.json package-lock.json ./
RUN npm install

# ✅ Copy all source files and build the Angular project
COPY . .
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "4000"]