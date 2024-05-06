# Use an official Node.js runtime as a parent image
FROM node:18.20.2-alpine as builder

WORKDIR /app

# Copy package.json and package-lock.json to the container
COPY WebApps/booklibrary-spa/package*.json ./

# Install app dependencies
RUN yarn

# Copy the rest of the app source code to the container
COPY WebApps/booklibrary-spa/ .

# Build the app
RUN yarn build 

# Run the app on nginx
FROM nginx:stable-alpine as runner

# Copy the build output to replace the default nginx contents
COPY --from=builder /app/dist /usr/share/nginx/html

# Expose the port the app listens on
EXPOSE 8080

# Set the build argument for the app version number
ARG APP_VERSION=0.1.0

# Set the environment variable for the app version number
ENV APP_VERSION=$APP_VERSION

# Copy the nginx configuration template to the container
COPY WebApps/booklibrary-spa/nginx.conf /etc/nginx/conf.d/nginx.conf.template

# Update the nginx configuration to use the app version number
# and Copy the nginx configuration template to the container
RUN envsubst '${APP_VERSION}' < /etc/nginx/conf.d/nginx.conf.template > /etc/nginx/conf.d/default.conf

# Start the app
CMD ["nginx", "-g", "daemon off;"]