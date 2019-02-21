## How to run ?

Step 1 :: Start RabbitMQ
```
$docker container run --rm --name rabbitmq rabbitmq:3.7.9
```

Step 2 :: Start Employee Consumer
```
$docker image build -t employee-consumer .
$docker container run --rm -it --link rabbitmq --name consumer01 employee-consumer
$docker container run --rm -it --link rabbitmq --name consumer02 employee-consumer
$docker container run --rm -it --link rabbitmq --name consumer03 employee-consumer
```