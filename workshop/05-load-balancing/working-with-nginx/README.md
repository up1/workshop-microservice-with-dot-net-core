## Load Balancer with NGINX

Step 1 :: Create multi-containers of Employee service (Use image from workshop 01)
```
$docker image build -t employee-service .
$docker container run --name employee-service01 employee-service
$docker container run --name employee-service02 employee-service
$docker container run --name employee-service03 employee-service
```

Step 2 :: Create nginx as Load Balancer
```
$docker-compose up
$docker-compose down
```