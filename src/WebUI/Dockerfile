FROM node:15.13-alpine
LABEL authors="Harlok"
WORKDIR /app
COPY . .
RUN npm run build
CMD ["npm", "start"]