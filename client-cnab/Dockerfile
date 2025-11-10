FROM node:20-alpine AS build
WORKDIR /app
COPY package.json package-lock.json* ./
RUN npm install
COPY . .
RUN npm run build --configuration=production

FROM nginx:alpine
COPY --from=build /app/dist/client-cnab/browser /usr/share/nginx/html
COPY <<EOF /etc/nginx/conf.d/default.conf
server {
  listen 80;
  index index.html;
  root /usr/share/nginx/html;
  
  location / { 
    try_files \$uri \$uri/ @fallback;
  }
  
  location @fallback {
    rewrite ^.*\$ /index.html last;
  }
  
  location /api/ { 
    proxy_pass http://api:8080/api/; 
    proxy_set_header Host \$host;
    proxy_set_header X-Real-IP \$remote_addr;
    proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
    proxy_set_header X-Forwarded-Proto \$scheme;
  }
}
EOF
