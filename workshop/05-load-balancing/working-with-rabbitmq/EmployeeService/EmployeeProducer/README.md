## How to run ?

Start Employee Producer
```
$docker image build -t employee-producer .
$docker container run --rm --link rabbitmq employee-producer
```