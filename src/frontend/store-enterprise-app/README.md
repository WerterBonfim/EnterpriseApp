### Usando via docker. Como Dev


Build da imagem:

```
docker build -t imagem-store-app:dev .
```

Executando o container:
```
docker run \
    -it \
    -v $(pwd):/app \
    -v /app/node_modules \
    -p 3001:3000 \
    -e CHOKIDAR_USEPOLLING=true \
    -e NODE_ENV=development \
    --rm imagem-store
```



