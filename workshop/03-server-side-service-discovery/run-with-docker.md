## Delete all containers
```
// Good
$docker container stop $(docker container ps -q)
$docker container prune

// Bad
$docker rm -f $(docker ps -q -a)
```

## Deploy with docker compose
```
$docker-compose build
$docker-compose up -d
$docker-compose ps
$docker-compose scale employee=5
$docker-compose ps
$docker-compose down
```

## Deploy with docker swarm
```
$docker swarm init
$docker node ls

$docker stack deploy --compose-file docker-compose-deploy.yml demo
$docker stack services demo

$docker service scale demo_employee=5

// Delete all containers
$docker container rm -f $(docker ps -a -q)
$docker stack services demo

$docker swarm leave --force
```