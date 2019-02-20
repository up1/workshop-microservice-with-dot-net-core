## How to run ?

1. Start Registry server (Consul)
```
$docker container run --rm --name=consul -e CONSUL_BIND_INTERFACE=eth0 -e CONSUL_CLIENT_INTERFACE=eth0 -p 8500:8500 consul
```

Open url=http://localhost:8500 in browser

2. Start and register service
```
$docker image build -t point-to-point .
$docker container run --rm -p 8080:80 --name employee --link consul:consul point-to-point
```

Open url=http://localhost:8080/ in browser