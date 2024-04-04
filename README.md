# PTS
In root of solution run
========
openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:host.docker.internal"

openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "PtsDocker"

sudo cp localhost.crt /usr/local/share/ca-certificates

In root of solution create .env file
========
ASPNETCORE_ENVIRONMENT="{some_env}"

For start
========
docker compose up --build
