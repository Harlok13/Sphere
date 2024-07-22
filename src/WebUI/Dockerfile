FROM node:15.13-alpine
LABEL authors="Harlok"
WORKDIR /app
COPY . .
RUN ls -a "/app"
RUN npm run build
CMD ["npm","start"]